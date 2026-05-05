using System;
using UnityEngine;
using MonsterVox.Data;

namespace MonsterVox.Managers
{
    /// <summary>
    /// Singleton managing all economy operations: coins, unlocks, and persistence.
    /// UI subscribes to OnCoinsChanged for reactive updates.
    /// </summary>
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager Instance { get; private set; }

        public PlayerData Data { get; private set; }

        /// <summary>
        /// Coins collected during the current Stage session (resets when leaving Stage).
        /// </summary>
        public int SessionCoins { get; private set; }

        /// <summary>
        /// Fired whenever totalCoins changes. Passes the new total.
        /// </summary>
        public event Action<int> OnCoinsChanged;

        /// <summary>
        /// Fired when any item is unlocked (theme, monster, or slot).
        /// </summary>
        public event Action OnInventoryChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            Data = PlayerData.Load();
            Debug.Log($"[EconomyManager] Loaded save. Coins: {Data.totalCoins}, " +
                      $"Themes: {Data.unlockedThemeIDs.Count}, Monsters: {Data.unlockedMonsterIDs.Count}");
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            Data.totalCoins += amount;
            SessionCoins += amount;
            OnCoinsChanged?.Invoke(Data.totalCoins);
        }

        public bool TrySpendCoins(int amount)
        {
            if (amount <= 0 || Data.totalCoins < amount) return false;
            Data.totalCoins -= amount;
            OnCoinsChanged?.Invoke(Data.totalCoins);
            return true;
        }

        public bool TryUnlockTheme(string themeID)
        {
            if (Data.IsThemeUnlocked(themeID)) return false;

            var catalog = GameDataCatalog.Instance;
            if (catalog == null) return false;

            ThemeDataSO theme = catalog.GetTheme(themeID);
            if (theme == null) return false;

            if (!TrySpendCoins(theme.UnlockCostCoins)) return false;

            Data.unlockedThemeIDs.Add(themeID);
            SaveProgress();
            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool TryUnlockMonster(string monsterID)
        {
            if (Data.IsMonsterUnlocked(monsterID)) return false;

            var catalog = GameDataCatalog.Instance;
            if (catalog == null) return false;

            MonsterDataSO monster = catalog.GetMonster(monsterID);
            if (monster == null) return false;

            if (!TrySpendCoins(monster.UnlockCostCoins)) return false;

            Data.unlockedMonsterIDs.Add(monsterID);
            SaveProgress();
            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool TryUpgradeSlot()
        {
            int nextLevel = Data.currentSlotLevel + 1;
            if (nextLevel > 3) return false; // Max level

            var catalog = GameDataCatalog.Instance;
            if (catalog == null) return false;

            int cost = catalog.GetSlotUpgradeCost(nextLevel);
            if (!TrySpendCoins(cost)) return false;

            Data.currentSlotLevel = nextLevel;
            SaveProgress();
            OnInventoryChanged?.Invoke();
            return true;
        }

        public void ResetSessionCoins()
        {
            SessionCoins = 0;
        }

        public void SaveProgress()
        {
            Data.Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveProgress();
            }
        }

        private void OnApplicationQuit()
        {
            SaveProgress();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
