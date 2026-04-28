using System;
using System.Collections;
using UnityEngine;
using MonsterVox.Data;

namespace MonsterVox.Audio
{
    public class MicrophoneRecorder : MonoBehaviour
    {
        [SerializeField] private AudioConfigSO audioConfig;

        public event Action<AudioClip> OnRecordingFinished;

        private AudioClip tempRawClip;
        private string activeMicrophoneDevice = null;
        private Coroutine recordingTimeoutCoroutine;

        private void Awake()
        {
            if (audioConfig == null)
            {
                Debug.LogError("MicrophoneRecorder: AudioConfigSO is missing.");
                return;
            }

            WarmUpMicrophone();
        }

        private void WarmUpMicrophone()
        {
            // Resolve device name without generating garbage string loops
            if (Microphone.devices.Length > 0)
            {
                activeMicrophoneDevice = Microphone.devices[0];
                // Start and IMMEDIATELY stop to allocate native buffer (anti-spike technique)
                Microphone.Start(activeMicrophoneDevice, false, 1, audioConfig.microphoneSampleRate);
                Microphone.End(activeMicrophoneDevice);
            }
            else
            {
                Debug.LogError("No microphone device found!");
            }
        }

        public void StartRecording()
        {
            if (string.IsNullOrEmpty(activeMicrophoneDevice)) return;

            // Clear previous raw clip from RAM
            CleanupRawClip();

            // Microphone.Start length argument is integer seconds
            int lengthSec = Mathf.CeilToInt(audioConfig.maxRecordTime);
            
            tempRawClip = Microphone.Start(activeMicrophoneDevice, false, lengthSec, audioConfig.microphoneSampleRate);
            
            if (recordingTimeoutCoroutine != null)
            {
                StopCoroutine(recordingTimeoutCoroutine);
            }
            recordingTimeoutCoroutine = StartCoroutine(RecordingTimeoutRoutine(audioConfig.maxRecordTime));
        }

        public void StopRecording()
        {
            if (string.IsNullOrEmpty(activeMicrophoneDevice) || !Microphone.IsRecording(activeMicrophoneDevice)) return;

            // Calculate actual hit time
            int position = Microphone.GetPosition(activeMicrophoneDevice);
            Microphone.End(activeMicrophoneDevice);

            if (recordingTimeoutCoroutine != null)
            {
                StopCoroutine(recordingTimeoutCoroutine);
                recordingTimeoutCoroutine = null;
            }

            ProcessAndDispatchClip(position);
        }

        private IEnumerator RecordingTimeoutRoutine(float maxTime)
        {
            yield return new WaitForSeconds(maxTime);
            // Time is up
            StopRecording();
        }

        private void ProcessAndDispatchClip(int rawSamplePosition)
        {
            if (tempRawClip == null) return;

            // 1. We must copy the substring of the AudioClip that was actually recorded.
            // If the user recorded for 0.5s, the rest of the clip is silence buffer.
            if (rawSamplePosition <= 0) return;

            float[] recordedData = new float[rawSamplePosition * tempRawClip.channels];
            tempRawClip.GetData(recordedData, 0);

            AudioClip clippedRaw = AudioClip.Create("RawTemp", rawSamplePosition, tempRawClip.channels, tempRawClip.frequency, false);
            clippedRaw.SetData(recordedData, 0);

            // 2. Pre-process and trim silence using the pure logic layer
            AudioClip finalTrimmedClip = AudioTrimmerUtility.TrimSilence(clippedRaw, audioConfig.noiseGateDB);

            // 3. Clear the immediate garbage 
            Destroy(clippedRaw);
            CleanupRawClip();

            // 4. Dispatch the Event for UI to pickup
            if (finalTrimmedClip != null)
            {
                OnRecordingFinished?.Invoke(finalTrimmedClip);
            }
        }

        private void CleanupRawClip()
        {
            if (tempRawClip != null)
            {
                Destroy(tempRawClip);
                tempRawClip = null;
            }
        }

        private void OnDestroy()
        {
            CleanupRawClip();
            if (Microphone.IsRecording(activeMicrophoneDevice))
            {
                Microphone.End(activeMicrophoneDevice);
            }
        }
    }
}
