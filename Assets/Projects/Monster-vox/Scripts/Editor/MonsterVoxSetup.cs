using UnityEngine;
using UnityEditor;
using MonsterVox.Data;
using MonsterVox.Audio;
using MonsterVox.UI;
using MonsterVox.Managers;
using MonsterVox.Gameplay;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MonsterVox.Editor
{
    public class MonsterVoxSetup
    {
        private static string rootPath = "Assets/Projects/Monster-vox";

        [MenuItem("MonsterVox/1. Setup Data Assets")]
        public static void SetupDataAssets()
        {
            EnsureFolders();

            // AudioConfig
            string configPath = $"{rootPath}/Data/AudioConfig.asset";
            AudioConfigSO config = AssetDatabase.LoadAssetAtPath<AudioConfigSO>(configPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<AudioConfigSO>();
                AssetDatabase.CreateAsset(config, configPath);
            }

            // Themes
            CreateThemeSO("Theme_01", "Spooky Room", 120f, 0, 0);
            CreateThemeSO("Theme_02", "Neon Graveyard", 120f, 500, 2);
            CreateThemeSO("Theme_03", "Alien Stage", 130f, 800, 3);

            // Monsters
            CreateMonsterSO("Mon_01", "Normal Cyclops", VoiceFilterType.Normal, 0, 0);
            CreateMonsterSO("Mon_02", "Chipmunk Ghost", VoiceFilterType.PitchUp, 150, 1);
            CreateMonsterSO("Mon_03", "Robo Bat", VoiceFilterType.Robot, 300, 1);
            CreateMonsterSO("Mon_04", "Deep Blob", VoiceFilterType.PitchDown, 400, 2);

            // Voice Clip Prefab
            CreateVoiceClipPrefab();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("✅ Data Assets created (AudioConfig + 3 Themes + 4 Monsters + VoiceClipUI)");
        }

        private static void CreateVoiceClipPrefab()
        {
            string prefabDir = $"{rootPath}/Prefabs";
            if (!AssetDatabase.IsValidFolder(prefabDir))
            {
                string parentDir = prefabDir.Substring(0, prefabDir.LastIndexOf('/'));
                string newFolder = prefabDir.Substring(prefabDir.LastIndexOf('/') + 1);
                AssetDatabase.CreateFolder(parentDir, newFolder);
            }

            string prefabPath = $"{prefabDir}/VoiceClipUI.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null) return;

            GameObject go = new GameObject("VoiceClipUI");
            RectTransform rt = go.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(80f, 80f);
            
            Image img = go.AddComponent<Image>();
            img.color = Color.cyan;
            // In a real project, assign a circle sprite here

            go.AddComponent<CanvasGroup>();
            go.AddComponent<AudioDragDropUI>();

            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);
        }

        [MenuItem("MonsterVox/2. Create Main Scene")]
        public static void CreateMainScene()
        {
            // Load dependencies
            AudioConfigSO config = AssetDatabase.LoadAssetAtPath<AudioConfigSO>($"{rootPath}/Data/AudioConfig.asset");
            if (config == null) { Debug.LogError("Run 'Setup Data Assets' first!"); return; }

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // ── Camera ──
            GameObject camGO = new GameObject("Main Camera");
            Camera cam = camGO.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 7f;
            cam.backgroundColor = new Color(0.08f, 0.05f, 0.15f);
            camGO.AddComponent<AudioListener>();
            camGO.tag = "MainCamera";
            camGO.transform.position = new Vector3(0, 0, -10f);

            // ── EventSystem ──
            GameObject evsGO = new GameObject("EventSystem");
            evsGO.AddComponent<EventSystem>();
            evsGO.AddComponent<StandaloneInputModule>();

            // ── Core_Managers ──
            GameObject managersRoot = new GameObject("Core_Managers");

            GameObject gmGO = new GameObject("GameManager");
            gmGO.transform.SetParent(managersRoot.transform);
            gmGO.AddComponent<GameManager>();

            GameObject emGO = new GameObject("EconomyManager");
            emGO.transform.SetParent(managersRoot.transform);
            emGO.AddComponent<EconomyManager>();

            GameObject catalogGO = new GameObject("GameDataCatalog");
            catalogGO.transform.SetParent(managersRoot.transform);
            var catalog = catalogGO.AddComponent<GameDataCatalog>();
            // Wire catalog data via SerializedObject
            WireCatalog(catalog);

            // ── Audio ──
            GameObject audioRoot = new GameObject("AudioSources");
            GameObject bgmGO = new GameObject("Source_BGM");
            bgmGO.transform.SetParent(audioRoot.transform);
            AudioSource bgmSrc = bgmGO.AddComponent<AudioSource>();
            bgmSrc.playOnAwake = false;
            bgmSrc.loop = true;

            // Assign BGM clip to Theme_01
            AudioClip bgmClip = AssetDatabase.LoadAssetAtPath<AudioClip>($"{rootPath}/SFX/Monster_In_The_Playroom.mp3");
            ThemeDataSO theme01 = AssetDatabase.LoadAssetAtPath<ThemeDataSO>($"{rootPath}/Data/Theme_01.asset");
            if (bgmClip != null && theme01 != null)
            {
                var so = new SerializedObject(theme01);
                so.FindProperty("bgmClip").objectReferenceValue = bgmClip;
                so.ApplyModifiedProperties();
            }

            GameObject poolGO = new GameObject("AudioSourcePool");
            poolGO.transform.SetParent(audioRoot.transform);
            var pool = poolGO.AddComponent<AudioSourcePool>();
            var poolSO = new SerializedObject(pool);
            poolSO.FindProperty("initialSize").intValue = 5;
            poolSO.ApplyModifiedProperties();

            // ── Environment ──
            GameObject envRoot = new GameObject("Environment_2D");
            GameObject bgRend = new GameObject("Background");
            bgRend.transform.SetParent(envRoot.transform);
            bgRend.AddComponent<SpriteRenderer>().sortingOrder = -10;

            // Floor collider for coins
            GameObject floor = new GameObject("DropArea_Floor");
            floor.transform.SetParent(envRoot.transform);
            floor.transform.position = new Vector3(0, -5f, 0);
            BoxCollider2D floorCol = floor.AddComponent<BoxCollider2D>();
            floorCol.size = new Vector2(20f, 1f);

            // CoinSpawner
            GameObject coinSpawnerGO = new GameObject("CoinSpawner");
            coinSpawnerGO.transform.SetParent(envRoot.transform);
            var coinSpawner = coinSpawnerGO.AddComponent<CoinSpawner>();

            // StageManager
            GameObject stageGO = new GameObject("StageManager");
            stageGO.transform.SetParent(envRoot.transform);
            var stage = stageGO.AddComponent<StageManager>();
            var stageSO = new SerializedObject(stage);
            stageSO.FindProperty("bgmSource").objectReferenceValue = bgmSrc;
            stageSO.FindProperty("backgroundRenderer").objectReferenceValue = bgRend.GetComponent<SpriteRenderer>();
            stageSO.FindProperty("coinSpawner").objectReferenceValue = coinSpawner;
            stageSO.FindProperty("audioConfig").objectReferenceValue = config;
            stageSO.ApplyModifiedProperties();

            // ── Entities ──
            GameObject entitiesRoot = new GameObject("Entities");
            GameObject recGO = new GameObject("MicrophoneRecorder");
            recGO.transform.SetParent(entitiesRoot.transform);
            var recorder = recGO.AddComponent<MicrophoneRecorder>();
            var recSO = new SerializedObject(recorder);
            recSO.FindProperty("audioConfig").objectReferenceValue = config;
            recSO.ApplyModifiedProperties();

            // ── UI Canvas ──
            GameObject canvasGO = CreateCanvas("UI_Canvas");
            
            // -- Panel_MainMenu --
            GameObject menuPanel = CreatePanel(canvasGO.transform, "Panel_MainMenu");
            var uiMgr = canvasGO.AddComponent<UIManager>();

            // Theme Name
            GameObject themeNameGO = CreateText(menuPanel.transform, "ThemeName", "Spooky Room", 28, TextAlignmentOptions.Center);
            RectTransform tnRT = themeNameGO.GetComponent<RectTransform>();
            tnRT.anchorMin = new Vector2(0.1f, 0.55f); tnRT.anchorMax = new Vector2(0.9f, 0.65f);
            tnRT.offsetMin = Vector2.zero; tnRT.offsetMax = Vector2.zero;

            // Theme Preview
            GameObject previewGO = new GameObject("ThemePreview");
            previewGO.transform.SetParent(menuPanel.transform, false);
            Image previewImg = previewGO.AddComponent<Image>();
            previewImg.color = new Color(0.3f, 0.1f, 0.4f);
            RectTransform pvRT = previewGO.GetComponent<RectTransform>();
            pvRT.anchorMin = new Vector2(0.15f, 0.35f); pvRT.anchorMax = new Vector2(0.85f, 0.55f);
            pvRT.offsetMin = Vector2.zero; pvRT.offsetMax = Vector2.zero;

            // Lock overlay
            GameObject lockGO = CreateText(menuPanel.transform, "LockOverlay", "🔒", 40, TextAlignmentOptions.Center);
            RectTransform lkRT = lockGO.GetComponent<RectTransform>();
            lkRT.anchorMin = new Vector2(0.15f, 0.35f); lkRT.anchorMax = new Vector2(0.85f, 0.55f);
            lkRT.offsetMin = Vector2.zero; lkRT.offsetMax = Vector2.zero;

            // Status text
            GameObject statusGO = CreateText(menuPanel.transform, "ThemeStatus", "UNLOCKED", 18, TextAlignmentOptions.Center);
            RectTransform stRT = statusGO.GetComponent<RectTransform>();
            stRT.anchorMin = new Vector2(0.2f, 0.30f); stRT.anchorMax = new Vector2(0.8f, 0.35f);
            stRT.offsetMin = Vector2.zero; stRT.offsetMax = Vector2.zero;

            // Nav buttons
            GameObject btnPrev = CreateButton(menuPanel.transform, "BtnPrev", "<", 0.02f, 0.42f, 0.08f, 0.48f);
            GameObject btnNext = CreateButton(menuPanel.transform, "BtnNext", ">", 0.92f, 0.42f, 0.98f, 0.48f);
            GameObject btnPlay = CreateButton(menuPanel.transform, "BtnPlay", "PLAY", 0.25f, 0.20f, 0.75f, 0.28f);
            btnPlay.GetComponent<Image>().color = new Color(0.1f, 0.8f, 0.3f);
            GameObject btnUnlock = CreateButton(menuPanel.transform, "BtnUnlock", "UNLOCK", 0.25f, 0.20f, 0.75f, 0.28f);
            btnUnlock.GetComponent<Image>().color = new Color(0.9f, 0.6f, 0.1f);

            // Coin HUD (Home)
            GameObject coinHudHome = CreateText(menuPanel.transform, "CoinDisplay", "0", 22, TextAlignmentOptions.Left);
            RectTransform chRT = coinHudHome.GetComponent<RectTransform>();
            chRT.anchorMin = new Vector2(0.05f, 0.90f); chRT.anchorMax = new Vector2(0.4f, 0.96f);
            chRT.offsetMin = Vector2.zero; chRT.offsetMax = Vector2.zero;
            var homeCoinHUD = coinHudHome.AddComponent<HUDCoinDisplay>();
            var homeCoinSO = new SerializedObject(homeCoinHUD);
            homeCoinSO.FindProperty("coinText").objectReferenceValue = coinHudHome.GetComponent<TextMeshProUGUI>();
            homeCoinSO.FindProperty("showSessionCoins").boolValue = false;
            homeCoinSO.ApplyModifiedProperties();

            // Bottom buttons
            GameObject btnStore = CreateButton(menuPanel.transform, "BtnStore", "STORE", 0.05f, 0.03f, 0.45f, 0.10f);
            GameObject btnSettings = CreateButton(menuPanel.transform, "BtnSettings", "SETTINGS", 0.55f, 0.03f, 0.95f, 0.10f);

            // ThemeCarouselUI
            var carousel = menuPanel.AddComponent<ThemeCarouselUI>();
            var carSO = new SerializedObject(carousel);
            carSO.FindProperty("themePreviewImage").objectReferenceValue = previewImg;
            carSO.FindProperty("themeNameText").objectReferenceValue = themeNameGO.GetComponent<TextMeshProUGUI>();
            carSO.FindProperty("themeStatusText").objectReferenceValue = statusGO.GetComponent<TextMeshProUGUI>();
            carSO.FindProperty("btnPrevious").objectReferenceValue = btnPrev.GetComponent<Button>();
            carSO.FindProperty("btnNext").objectReferenceValue = btnNext.GetComponent<Button>();
            carSO.FindProperty("btnPlay").objectReferenceValue = btnPlay.GetComponent<Button>();
            carSO.FindProperty("btnUnlock").objectReferenceValue = btnUnlock.GetComponent<Button>();
            carSO.FindProperty("lockOverlay").objectReferenceValue = lockGO;
            carSO.ApplyModifiedProperties();

            // -- Panel_Stage --
            GameObject stagePanel = CreatePanel(canvasGO.transform, "Panel_Stage");
            stagePanel.GetComponent<Image>().color = Color.clear; // G6: transparent so world space is visible
            stagePanel.SetActive(false);

            GameObject btnBack = CreateButton(stagePanel.transform, "BtnBack", "< BACK", 0.02f, 0.92f, 0.25f, 0.98f);
            
            GameObject sessionCoinGO = CreateText(stagePanel.transform, "SessionCoins", "0", 22, TextAlignmentOptions.Right);
            RectTransform scRT = sessionCoinGO.GetComponent<RectTransform>();
            scRT.anchorMin = new Vector2(0.6f, 0.92f); scRT.anchorMax = new Vector2(0.95f, 0.98f);
            scRT.offsetMin = Vector2.zero; scRT.offsetMax = Vector2.zero;
            var stageCoinHUD = sessionCoinGO.AddComponent<HUDCoinDisplay>();
            var stageCoinSO = new SerializedObject(stageCoinHUD);
            stageCoinSO.FindProperty("coinText").objectReferenceValue = sessionCoinGO.GetComponent<TextMeshProUGUI>();
            stageCoinSO.FindProperty("showSessionCoins").boolValue = true;
            stageCoinSO.ApplyModifiedProperties();

            // Record button was removed in favor of NewAnimalPopupUI

            // -- Popup_Store (minimal placeholder) --
            GameObject storePopup = CreatePanel(canvasGO.transform, "Popup_Store");
            storePopup.SetActive(false);
            var storeUI = storePopup.AddComponent<StorePopupUI>();

            // -- Popup_Settings (minimal placeholder) --
            GameObject settingsPopup = CreatePanel(canvasGO.transform, "Popup_Settings");
            settingsPopup.SetActive(false);
            settingsPopup.AddComponent<SettingsPopupUI>();

            // ── Wire UIManager ──
            var uiSO = new SerializedObject(uiMgr);
            uiSO.FindProperty("panelMainMenu").objectReferenceValue = menuPanel;
            uiSO.FindProperty("panelStage").objectReferenceValue = stagePanel;
            uiSO.FindProperty("popupStore").objectReferenceValue = storePopup;
            uiSO.FindProperty("popupSettings").objectReferenceValue = settingsPopup;
            uiSO.FindProperty("btnBack").objectReferenceValue = btnBack.GetComponent<Button>();
            uiSO.FindProperty("btnStore").objectReferenceValue = btnStore.GetComponent<Button>();
            uiSO.FindProperty("btnSettings").objectReferenceValue = btnSettings.GetComponent<Button>();
            uiSO.ApplyModifiedProperties();

            // Save
            string scenePath = $"{rootPath}/Scenes/MonsterVox_Main.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log($"✅ Main Scene created: {scenePath}");
        }

        // ── Helper Methods ──

        private static void EnsureFolders()
        {
            string[] folders = { "Data", "Prefabs", "Scenes" };
            foreach (string f in folders)
            {
                string path = $"{rootPath}/{f}";
                if (!AssetDatabase.IsValidFolder(path))
                    AssetDatabase.CreateFolder(rootPath, f);
            }
        }

        private static void CreateThemeSO(string id, string name, float bpm, int costCoins, int costAds)
        {
            string path = $"{rootPath}/Data/{id}.asset";
            if (AssetDatabase.LoadAssetAtPath<ThemeDataSO>(path) != null) return;
            
            ThemeDataSO so = ScriptableObject.CreateInstance<ThemeDataSO>();
            var obj = new SerializedObject(so);
            obj.FindProperty("themeID").stringValue = id;
            obj.FindProperty("themeName").stringValue = name;
            obj.FindProperty("bpm").floatValue = bpm;
            obj.FindProperty("unlockCostCoins").intValue = costCoins;
            obj.FindProperty("unlockCostAds").intValue = costAds;
            obj.ApplyModifiedProperties();
            AssetDatabase.CreateAsset(so, path);
        }

        private static void CreateMonsterSO(string id, string name, VoiceFilterType filter, int costCoins, int costAds)
        {
            string path = $"{rootPath}/Data/{id}.asset";
            if (AssetDatabase.LoadAssetAtPath<MonsterDataSO>(path) != null) return;

            MonsterDataSO so = ScriptableObject.CreateInstance<MonsterDataSO>();
            var obj = new SerializedObject(so);
            obj.FindProperty("monsterID").stringValue = id;
            obj.FindProperty("monsterName").stringValue = name;
            obj.FindProperty("voiceFilter").enumValueIndex = (int)filter;
            obj.FindProperty("unlockCostCoins").intValue = costCoins;
            obj.FindProperty("unlockCostAds").intValue = costAds;
            obj.ApplyModifiedProperties();
            AssetDatabase.CreateAsset(so, path);
        }

        private static void WireCatalog(GameDataCatalog catalog)
        {
            ThemeDataSO[] themes = {
                AssetDatabase.LoadAssetAtPath<ThemeDataSO>($"{rootPath}/Data/Theme_01.asset"),
                AssetDatabase.LoadAssetAtPath<ThemeDataSO>($"{rootPath}/Data/Theme_02.asset"),
                AssetDatabase.LoadAssetAtPath<ThemeDataSO>($"{rootPath}/Data/Theme_03.asset"),
            };
            MonsterDataSO[] monsters = {
                AssetDatabase.LoadAssetAtPath<MonsterDataSO>($"{rootPath}/Data/Mon_01.asset"),
                AssetDatabase.LoadAssetAtPath<MonsterDataSO>($"{rootPath}/Data/Mon_02.asset"),
                AssetDatabase.LoadAssetAtPath<MonsterDataSO>($"{rootPath}/Data/Mon_03.asset"),
                AssetDatabase.LoadAssetAtPath<MonsterDataSO>($"{rootPath}/Data/Mon_04.asset"),
            };

            var so = new SerializedObject(catalog);
            var themesProp = so.FindProperty("themes");
            themesProp.arraySize = themes.Length;
            for (int i = 0; i < themes.Length; i++)
                themesProp.GetArrayElementAtIndex(i).objectReferenceValue = themes[i];

            var monstersProp = so.FindProperty("monsters");
            monstersProp.arraySize = monsters.Length;
            for (int i = 0; i < monsters.Length; i++)
                monstersProp.GetArrayElementAtIndex(i).objectReferenceValue = monsters[i];

            so.ApplyModifiedProperties();
        }

        private static GameObject CreateCanvas(string name)
        {
            GameObject go = new GameObject(name);
            Canvas canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;
            go.AddComponent<GraphicRaycaster>();
            return go;
        }

        private static GameObject CreatePanel(Transform parent, string name)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            Image bg = go.AddComponent<Image>();
            bg.color = new Color(0.06f, 0.04f, 0.12f, 0.95f);
            return go;
        }

        private static GameObject CreateButton(Transform parent, string name, string label,
            float aMinX, float aMinY, float aMaxX, float aMaxY)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(aMinX, aMinY);
            rt.anchorMax = new Vector2(aMaxX, aMaxY);
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            Image img = go.AddComponent<Image>();
            img.color = new Color(0.25f, 0.15f, 0.4f);
            go.AddComponent<Button>();

            GameObject textGO = new GameObject("Label");
            textGO.transform.SetParent(go.transform, false);
            TextMeshProUGUI txt = textGO.AddComponent<TextMeshProUGUI>();
            txt.text = label;
            txt.alignment = TextAlignmentOptions.Center;
            txt.color = Color.white;
            txt.fontSize = 24;
            RectTransform trt = textGO.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;

            return go;
        }

        private static GameObject CreateText(Transform parent, string name, string content, int size, TextAlignmentOptions align)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            TextMeshProUGUI txt = go.AddComponent<TextMeshProUGUI>();
            txt.text = content;
            txt.fontSize = size;
            txt.alignment = align;
            txt.color = Color.white;
            return go;
        }
    }
}
