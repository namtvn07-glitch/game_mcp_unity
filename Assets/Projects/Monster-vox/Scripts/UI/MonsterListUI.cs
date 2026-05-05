using UnityEngine;
using System.Collections.Generic;
using MonsterVox.Data;
using MonsterVox.Managers;

namespace MonsterVox.UI
{
    public class MonsterListUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform contentParent;
        [SerializeField] private MonsterListItemUI itemPrefab;

        private List<MonsterListItemUI> activeItems = new List<MonsterListItemUI>();

        private void Start()
        {
            PopulateList();
            
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnInventoryChanged += RefreshAllItems;
            }
        }

        private void OnDestroy()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnInventoryChanged -= RefreshAllItems;
            }
        }

        private void PopulateList()
        {
            if (contentParent == null || itemPrefab == null) return;

            // Clear existing
            foreach(var item in activeItems)
            {
                if (item != null) Destroy(item.gameObject);
            }
            activeItems.Clear();

            var catalog = GameDataCatalog.Instance;
            if (catalog == null || catalog.AllMonsters == null) return;

            foreach(var monsterData in catalog.AllMonsters)
            {
                if (monsterData == null) continue;

                MonsterListItemUI newItem = Instantiate(itemPrefab, contentParent);
                newItem.Setup(monsterData);
                activeItems.Add(newItem);
            }
        }

        private void RefreshAllItems()
        {
            foreach(var item in activeItems)
            {
                if (item != null) item.RefreshState();
            }
        }
    }
}
