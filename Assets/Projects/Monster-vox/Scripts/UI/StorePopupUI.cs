using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MonsterVox.Data;
using MonsterVox.Managers;

namespace MonsterVox.UI
{
    /// <summary>
    /// Store popup with 3 tabs: Themes, Monsters, Slots.
    /// Dynamically populates item grids from GameDataCatalog.
    /// </summary>
    public class StorePopupUI : MonoBehaviour
    {
        [Header("Tab Buttons")]
        [SerializeField] private Button btnTabThemes;
        [SerializeField] private Button btnTabMonsters;
        [SerializeField] private Button btnTabSlots;
        [SerializeField] private Button btnClose;

        [Header("Tab Panels")]
        [SerializeField] private GameObject panelThemes;
        [SerializeField] private GameObject panelMonsters;
        [SerializeField] private GameObject panelSlots;

        [Header("Themes Tab")]
        [SerializeField] private Transform themesGridParent;
        [SerializeField] private GameObject storeItemPrefab;

        [Header("Monsters Tab")]
        [SerializeField] private Transform monstersGridParent;

        [Header("Slots Tab")]
        [SerializeField] private TextMeshProUGUI slotLevelText;
        [SerializeField] private TextMeshProUGUI slotCostText;
        [SerializeField] private Button btnUpgradeSlot;

        private StoreTab currentTab = StoreTab.Themes;

        private void Start()
        {
            if (btnTabThemes != null) btnTabThemes.onClick.AddListener(() => SwitchTab(StoreTab.Themes));
            if (btnTabMonsters != null) btnTabMonsters.onClick.AddListener(() => SwitchTab(StoreTab.Monsters));
            if (btnTabSlots != null) btnTabSlots.onClick.AddListener(() => SwitchTab(StoreTab.Slots));
            if (btnUpgradeSlot != null) btnUpgradeSlot.onClick.AddListener(OnUpgradeSlot);
            if (btnClose != null) btnClose.onClick.AddListener(Close);
        }

        private void OnEnable()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnInventoryChanged += RefreshCurrentTab;
                EconomyManager.Instance.OnCoinsChanged += OnCoinsRefresh;
            }
            SwitchTab(StoreTab.Themes);
        }

        private void OnDisable()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnInventoryChanged -= RefreshCurrentTab;
                EconomyManager.Instance.OnCoinsChanged -= OnCoinsRefresh;
            }
        }

        private void OnCoinsRefresh(int _)
        {
            RefreshCurrentTab();
        }

        private void SwitchTab(StoreTab tab)
        {
            currentTab = tab;
            if (panelThemes != null) panelThemes.SetActive(tab == StoreTab.Themes);
            if (panelMonsters != null) panelMonsters.SetActive(tab == StoreTab.Monsters);
            if (panelSlots != null) panelSlots.SetActive(tab == StoreTab.Slots);
            RefreshCurrentTab();
        }

        private void RefreshCurrentTab()
        {
            switch (currentTab)
            {
                case StoreTab.Themes: RefreshThemes(); break;
                case StoreTab.Monsters: RefreshMonsters(); break;
                case StoreTab.Slots: RefreshSlots(); break;
            }
        }

        private void RefreshThemes()
        {
            if (themesGridParent == null || storeItemPrefab == null) return;
            var catalog = GameDataCatalog.Instance;
            var economy = EconomyManager.Instance;
            if (catalog == null || economy == null) return;

            ClearChildren(themesGridParent);

            foreach (ThemeDataSO theme in catalog.AllThemes)
            {
                if (theme == null) continue;
                GameObject item = Instantiate(storeItemPrefab, themesGridParent);
                SetupStoreItem(item, theme.ThemeName,
                    economy.Data.IsThemeUnlocked(theme.ThemeID),
                    theme.UnlockCostCoins,
                    () => { economy.TryUnlockTheme(theme.ThemeID); });
            }
        }

        private void RefreshMonsters()
        {
            if (monstersGridParent == null || storeItemPrefab == null) return;
            var catalog = GameDataCatalog.Instance;
            var economy = EconomyManager.Instance;
            if (catalog == null || economy == null) return;

            ClearChildren(monstersGridParent);

            foreach (MonsterDataSO monster in catalog.AllMonsters)
            {
                if (monster == null) continue;
                GameObject item = Instantiate(storeItemPrefab, monstersGridParent);
                SetupStoreItem(item, monster.MonsterName,
                    economy.Data.IsMonsterUnlocked(monster.MonsterID),
                    monster.UnlockCostCoins,
                    () => { economy.TryUnlockMonster(monster.MonsterID); });
            }
        }

        private void RefreshSlots()
        {
            if (EconomyManager.Instance == null) return;
            int level = EconomyManager.Instance.Data.currentSlotLevel;
            int maxSlots = EconomyManager.Instance.Data.GetMaxSlots();

            if (slotLevelText != null)
                slotLevelText.text = $"Current: {maxSlots} Slots";

            if (level >= 3)
            {
                if (slotCostText != null) slotCostText.text = "MAX";
                if (btnUpgradeSlot != null) btnUpgradeSlot.interactable = false;
            }
            else
            {
                var catalog = GameDataCatalog.Instance;
                int nextCost = catalog != null ? catalog.GetSlotUpgradeCost(level + 1) : 0;
                if (slotCostText != null) slotCostText.text = $"Upgrade: {nextCost} Coins";
                if (btnUpgradeSlot != null) btnUpgradeSlot.interactable = true;
            }
        }

        private void OnUpgradeSlot()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.TryUpgradeSlot();
            }
        }

        private void SetupStoreItem(GameObject item, string itemName, bool isUnlocked, int cost, System.Action onBuy)
        {
            // Find child components by name convention
            TextMeshProUGUI nameText = FindChildText(item, "ItemName");
            TextMeshProUGUI statusText = FindChildText(item, "ItemStatus");
            Button buyButton = FindChildButton(item, "BuyButton");

            if (nameText != null) nameText.text = itemName;

            if (isUnlocked)
            {
                if (statusText != null) statusText.text = "OWNED";
                if (buyButton != null) buyButton.gameObject.SetActive(false);
            }
            else
            {
                if (statusText != null) statusText.text = $"{cost} Coins";
                if (buyButton != null)
                {
                    buyButton.gameObject.SetActive(true);
                    buyButton.onClick.AddListener(() => onBuy?.Invoke());
                }
            }
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void ClearChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }

        private TextMeshProUGUI FindChildText(GameObject parent, string childName)
        {
            Transform child = parent.transform.Find(childName);
            return child != null ? child.GetComponent<TextMeshProUGUI>() : null;
        }

        private Button FindChildButton(GameObject parent, string childName)
        {
            Transform child = parent.transform.Find(childName);
            return child != null ? child.GetComponent<Button>() : null;
        }
    }
}
