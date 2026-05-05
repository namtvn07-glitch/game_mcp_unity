using System.Collections.Generic;
using UnityEngine;

namespace MonsterVox.Gameplay
{
    /// <summary>
    /// Object pool for spawning coins on the stage with 2D physics.
    /// Coins are spawned at given positions with upward force for a bouncing effect.
    /// </summary>
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private int poolSize = 20;
        [SerializeField] private float spawnForceMin = 3f;
        [SerializeField] private float spawnForceMax = 5f;
        [SerializeField] private float horizontalSpread = 1f;

        private Queue<CoinPickup> pool = new Queue<CoinPickup>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                CoinPickup coin = CreateCoinObject();
                coin.gameObject.SetActive(false);
                pool.Enqueue(coin);
            }
        }

        /// <summary>
        /// Spawns coins at the given world positions (one coin per position).
        /// </summary>
        public void SpawnCoins(int count, Vector3[] spawnPositions)
        {
            if (spawnPositions == null) return;

            for (int i = 0; i < count && i < spawnPositions.Length; i++)
            {
                CoinPickup coin = GetFromPool();
                coin.transform.position = spawnPositions[i] + Vector3.up * 0.5f;
                coin.gameObject.SetActive(true);
                coin.Initialize(this);

                // Apply upward force with random horizontal spread
                Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    float forceY = Random.Range(spawnForceMin, spawnForceMax);
                    float forceX = Random.Range(-horizontalSpread, horizontalSpread);
                    rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
                }
            }
        }

        public void ReturnToPool(CoinPickup coin)
        {
            if (coin == null) return;
            coin.gameObject.SetActive(false);

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            pool.Enqueue(coin);
        }

        private CoinPickup GetFromPool()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            // Grow pool if exhausted
            return CreateCoinObject();
        }

        private CoinPickup CreateCoinObject()
        {
            GameObject coinGO = new GameObject("Coin");
            coinGO.transform.SetParent(transform);
            coinGO.layer = gameObject.layer;

            // Visual placeholder: small yellow circle
            SpriteRenderer sr = coinGO.AddComponent<SpriteRenderer>();
            sr.color = new Color(1f, 0.85f, 0.1f); // gold
            sr.sortingOrder = 10;

            // Physics
            CircleCollider2D col = coinGO.AddComponent<CircleCollider2D>();
            col.radius = 0.2f;

            Rigidbody2D rb = coinGO.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1.5f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.freezeRotation = true;

            // Scale down
            coinGO.transform.localScale = Vector3.one * 0.4f;

            CoinPickup pickup = coinGO.AddComponent<CoinPickup>();
            return pickup;
        }
    }
}
