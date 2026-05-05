using UnityEngine;
using System.Collections;
using MonsterVox.Audio;
using MonsterVox.Data;

namespace MonsterVox.Gameplay
{
    /// <summary>
    /// Controls a single monster on the stage: receives audio clips, applies voice filter,
    /// and provides placeholder "singing" animation via scale pulsing.
    /// </summary>
    [RequireComponent(typeof(QuantizedAudioPlayer))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(MonsterVox.Utils.SpriteColliderGenerator))]
    public class MonsterController : MonoBehaviour
    {
        [SerializeField] private MonsterDataSO monsterData;

        private QuantizedAudioPlayer audioPlayer;
        private SpriteRenderer spriteRenderer;
        private Vector3 baseScale;
        private bool isSinging;

        // Pulse animation settings
        private const float PULSE_AMPLITUDE = 0.1f;
        private const float PULSE_SPEED = 6f;

        public MonsterDataSO MonsterData => monsterData;
        public bool IsSinging => isSinging;

        /// <summary>
        /// Fired each time the monster's audio completes one loop cycle.
        /// StageManager subscribes to this for coin drop timing.
        /// </summary>
        public event System.Action OnLoopCompleted;

        private void Awake()
        {
            audioPlayer = GetComponent<QuantizedAudioPlayer>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseScale = transform.localScale;
        }

        private void OnEnable()
        {
            if (audioPlayer != null)
            {
                audioPlayer.OnLoopCycleCompleted += HandleLoopCompleted;
            }
        }

        private void OnDisable()
        {
            if (audioPlayer != null)
            {
                audioPlayer.OnLoopCycleCompleted -= HandleLoopCompleted;
            }
            StopSinging();
        }

        /// <summary>
        /// Initialize this monster with data. Call once when placing on a slot.
        /// </summary>
        public void Setup(MonsterDataSO data)
        {
            monsterData = data;
            if (data != null && data.MonsterSprite != null)
            {
                spriteRenderer.sprite = data.MonsterSprite;
                spriteRenderer.color = Color.white;
                
                var colliderGenerator = GetComponent<MonsterVox.Utils.SpriteColliderGenerator>();
                if (colliderGenerator != null)
                {
                    colliderGenerator.GenerateCollider();
                }
            }
        }

        /// <summary>
        /// Receives a voice clip and starts quantized playback with the appropriate voice filter.
        /// Called by AudioDragDropUI when a bubble is dropped on this monster.
        /// </summary>
        public void ReceiveClip(AudioClip clip)
        {
            if (clip == null || audioPlayer == null) return;

            // Determine mixer group based on voice filter type
            UnityEngine.Audio.AudioMixerGroup mixerGroup = GetMixerGroupForFilter();
            audioPlayer.ReceiveNewClip(clip, mixerGroup);
            
            if (!isSinging)
            {
                isSinging = true;
                StartCoroutine(SingingAnimationRoutine());
            }
        }

        private UnityEngine.Audio.AudioMixerGroup GetMixerGroupForFilter()
        {
            // Voice filter routing is handled by the AudioMixer's Vocal group.
            // For the prototype, all monsters route through the same Vocal group.
            // Per-monster pitch shifting would require per-source AudioMixer snapshots
            // which is a Phase 2 enhancement. Return null to use pool default.
            return null;
        }

        private void HandleLoopCompleted()
        {
            OnLoopCompleted?.Invoke();
        }

        private void StopSinging()
        {
            isSinging = false;
            StopAllCoroutines();
            transform.localScale = baseScale;
        }

        private IEnumerator SingingAnimationRoutine()
        {
            while (isSinging)
            {
                float pulse = 1f + Mathf.Sin(Time.time * PULSE_SPEED) * PULSE_AMPLITUDE;
                transform.localScale = baseScale * pulse;
                yield return null;
            }
            transform.localScale = baseScale;
        }
    }
}
