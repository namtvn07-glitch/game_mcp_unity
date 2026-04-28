using UnityEngine;

namespace MonsterVox.Data
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "MonsterVox/AudioConfig")]
    public class AudioConfigSO : ScriptableObject
    {
        [Header("Master Settings")]
        [Tooltip("Tốc độ nhịp nền của game (Beats Per Minute)")]
        public float masterBPM = 120f;

        [Header("Recording Settings")]
        [Tooltip("Thời gian thu âm tối đa cho phép bằng giây (Ví dụ 2.0s = đúng 4 nhịp ở 120 BPM)")]
        public float maxRecordTime = 2.0f;

        [Tooltip("Ngưỡng âm lượng tối thiểu (dB). Dưới mức này coi là khoảng lặng.")]
        public float noiseGateDB = -40f;

        [Tooltip("Tần số lấy mẫu âm thanh (Hz) khi thu từ Microphone")]
        public int microphoneSampleRate = 44100;

        [Header("Quantization")]
        [Tooltip("Mốc lưới nhịp bù trễ. Ví dụ: 1/4 note ở 120 BPM là 0.5 giây.")]
        public float snapGridInterval = 0.5f;
    }
}
