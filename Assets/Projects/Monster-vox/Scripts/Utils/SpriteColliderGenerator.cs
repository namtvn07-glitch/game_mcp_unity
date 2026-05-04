using UnityEngine;

namespace MonsterVox.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteColliderGenerator : MonoBehaviour
    {
        public void GenerateCollider()
        {
            // Remove old colliders to avoid duplicates
            Collider2D[] existingColliders = GetComponents<Collider2D>();
            foreach (Collider2D col in existingColliders)
            {
                DestroyImmediate(col);
            }

            // Unity automatically fits the PolygonCollider2D to the SpriteRenderer's sprite upon creation
            PolygonCollider2D polyCol = gameObject.AddComponent<PolygonCollider2D>();
            if (polyCol != null)
            {
                Debug.Log($"[MonsterVox] Successfully generated PolygonCollider2D for {gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"[MonsterVox] Failed to generate PolygonCollider2D for {gameObject.name}");
            }
        }
    }
}