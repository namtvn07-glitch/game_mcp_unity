using System.Collections.Generic;
using UnityEngine;

namespace MonsterVox.Audio
{
    /// <summary>
    /// Implements AudioSource pooling to avoid Instantiate/Destroy GC allocations
    /// during intense quantization scheduling. 
    /// </summary>
    public class AudioSourcePool : MonoBehaviour
    {
        public static AudioSourcePool Instance { get; private set; }

        [SerializeField] private int initialSize = 5;
        private Queue<AudioSource> availableSources = new Queue<AudioSource>();

        private void Awake()
        {
            // While Singletons are generally discouraged (Anti-Singleton), 
            // a purely generic uncoupled Object Pool Service Locator is acceptable.
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;

            for (int i = 0; i < initialSize; i++)
            {
                CreateNewSource();
            }
        }

        private void CreateNewSource()
        {
            GameObject obj = new GameObject("PooledAudioSource");
            obj.transform.SetParent(this.transform);
            
            AudioSource source = obj.AddComponent<AudioSource>();
            source.spatialBlend = 0f; // Force 2D
            source.playOnAwake = false;
            
            obj.SetActive(false);
            availableSources.Enqueue(source);
        }

        public AudioSource GetSource()
        {
            if (availableSources.Count == 0)
            {
                CreateNewSource();
            }

            AudioSource source = availableSources.Dequeue();
            source.gameObject.SetActive(true);
            source.volume = 1f;
            return source;
        }

        public void ReturnSource(AudioSource source)
        {
            if (source == null) return;
            
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
            availableSources.Enqueue(source);
        }
        
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
