using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MonsterVox.Audio;

namespace MonsterVox.UI
{
    /// <summary>
    /// Hold-to-record button with visual feedback.
    /// Uses IPointerDownHandler/IPointerUpHandler for hold detection.
    /// Spawns a SoundBubbleUI when recording finishes.
    /// </summary>
    public class RecordButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("References")]
        [SerializeField] private MicrophoneRecorder microphoneRecorder;
        [SerializeField] private GameObject soundBubblePrefab;
        [SerializeField] private Transform bubbleSpawnParent;

        [Header("Visual Feedback")]
        [SerializeField] private Image buttonImage;

        private Color normalColor = new Color(0.9f, 0.15f, 0.2f);
        private Color recordingColor = new Color(1f, 0.3f, 0.4f);
        private bool isRecording;

        private void Start()
        {
            if (buttonImage != null) buttonImage.color = normalColor;
        }

        private void OnEnable()
        {
            if (microphoneRecorder != null)
            {
                microphoneRecorder.OnRecordingFinished += HandleRecordingFinished;
            }
        }

        private void OnDisable()
        {
            if (microphoneRecorder != null)
            {
                microphoneRecorder.OnRecordingFinished -= HandleRecordingFinished;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (microphoneRecorder == null) return;
            microphoneRecorder.StartRecording();
            isRecording = true;

            if (buttonImage != null) buttonImage.color = recordingColor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (microphoneRecorder == null || !isRecording) return;
            microphoneRecorder.StopRecording();
            isRecording = false;

            if (buttonImage != null) buttonImage.color = normalColor;
        }

        private void HandleRecordingFinished(AudioClip clip)
        {
            if (clip == null || soundBubblePrefab == null || bubbleSpawnParent == null) return;

            GameObject bubbleGO = Instantiate(soundBubblePrefab, bubbleSpawnParent);
            RectTransform rt = bubbleGO.GetComponent<RectTransform>();
            if (rt != null)
            {
                // Spawn above the record button
                rt.anchoredPosition = new Vector2(0, 120f);
            }

            AudioDragDropUI dragDrop = bubbleGO.GetComponent<AudioDragDropUI>();
            if (dragDrop != null)
            {
                dragDrop.VoiceClip = clip;
            }
        }

        private void Update()
        {
            // Pulsing effect while recording
            if (isRecording && buttonImage != null)
            {
                float pulse = 0.85f + Mathf.Sin(Time.time * 8f) * 0.15f;
                buttonImage.transform.localScale = Vector3.one * pulse;
            }
            else if (buttonImage != null)
            {
                buttonImage.transform.localScale = Vector3.one;
            }

#if UNITY_EDITOR
            // Editor shortcut: Space key to spawn dummy bubble for testing
            if (Input.GetKeyDown(KeyCode.Space) && microphoneRecorder != null)
            {
                // Use the same flow as if we had finished recording
                HandleRecordingFinished(Resources.Load<AudioClip>("test_dummy"));
            }
#endif
        }
    }
}
