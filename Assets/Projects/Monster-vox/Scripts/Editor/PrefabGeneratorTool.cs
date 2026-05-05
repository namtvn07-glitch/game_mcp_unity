#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using MonsterVox.Data;
using MonsterVox.Gameplay;
using MonsterVox.Audio;

namespace MonsterVox.EditorTools
{
    public static class PrefabGeneratorTool
    {
        [MenuItem("Tools/MonsterVox/Generate Prefab Variants")]
        public static void GeneratePrefabs()
        {
            string themePath = "Assets/Projects/Monster-vox/Prefabs/Themes";
            string monsterPath = "Assets/Projects/Monster-vox/Prefabs/Monsters";

            if (!AssetDatabase.IsValidFolder(themePath))
            {
                System.IO.Directory.CreateDirectory(themePath);
            }
            if (!AssetDatabase.IsValidFolder(monsterPath))
            {
                System.IO.Directory.CreateDirectory(monsterPath);
            }

            // 1. Create Base Theme
            string baseThemePath = $"{themePath}/BaseTheme.prefab";
            GameObject baseThemeGO = new GameObject("BaseTheme");
            
            // Add BG
            GameObject bgGO = new GameObject("Background");
            bgGO.transform.SetParent(baseThemeGO.transform);
            SpriteRenderer bgSr = bgGO.AddComponent<SpriteRenderer>();
            bgSr.sortingOrder = -10;

            // Add Slots
            GameObject slotsGO = new GameObject("Slots");
            slotsGO.transform.SetParent(baseThemeGO.transform);
            
            Transform[] baseSlots = new Transform[3];
            for(int i = 0; i < 3; i++)
            {
                GameObject slot = new GameObject($"Slot_{i}");
                slot.transform.SetParent(slotsGO.transform);
                slot.transform.localPosition = new Vector3(i * 2.5f, -1f, 0f);
                baseSlots[i] = slot.transform;
            }

            var themeController = baseThemeGO.AddComponent<ThemeController>();
            // Serialize Private Fields via SerializedObject
            var soTheme = new SerializedObject(themeController);
            soTheme.FindProperty("backgroundRenderer").objectReferenceValue = bgSr;
            var slotsProp = soTheme.FindProperty("slotTransforms");
            slotsProp.arraySize = 3;
            for(int i = 0; i < 3; i++)
            {
                slotsProp.GetArrayElementAtIndex(i).objectReferenceValue = baseSlots[i];
            }
            soTheme.ApplyModifiedProperties();

            GameObject baseThemePrefab = PrefabUtility.SaveAsPrefabAsset(baseThemeGO, baseThemePath);
            Object.DestroyImmediate(baseThemeGO);

            // 2. Create Base Monster
            string baseMonsterPath = $"{monsterPath}/BaseMonster.prefab";
            GameObject baseMonsterGO = new GameObject("BaseMonster");
            
            SpriteRenderer mSr = baseMonsterGO.AddComponent<SpriteRenderer>();
            mSr.sortingOrder = 5;
            baseMonsterGO.AddComponent<MonsterVox.Utils.SpriteColliderGenerator>();
            baseMonsterGO.AddComponent<QuantizedAudioPlayer>();
            baseMonsterGO.AddComponent<MonsterController>();

            GameObject baseMonsterPrefab = PrefabUtility.SaveAsPrefabAsset(baseMonsterGO, baseMonsterPath);
            Object.DestroyImmediate(baseMonsterGO);

            // 3. Process ThemeDataSO
            string[] themeGuids = AssetDatabase.FindAssets("t:ThemeDataSO");
            foreach(var guid in themeGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ThemeDataSO data = AssetDatabase.LoadAssetAtPath<ThemeDataSO>(path);
                
                string variantPath = $"{themePath}/ThemeVariant_{data.name}.prefab";
                GameObject variantInstance = (GameObject)PrefabUtility.InstantiatePrefab(baseThemePrefab);
                
                // Modify Variant
                var controller = variantInstance.GetComponent<ThemeController>();
                if (controller != null && controller.BackgroundRenderer != null && data.BackgroundSprite != null)
                {
                    controller.BackgroundRenderer.sprite = data.BackgroundSprite;
                }

                GameObject savedVariant = PrefabUtility.SaveAsPrefabAsset(variantInstance, variantPath);
                Object.DestroyImmediate(variantInstance);

                // Link to SO
                var soData = new SerializedObject(data);
                soData.FindProperty("themePrefab").objectReferenceValue = savedVariant;
                soData.ApplyModifiedProperties();
            }

            // 4. Process MonsterDataSO
            string[] monsterGuids = AssetDatabase.FindAssets("t:MonsterDataSO");
            foreach(var guid in monsterGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MonsterDataSO data = AssetDatabase.LoadAssetAtPath<MonsterDataSO>(path);

                string variantPath = $"{monsterPath}/MonsterVariant_{data.name}.prefab";
                GameObject variantInstance = (GameObject)PrefabUtility.InstantiatePrefab(baseMonsterPrefab);

                var sr = variantInstance.GetComponent<SpriteRenderer>();
                if (sr != null && data.MonsterSprite != null)
                {
                    sr.sprite = data.MonsterSprite;
                }

                GameObject savedVariant = PrefabUtility.SaveAsPrefabAsset(variantInstance, variantPath);
                Object.DestroyImmediate(variantInstance);

                // Link to SO
                var soData = new SerializedObject(data);
                soData.FindProperty("monsterPrefab").objectReferenceValue = savedVariant;
                soData.ApplyModifiedProperties();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[PrefabGeneratorTool] Prefab Variants created and linked successfully!");
        }
    }
}
#endif
