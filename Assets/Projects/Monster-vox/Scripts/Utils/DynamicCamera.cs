using UnityEngine;

namespace MonsterVox.Utils
{
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class DynamicCamera : MonoBehaviour
    {
        [Tooltip("The fixed width in Unity units that the camera should always show.")]
        public float targetWidth = 8f;

        void Start()
        {
            UpdateCameraSize();
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                UpdateCameraSize();
            }
        }

        private void UpdateCameraSize()
        {
            Camera cam = GetComponent<Camera>();
            if (cam != null && cam.orthographic)
            {
                // Orthographic size is half of the height.
                // height = width / aspect
                // size = height / 2 = (width / aspect) / 2 = (width / 2) / aspect
                cam.orthographicSize = (targetWidth / 2f) / cam.aspect;
            }
        }
    }
}