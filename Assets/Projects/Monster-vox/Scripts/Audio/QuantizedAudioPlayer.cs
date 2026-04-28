using UnityEngine;
using MonsterVox.Data;

namespace MonsterVox.Audio
{
    public class QuantizedAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioConfigSO audioConfig;

        private AudioSource mySource;
        private AudioClip assignedClip;

        private double nextEventTime;
        private float loopIntervalSeconds;
        private bool isPlaying = false;
        private const double SCHEDULE_THRESHOLD = 0.1d; // 100ms look-ahead

        public void ReceiveNewClip(AudioClip newClip)
        {
            if (newClip == null) return;
            
            // Return old source to pool if we already had one
            if (mySource != null)
            {
                AudioSourcePool.Instance.ReturnSource(mySource);
            }

            assignedClip = newClip;
            mySource = AudioSourcePool.Instance.GetSource();
            mySource.clip = assignedClip;
            
            CalculateScheduling();
        }

        private void CalculateScheduling()
        {
            if (audioConfig == null) return;

            float baseInterval = audioConfig.snapGridInterval;
            float length = assignedClip.length;
            
            // Tính số ô nhịp vừa đủ để bọc hết chiều dài của đoạn thu âm
            int gridsForLength = Mathf.CeilToInt(length / baseInterval);
            loopIntervalSeconds = gridsForLength * baseInterval;

            // Đồng bộ đồng hồ dspTime 
            double currentDsp = AudioSettings.dspTime;
            double remainder = currentDsp % baseInterval;
            double timeToNextGrid = baseInterval - remainder;
            
            // Xếp dspTime tại chính xác mốc grid kế tiếp
            nextEventTime = currentDsp + timeToNextGrid;

            mySource.PlayScheduled(nextEventTime);
            mySource.SetScheduledEndTime(nextEventTime + length);
            
            isPlaying = true;
        }

        private void Update()
        {
            // Do not generate strings or instantiate items here.
            // Pure math double calculations for zero-GC drift-free looping.
            if (!isPlaying || mySource == null || assignedClip == null) return;

            double time = AudioSettings.dspTime;
            
            // Nếu đã vượt qua ngưỡng để chuẩn bị cho queue loop tiếp theo
            if (time + SCHEDULE_THRESHOLD > nextEventTime + loopIntervalSeconds)
            {
                // Cập nhật móc thời gian sự kiện tiếp theo
                nextEventTime += loopIntervalSeconds;
                
                mySource.PlayScheduled(nextEventTime);
                mySource.SetScheduledEndTime(nextEventTime + assignedClip.length);
            }
        }

        private void OnDisable()
        {
            isPlaying = false;
            if (mySource != null && AudioSourcePool.Instance != null)
            {
                AudioSourcePool.Instance.ReturnSource(mySource);
                mySource = null;
            }
        }
    }
}
