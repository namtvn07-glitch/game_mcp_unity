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
            rectTransform.position = Input.mousePosition; 
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;

            // Simple raycast check from Camera 
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Try Physics 2D first
                RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
                
                if (hit2D.collider != null)
                {
                    TryAssignToMonster(hit2D.collider.gameObject);
                    return;
                }
                else
                {
                    // Fallback to Physics 3D
                    if (Physics.Raycast(ray, out RaycastHit hit3D))
                    {
                        TryAssignToMonster(hit3D.collider.gameObject);
                        return;
                    }
                }
            }

            // Return if nothing hits
            rectTransform.anchoredPosition = originalPosition;
        }

        private void TryAssignToMonster(GameObject targetGo)
        {
            var player = targetGo.GetComponent<MonsterVox.Audio.QuantizedAudioPlayer>();
            if (player != null && VoiceClip != null)
            {
                player.ReceiveNewClip(VoiceClip);
                // Dispose UI clip icon after successfully assigned
                Destroy(gameObject);
            }
            else
            {
                rectTransform.anchoredPosition = originalPosition;
            }
        }
    }
}
