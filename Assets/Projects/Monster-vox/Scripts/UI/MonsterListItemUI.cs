using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using MonsterVox.Data;
using MonsterVox.Managers;
using MonsterVox.Gameplay;

namespace MonsterVox.UI
{
    public class MonsterListItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("UI Elements")]
        [SerializeField] private Image monsterImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject lockOverlay;
        [SerializeField] private Button unlockCoinBtn;
        [SerializeField] private TextMeshProUGUI unlockCoinText;
        [SerializeField] private Button unlockAdsBtn;
        [SerializeField] private TextMeshProUGUI unlockAdsText;

        private MonsterDataSO data;
        private bool isUnlocked;
        private RectTransform dragIconRect;
        private GameObject dragIcon;

        public void Setup(MonsterDataSO monsterData)
        {
            this.data = monsterData;
            if (data == null) return;

            if (monsterImage != null) monsterImage.sprite = data.MonsterSprite;
            if (nameText != null) nameText.text = data.MonsterName;

            if (unlockCoinText != null) unlockCoinText.text = data.UnlockCostCoins.ToString();
            if (unlockAdsText != null) unlockAdsText.text = data.UnlockCostAds.ToString();

            if (unlockCoinBtn != null)
            {
                unlockCoinBtn.onClick.RemoveAllListeners();
                unlockCoinBtn.onClick.AddListener(OnUnlockCoinClicked);
            }

            if (unlockAdsBtn != null)
            {
                unlockAdsBtn.onClick.RemoveAllListeners();
                unlockAdsBtn.onClick.AddListener(OnUnlockAdsClicked);
            }

            RefreshState();
        }

        public void RefreshState()
        {
            if (data == null || EconomyManager.Instance == null) return;

            isUnlocked = EconomyManager.Instance.Data.IsMonsterUnlocked(data.MonsterID);

            if (lockOverlay != null) lockOverlay.SetActive(!isUnlocked);
            if (unlockCoinBtn != null) unlockCoinBtn.gameObject.SetActive(!isUnlocked);
            if (unlockAdsBtn != null) unlockAdsBtn.gameObject.SetActive(!isUnlocked);
        }

        private void OnUnlockCoinClicked()
        {
            if (EconomyManager.Instance != null && EconomyManager.Instance.TryUnlockMonster(data.MonsterID))
            {
                RefreshState();
            }
            else
            {
                Debug.LogWarning("[MonsterListItemUI] Not enough coins!");
            }
        }

        private void OnUnlockAdsClicked()
        {
            // Placeholder for Ads SDK
            Debug.Log("[MonsterListItemUI] Showing Ad... Unlocked!");
            if (EconomyManager.Instance != null)
            {
                // Fake unlock
                EconomyManager.Instance.Data.unlockedMonsterIDs.Add(data.MonsterID);
                EconomyManager.Instance.SaveProgress();
                // Trigger refresh globally
                // We'll rely on the parent list listening to OnInventoryChanged
                RefreshState();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isUnlocked || data == null) return;

            // Create a floating icon
            dragIcon = new GameObject("DragIcon");
            Canvas canvas = GetComponentInParent<Canvas>();
            dragIcon.transform.SetParent(canvas.transform, false);
            dragIcon.transform.SetAsLastSibling(); // Ensure it renders on top

            Image img = dragIcon.AddComponent<Image>();
            img.sprite = data.MonsterSprite;
            img.raycastTarget = false; // Don't block raycasts

            dragIconRect = dragIcon.GetComponent<RectTransform>();
            dragIconRect.sizeDelta = new Vector2(100, 100);
            
            // Initial position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
            dragIconRect.localPosition = localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragIconRect != null)
            {
                Canvas canvas = GetComponentInParent<Canvas>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
                dragIconRect.localPosition = localPoint;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragIcon != null)
            {
                Destroy(dragIcon);
                dragIcon = null;
            }

            if (!isUnlocked || data == null) return;

            // Raycast into 2D world space
            Camera cam = eventData.pressEventCamera != null ? eventData.pressEventCamera : Camera.main;
            if (cam == null) return;

            Vector3 worldPos = cam.ScreenToWorldPoint(eventData.position);
            worldPos.z = 0f; // Force Z to 0 for 2D overlap

            Collider2D[] cols = Physics2D.OverlapPointAll(worldPos);
            StageManager stageMgr = null;
            
            foreach (Collider2D col in cols)
            {
                SlotController slot = col.GetComponent<SlotController>();
                if (slot != null)
                {
                    if (stageMgr == null) stageMgr = FindObjectOfType<StageManager>();
                    if (stageMgr != null)
                    {
                        stageMgr.TryAssignMonsterToSlot(data, slot);
                        break;
                    }
                }
            }
        }
    }
}
