using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MonsterVox.Data;
using MonsterVox.Audio;
using MonsterVox.Gameplay;

namespace MonsterVox.UI
{
    public class NewAnimalPopupUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Image imgPortrait;
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private Button btnRecord;
        [SerializeField] private Button btnPlay;
        [SerializeField] private Button btnTryAgain;
        [SerializeField] private Button btnConfirm;
        [SerializeField] private TextMeshProUGUI txtTimer;
        
        [Header("Audio")]
        [SerializeField] private MicrophoneRecorder recorder;
        [SerializeField] private AudioClip dummyTestClip; // For editor test

        private MonsterController currentMonster;
        private MonsterDataSO currentData;
        private AudioClip recordedClip;
        private AudioSource testPlayerSource;

        private bool isRecording = false;
        private Coroutine countdownCoroutine;

        private void Awake()
        {
            if (recorder == null)
            {
                recorder = GetComponentInChildren<MicrophoneRecorder>();
                if (recorder == null)
                {
                    recorder = gameObject.AddComponent<MicrophoneRecorder>();
                }
            }

            testPlayerSource = gameObject.AddComponent<AudioSource>();

            btnRecord.onClick.AddListener(OnRecordClicked);
            btnPlay.onClick.AddListener(OnPlayClicked);
            btnTryAgain.onClick.AddListener(OnTryAgainClicked);
            btnConfirm.onClick.AddListener(OnConfirmClicked);
        }

        private void OnEnable()
        {
            if (recorder != null)
                recorder.OnRecordingFinished += HandleRecordingFinished;
        }

        private void OnDisable()
        {
            if (recorder != null)
            {
                recorder.OnRecordingFinished -= HandleRecordingFinished;
                if (isRecording)
                {
                    recorder.StopRecording();
                    isRecording = false;
                }
            }
                
            if (testPlayerSource != null && testPlayerSource.isPlaying)
                testPlayerSource.Stop();
                
        }

        public void Open(MonsterController monsterController, MonsterDataSO data)
        {
            currentMonster = monsterController;
            currentData = data;
            gameObject.SetActive(true);

            if (data != null)
            {
                if (imgPortrait != null) imgPortrait.sprite = data.MonsterSprite;
                // imgElement setup depends on how you store element icon, left out for now or assume data has it
            }

            ResetState();
        }

        private void ResetState()
        {
            recordedClip = null;
            inputName.text = "";
            txtTimer.text = "05:00";
            
            btnRecord.gameObject.SetActive(true);
            btnPlay.gameObject.SetActive(false);
            btnTryAgain.gameObject.SetActive(false);
            btnConfirm.gameObject.SetActive(false);
        }

        private void OnRecordClicked()
        {
            // Simulate dummy record in editor if dummy clip is assigned and no mic
#if UNITY_EDITOR
            if (dummyTestClip != null)
            {
                Debug.Log("[NewAnimalPopup] Using Dummy Test Clip for recording test.");
                HandleRecordingFinished(dummyTestClip);
                return;
            }
#endif
            
            if (!isRecording)
            {
                isRecording = true;
                recorder.StartRecording();
                if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
                countdownCoroutine = StartCoroutine(RecordingCountdownRoutine());
                // Change icon to stop
            }
            else
            {
                isRecording = false;
                recorder.StopRecording();
                if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
                txtTimer.text = "00:00";
            }
        }

        private IEnumerator RecordingCountdownRoutine()
        {
            float remainingTime = recorder.MaxRecordTime;
            while (remainingTime > 0)
            {
                txtTimer.text = $"Recording... {Mathf.CeilToInt(remainingTime)}s";
                yield return null;
                remainingTime -= Time.unscaledDeltaTime;
            }
            txtTimer.text = "00:00";
        }

        private void HandleRecordingFinished(AudioClip clip)
        {
            isRecording = false;
            if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
            
            recordedClip = clip;
            
            btnRecord.gameObject.SetActive(false);
            btnPlay.gameObject.SetActive(true);
            btnTryAgain.gameObject.SetActive(true);
            btnConfirm.gameObject.SetActive(true);
        }

        private void OnPlayClicked()
        {
            if (recordedClip != null && testPlayerSource != null)
            {
                testPlayerSource.clip = recordedClip;
                testPlayerSource.Play();
            }
        }

        private void OnTryAgainClicked()
        {
            if (testPlayerSource != null) testPlayerSource.Stop();
            ResetState();
        }

        private void OnConfirmClicked()
        {
            if (currentMonster != null && recordedClip != null)
            {
                // Apply name
                currentMonster.gameObject.name = string.IsNullOrEmpty(inputName.text) ? currentData.MonsterName : inputName.text;

                // Apply clip
                var qap = currentMonster.GetComponent<QuantizedAudioPlayer>();
                if (qap != null)
                {
                    qap.ReceiveNewClip(recordedClip);
                }
            }

            gameObject.SetActive(false);
        }
    }
}
