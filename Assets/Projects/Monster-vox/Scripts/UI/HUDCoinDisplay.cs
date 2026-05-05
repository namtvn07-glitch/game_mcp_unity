using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MonsterVox.Managers;

namespace MonsterVox.UI
{
    /// <summary>
    /// Displays coin count. Can show either total coins or session coins.
    /// Subscribe to EconomyManager events for reactive updates.
    /// </summary>
    public class HUDCoinDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;

        private void OnEnable()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnCoinsChanged -= HandleCoinsChanged; // Prevent double sub
                EconomyManager.Instance.OnCoinsChanged += HandleCoinsChanged;
                RefreshDisplay();
            }
        }

        private void Start()
        {
            // Fallback: If OnEnable ran before EconomyManager Awake, Instance was null.
            // Start runs after all Awakes.
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnCoinsChanged -= HandleCoinsChanged;
                EconomyManager.Instance.OnCoinsChanged += HandleCoinsChanged;
                RefreshDisplay();
            }
            else
            {
                Debug.LogWarning("[HUDCoinDisplay] EconomyManager.Instance is still NULL in Start!");
            }
        }

        private void OnDisable()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnCoinsChanged -= HandleCoinsChanged;
            }
        }

        private void HandleCoinsChanged(int totalCoins)
        {
            Debug.Log($"[HUDCoinDisplay] Coins changed to {totalCoins}. Updating UI.");
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (coinText == null || EconomyManager.Instance == null) return;

            int value = EconomyManager.Instance.Data.totalCoins;

            coinText.text = value.ToString();
        }
    }
}
