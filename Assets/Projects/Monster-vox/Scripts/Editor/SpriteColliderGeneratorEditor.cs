using UnityEngine;
using UnityEditor;

namespace MonsterVox.Utils.Editor
{
    [CustomEditor(typeof(SpriteColliderGenerator))]
    public class SpriteColliderGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SpriteColliderGenerator generator = (SpriteColliderGenerator)target;

            GUILayout.Space(10);
            if (GUILayout.Button("Generate Polygon Collider 2D", GUILayout.Height(30)))
            {
                generator.GenerateCollider();
                EditorUtility.SetDirty(generator.gameObject);
                
                // If this is a prefab, make sure changes are recorded
                PrefabUtility.RecordPrefabInstancePropertyModifications(generator.gameObject);
            }
        }
    }
}