using UnityEngine;

namespace MonsterVox.Data
{
    /// <summary>
    /// Singleton registry holding references to all ThemeDataSO and MonsterDataSO assets.
    /// Assign arrays via Inspector. Provides lookup methods for other systems.
    /// </summary>
    public class GameDataCatalog : MonoBehaviour
    {
        public static GameDataCatalog Instance { get; private set; }

        [Header("Content")]
        [SerializeField] private ThemeDataSO[] themes;
        [SerializeField] private MonsterDataSO[] monsters;

        // Hardcoded slot upgrade costs (from Game_Data.md)
        private static readonly int[] slotUpgradeCosts = { 0, 500, 1500 };
        private static readonly int[] slotUpgradeAdCosts = { 0, 2, 5 };

        public ThemeDataSO[] AllThemes => themes;
        public MonsterDataSO[] AllMonsters => monsters;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public ThemeDataSO GetTheme(string themeID)
        {
            if (themes == null) return null;
            for (int i = 0; i < themes.Length; i++)
            {
                if (themes[i] != null && themes[i].ThemeID == themeID)
                    return themes[i];
            }
            return null;
        }

        public MonsterDataSO GetMonster(string monsterID)
        {
            if (monsters == null) return null;
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] != null && monsters[i].MonsterID == monsterID)
                    return monsters[i];
            }
            return null;
        }

        /// <summary>
        /// Returns coin cost to upgrade TO the given level (1-indexed).
        /// Level 1 is free (default). Level 2 costs 500. Level 3 costs 1500.
        /// </summary>
        public int GetSlotUpgradeCost(int targetLevel)
        {
            if (targetLevel < 0 || targetLevel >= slotUpgradeCosts.Length) return int.MaxValue;
            return slotUpgradeCosts[targetLevel];
        }

        public int GetSlotUpgradeAdCost(int targetLevel)
        {
            if (targetLevel < 0 || targetLevel >= slotUpgradeAdCosts.Length) return int.MaxValue;
            return slotUpgradeAdCosts[targetLevel];
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
