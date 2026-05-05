using UnityEngine;
using UnityEngine.EventSystems;

namespace MonsterVox.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class AudioDragDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Vector2 originalPosition;
        
        [HideInInspector]
        public AudioClip VoiceClip; 

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPosition = rectTransform.anchoredPosition;
            canvasGroup.blocksRaycasts = false; // Let raycast pierce through to 2D/3D scene
            canvasGroup.alpha = 0.6f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Basic screen follow
            rectTransform.position = eventData.position; 
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;

            if (Camera.main != null)
            {
                // For orthographic camera: convert screen pos to world XY, then point-cast
                // ScreenPointToRay produces Z=-10 ray parallel to monsters at Z=0, which misses.
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                    new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

                Collider2D hitCollider = Physics2D.OverlapPoint(new Vector2(worldPos.x, worldPos.y));

                if (hitCollider != null)
                {
                    TryAssignToMonster(hitCollider.gameObject);
                    return;
                }
            }

            // Nothing hit — return bubble to original position
            rectTransform.anchoredPosition = originalPosition;
        }

        private void TryAssignToMonster(GameObject targetGo)
        {
            // Prefer MonsterController (new system) over raw QuantizedAudioPlayer
            var controller = targetGo.GetComponent<MonsterVox.Gameplay.MonsterController>();
            if (controller != null && VoiceClip != null)
            {
                controller.ReceiveClip(VoiceClip);
                Destroy(gameObject);
                return;
            }

            // Fallback: direct QuantizedAudioPlayer (legacy Monster_Vocalist prefab)
            var player = targetGo.GetComponent<MonsterVox.Audio.QuantizedAudioPlayer>();
            if (player != null && VoiceClip != null)
            {
                player.ReceiveNewClip(VoiceClip);
                Destroy(gameObject);
                return;
            }

            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
