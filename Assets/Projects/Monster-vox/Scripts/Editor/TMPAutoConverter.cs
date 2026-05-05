using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using System.Collections.Generic;

namespace MonsterVox.Editor
{
    public class TMPAutoConverter : EditorWindow
    {
        [MenuItem("MonsterVox/Tools/Auto Convert Text to TMP")]
        public static void ShowWindow()
        {
            GetWindow<TMPAutoConverter>("TMP Converter");
        }

        private void OnGUI()
        {
            GUILayout.Label("Text to TMP Auto Converter", EditorStyles.boldLabel);

            if (GUILayout.Button("1. Convert Text in Active Scene"))
            {
                ConvertScene();
            }

            if (GUILayout.Button("2. Convert Text in All Prefabs"))
            {
                ConvertPrefabs();
            }

            if (GUILayout.Button("3. Auto-fix Null TMP References (Scene)"))
            {
                FixReferencesInScene();
            }

            if (GUILayout.Button("4. Auto-fix Null TMP References (Prefabs)"))
            {
                FixReferencesInPrefabs();
            }
        }

        private void ConvertScene()
        {
            Text[] texts = FindObjectsOfType<Text>(true);
            int count = ConvertTextArray(texts);
            Debug.Log($"[TMPConverter] Converted {count} Text components in Scene.");
        }

        private void ConvertPrefabs()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            int count = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                Text[] texts = prefab.GetComponentsInChildren<Text>(true);
                if (texts.Length > 0)
                {
                    GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    Text[] instanceTexts = instance.GetComponentsInChildren<Text>(true);
                    ConvertTextArray(instanceTexts);
                    PrefabUtility.SaveAsPrefabAsset(instance, path);
                    DestroyImmediate(instance);
                    count += texts.Length;
                }
            }
            Debug.Log($"[TMPConverter] Converted {count} Text components across {guids.Length} Prefabs.");
        }

        private int ConvertTextArray(Text[] texts)
        {
            int count = 0;
            foreach (Text t in texts)
            {
                GameObject go = t.gameObject;
                string textContent = t.text;
                Color color = t.color;
                int fontSize = t.fontSize;
                TextAnchor alignment = t.alignment;
                bool enabled = t.enabled;
                FontStyle style = t.fontStyle;

                Undo.DestroyObjectImmediate(t);

                TextMeshProUGUI tmp = Undo.AddComponent<TextMeshProUGUI>(go);
                tmp.text = textContent;
                tmp.color = color;
                tmp.fontSize = fontSize;
                tmp.enabled = enabled;

                if (style == FontStyle.Bold) tmp.fontStyle = FontStyles.Bold;
                else if (style == FontStyle.Italic) tmp.fontStyle = FontStyles.Italic;
                else if (style == FontStyle.BoldAndItalic) tmp.fontStyle = FontStyles.Bold | FontStyles.Italic;

                switch (alignment)
                {
                    case TextAnchor.UpperLeft: tmp.alignment = TextAlignmentOptions.TopLeft; break;
                    case TextAnchor.UpperCenter: tmp.alignment = TextAlignmentOptions.Top; break;
                    case TextAnchor.UpperRight: tmp.alignment = TextAlignmentOptions.TopRight; break;
                    case TextAnchor.MiddleLeft: tmp.alignment = TextAlignmentOptions.Left; break;
                    case TextAnchor.MiddleCenter: tmp.alignment = TextAlignmentOptions.Center; break;
                    case TextAnchor.MiddleRight: tmp.alignment = TextAlignmentOptions.Right; break;
                    case TextAnchor.LowerLeft: tmp.alignment = TextAlignmentOptions.BottomLeft; break;
                    case TextAnchor.LowerCenter: tmp.alignment = TextAlignmentOptions.Bottom; break;
                    case TextAnchor.LowerRight: tmp.alignment = TextAlignmentOptions.BottomRight; break;
                }
                count++;
            }
            return count;
        }

        private void FixReferencesInScene()
        {
            MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>(true);
            int count = FixScriptReferences(scripts);
            Debug.Log($"[TMPConverter] Auto-fixed {count} TMP references in Scene.");
        }

        private void FixReferencesInPrefabs()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            int totalFixed = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                MonoBehaviour[] scripts = prefab.GetComponentsInChildren<MonoBehaviour>(true);
                
                bool needsSave = false;
                GameObject instance = null;
                
                foreach (var s in scripts)
                {
                    var type = s.GetType();
                    var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var f in fields)
                    {
                        if (f.FieldType == typeof(TextMeshProUGUI))
                        {
                            if (instance == null) 
                            {
                                instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                                scripts = instance.GetComponentsInChildren<MonoBehaviour>(true);
                                break; // Restart with instance scripts
                            }
                        }
                    }
                    if (instance != null) break;
                }

                if (instance != null)
                {
                    int fixedCount = FixScriptReferences(instance.GetComponentsInChildren<MonoBehaviour>(true));
                    if (fixedCount > 0)
                    {
                        PrefabUtility.SaveAsPrefabAsset(instance, path);
                        totalFixed += fixedCount;
                    }
                    DestroyImmediate(instance);
                }
            }
            Debug.Log($"[TMPConverter] Auto-fixed {totalFixed} TMP references across Prefabs.");
        }

        private int FixScriptReferences(MonoBehaviour[] scripts)
        {
            int count = 0;
            foreach (var s in scripts)
            {
                if (s == null) continue;
                var type = s.GetType();
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                bool modified = false;
                foreach (var f in fields)
                {
                    if (f.FieldType == typeof(TextMeshProUGUI))
                    {
                        var val = f.GetValue(s);
                        if (val == null || val.ToString() == "null")
                        {
                            TextMeshProUGUI found = null;
                            string lowerName = f.Name.ToLower();

                            // Heuristics
                            if (lowerName.Contains("name")) found = FindChildBySub(s.transform, "name");
                            if (found == null && lowerName.Contains("coin")) found = FindChildBySub(s.transform, "coin");
                            if (found == null && lowerName.Contains("cost")) found = FindChildBySub(s.transform, "cost");
                            if (found == null && lowerName.Contains("level")) found = FindChildBySub(s.transform, "level");

                            if (found == null) found = s.GetComponentInChildren<TextMeshProUGUI>(true);

                            if (found != null)
                            {
                                f.SetValue(s, found);
                                modified = true;
                                count++;
                            }
                        }
                    }
                }
                if (modified)
                {
                    EditorUtility.SetDirty(s);
                }
            }
            return count;
        }

        private TextMeshProUGUI FindChildBySub(Transform parent, string sub)
        {
            foreach (var t in parent.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                if (t.name.ToLower().Contains(sub)) return t;
            }
            return null;
        }
    }
}
