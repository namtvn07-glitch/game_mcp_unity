using UnityEngine;
using UnityEngine.UI;
using MonsterVox.Managers;

namespace MonsterVox.UI
{
    /// <summary>
    /// Displays coin count. Can show either total coins or session coins.
    /// Subscribe to EconomyManager events for reactive updates.
    /// </summary>
    public class HUDCoinDisplay : MonoBehaviour
    {
        [SerializeField] private Text coinText;
        [SerializeField] private bool showSessionCoins;

        private void OnEnable()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnCoinsChanged += HandleCoinsChanged;
                RefreshDisplay();
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
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (coinText == null || EconomyManager.Instance == null) return;

            int value = showSessionCoins
                ? EconomyManager.Instance.SessionCoins
                : EconomyManager.Instance.Data.totalCoins;

            coinText.text = value.ToString();
        }
    }
}
