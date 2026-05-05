using UnityEngine;
using System.Collections.Generic;

namespace MonsterVox.Gameplay
{
    /// <summary>
    /// Controls the instantiated Theme prefab, holding references to its environment components.
    /// </summary>
    public class ThemeController : MonoBehaviour
    {
        [Header("Environment")]
        [SerializeField] private SpriteRenderer backgroundRenderer;
        
        [Header("Slot Transforms")]
        [SerializeField] private List<Transform> slotTransforms = new List<Transform>();

        public SpriteRenderer BackgroundRenderer => backgroundRenderer;
        public List<Transform> SlotTransforms => slotTransforms;

        /// <summary>
        /// Adjusts the background scale to cover the main camera view.
        /// </summary>
        public void FitBackgroundToScreen(Camera mainCamera)
        {
            if (backgroundRenderer == null || backgroundRenderer.sprite == null) return;
            
            Camera cam = mainCamera != null ? mainCamera : Camera.main;
            if (cam == null) return;
            
            // Reset scale to calculate proper bounds
            backgroundRenderer.transform.localScale = Vector3.one;
            
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            
            float spriteWidth = backgroundRenderer.sprite.bounds.size.x;
            float spriteHeight = backgroundRenderer.sprite.bounds.size.y;
            
            if (spriteWidth == 0 || spriteHeight == 0) return;
            
            float scaleX = camWidth / spriteWidth;
            float scaleY = camHeight / spriteHeight;
            
            // Max to ensure it covers the whole screen without empty borders
            float scale = Mathf.Max(scaleX, scaleY);
            
            backgroundRenderer.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
