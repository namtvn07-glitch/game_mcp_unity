using UnityEngine;
using MonsterVox.Data;

namespace MonsterVox.Gameplay
{
    /// <summary>
    /// Controls a single monster slot on the stage.
    /// Handles the visual placeholder and tracks the assigned monster.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class SlotController : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private SpriteRenderer placeholderVisual;

        public MonsterController CurrentMonster { get; private set; }
        public BoxCollider2D SlotCollider { get; private set; }

        private void Awake()
        {
            SlotCollider = GetComponent<BoxCollider2D>();
            SlotCollider.isTrigger = true; // We use OverlapPoint
            
            // Ensure we have a placeholder if none assigned
            if (placeholderVisual == null)
            {
                placeholderVisual = GetComponentInChildren<SpriteRenderer>();
                if (placeholderVisual == null)
                {
                    GameObject visualObj = new GameObject("PlaceholderVisual");
                    visualObj.transform.SetParent(transform);
                    visualObj.transform.localPosition = Vector3.zero;
                    placeholderVisual = visualObj.AddComponent<SpriteRenderer>();
                    placeholderVisual.color = new Color(0, 0, 0, 0.3f); // Semi-transparent black
                    placeholderVisual.sortingOrder = -5; // Behind monsters
                }
            }
        }

        public void SetupPlaceholder(Sprite sprite)
        {
            if (placeholderVisual != null && sprite != null)
            {
                placeholderVisual.sprite = sprite;
                // Auto-size collider to match placeholder sprite
                SlotCollider.size = sprite.bounds.size;
            }
            else
            {
                SlotCollider.size = new Vector2(2f, 2f); // Default fallback size
            }
        }

        public void AssignMonster(MonsterController monster)
        {
            CurrentMonster = monster;
            if (monster != null)
            {
                monster.transform.position = transform.position;
            }
            
            // Hide placeholder when monster is assigned
            if (placeholderVisual != null)
            {
                placeholderVisual.gameObject.SetActive(false);
            }
        }

        public void ClearSlot()
        {
            CurrentMonster = null;
            // Show placeholder again
            if (placeholderVisual != null)
            {
                placeholderVisual.gameObject.SetActive(true);
            }
        }
    }
}
