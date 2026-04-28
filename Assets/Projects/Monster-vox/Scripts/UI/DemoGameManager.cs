using UnityEngine;
using UnityEngine.EventSystems;
using MonsterVox.Audio;

namespace MonsterVox.UI
{
    public class DemoGameManager : MonoBehaviour
    {
        [Header("References")]
        public MicrophoneRecorder microphoneRecorder;
        public GameObject voiceClipUIPrefab;
        public Transform canvasTransform;
        public GameObject recordButton; // Added Reference for UI Button

        [Header("Editor Debugging")]
        public AudioClip dummyTestClip;

        private void Start()
        {
            if (recordButton != null)
            {
                EventTrigger trigger = recordButton.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = recordButton.AddComponent<EventTrigger>();
                }
                
                // Add Pointer Down
                EventTrigger.Entry pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
                pointerDown.callback.AddListener((data) => { OnRecordPointerDown(); });
                trigger.triggers.Add(pointerDown);

                // Add Pointer Up
                EventTrigger.Entry pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
                pointerUp.callback.AddListener((data) => { OnRecordPointerUp(); });
                trigger.triggers.Add(pointerUp);
            }
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

        // Hook this to a UI Button using EventTrigger (PointerDown)
        public void OnRecordPointerDown()
        {
            if (microphoneRecorder != null)
            {
                microphoneRecorder.StartRecording();
            }
        }

        // Hook this to a UI Button using EventTrigger (PointerUp)
        public void OnRecordPointerUp()
        {
            if (microphoneRecorder != null)
            {
                microphoneRecorder.StopRecording();
            }
        }

        private void HandleRecordingFinished(AudioClip clip)
        {
            if (clip == null || voiceClipUIPrefab == null || canvasTransform == null) return;

            // Instantiate VoiceClipUI at screen center
            GameObject uiGO = Instantiate(voiceClipUIPrefab, canvasTransform);
            RectTransform rt = uiGO.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = Vector2.zero;
            }

            AudioDragDropUI dragDropUI = uiGO.GetComponent<AudioDragDropUI>();
            if (dragDropUI != null)
            {
                dragDropUI.VoiceClip = clip;
            }
        }

        [ContextMenu("Simulate Recording Finished (Dummy)")]
        public void SpawnDummyClip()
        {
            if (dummyTestClip != null)
            {
                HandleRecordingFinished(dummyTestClip);
                Debug.Log("[Test Mode] Spawned Dummy Voice Clip for testing drag-and-drop.");
            }
            else
            {
                Debug.LogWarning("Please assign a Dummy Test Clip in the Inspector first!");
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            // Press Space to spawn a dummy clip instantly
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnDummyClip();
            }
#endif
        }
    }
}
