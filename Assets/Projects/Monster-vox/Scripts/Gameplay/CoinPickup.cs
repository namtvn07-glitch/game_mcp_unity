using UnityEngine;
using MonsterVox.Managers;

namespace MonsterVox.Gameplay
{
    /// <summary>
    /// Attached to each coin instance. Detects tap via OnMouseDown (requires 2D Collider).
    /// Auto-returns to pool after timeout if not collected.
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class CoinPickup : MonoBehaviour
    {
        private CoinSpawner spawner;
        private float spawnTime;
        private const float AUTO_DESPAWN_TIME = 10f;

        public void Initialize(CoinSpawner ownerSpawner)
        {
            spawner = ownerSpawner;
            spawnTime = Time.time;
        }

        private void Update()
        {
            // Auto-despawn after timeout
            if (Time.time - spawnTime > AUTO_DESPAWN_TIME)
            {
                ReturnToPool();
            }
        }

        private void OnMouseDown()
        {
            Collect();
        }

        private void Collect()
        {
            // Money is already added instantly when spawned. 
            // Tapping just cleans up the visual coin early.
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (spawner != null)
            {
                spawner.ReturnToPool(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
