using UnityEngine;
using UnityEditor;
using MonsterVox.Data;
using MonsterVox.Audio;
using MonsterVox.UI;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
namespace MonsterVox.Editor
{
    public class MonsterVoxSetup
    {
        [MenuItem("MonsterVox/Setup Base Assets")]
        public static void SetupAssets()
        {
            string rootPath = "Assets/Projects/Monster-vox";
            
            // 1. Ensure Folders
            if (!AssetDatabase.IsValidFolder(rootPath + "/Data")) AssetDatabase.CreateFolder(rootPath, "Data");
            if (!AssetDatabase.IsValidFolder(rootPath + "/Prefabs")) AssetDatabase.CreateFolder(rootPath, "Prefabs");
            
            // 2. Create Config SO
            string configPath = rootPath + "/Data/AudioConfig.asset";
            AudioConfigSO config = AssetDatabase.LoadAssetAtPath<AudioConfigSO>(configPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<AudioConfigSO>();
                AssetDatabase.CreateAsset(config, configPath);
            }

            // 3. Create MicrobiologyRecorder Prefab
            CreatePrefab("MicrophoneRecorder", rootPath + "/Prefabs/MicrophoneRecorder.prefab", go =>
            {
                var rec = go.AddComponent<MicrophoneRecorder>();
                var serializedObj = new SerializedObject(rec);
                serializedObj.FindProperty("audioConfig").objectReferenceValue = config;
                serializedObj.ApplyModifiedProperties();
            });

            // 4. Create AudioSourcePool Prefab
            CreatePrefab("AudioSourcePool", rootPath + "/Prefabs/AudioSourcePool.prefab", go =>
            {
                var pool = go.AddComponent<AudioSourcePool>();
                var serializedObj = new SerializedObject(pool);
                serializedObj.FindProperty("initialSize").intValue = 5;
                serializedObj.ApplyModifiedProperties();
            });

            // 5. Create Monster Prefab (QuantizedAudioPlayer)
            CreatePrefab("Monster_Vocalist", rootPath + "/Prefabs/Monster_Vocalist.prefab", go =>
            {
                var player = go.AddComponent<QuantizedAudioPlayer>();
                var serializedObj = new SerializedObject(player);
                serializedObj.FindProperty("audioConfig").objectReferenceValue = config;
                serializedObj.ApplyModifiedProperties();

                // Add 2D Collider for Raycast detection
                go.AddComponent<BoxCollider2D>();
            });

            // 6. Create UI VoiceClip Prefab
            CreatePrefab("VoiceClipUI", rootPath + "/Prefabs/VoiceClipUI.prefab", go =>
            {
                var rt = go.AddComponent<RectTransform>();
                rt.sizeDelta = new Vector2(100, 100);
                
                // Visual
                var img = go.AddComponent<Image>();
                img.color = new Color(0.2f, 0.8f, 0.2f);
                
                // Logic
                go.AddComponent<CanvasGroup>();
                go.AddComponent<AudioDragDropUI>();
            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("✅ Monster-vox Assets & Prefabs Base created successfully!");
        }

        [MenuItem("MonsterVox/Create Demo Scene")]
        public static void CreateDemoScene()
        {
            string rootPath = "Assets/Projects/Monster-vox";
            
            // Validate dependencies
            GameObject recorderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{rootPath}/Prefabs/MicrophoneRecorder.prefab");
            GameObject poolPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{rootPath}/Prefabs/AudioSourcePool.prefab");
            GameObject monsterPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{rootPath}/Prefabs/Monster_Vocalist.prefab");
            GameObject voiceUI = AssetDatabase.LoadAssetAtPath<GameObject>($"{rootPath}/Prefabs/VoiceClipUI.prefab");
            
            if (recorderPrefab == null || poolPrefab == null || monsterPrefab == null || voiceUI == null)
            {
                Debug.LogError("Missing prefabs! Please run 'MonsterVox/Setup Base Assets' first.");
                return;
            }

            // Create blank scene
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Setup Camera
            GameObject camGO = new GameObject("Main Camera");
            Camera cam = camGO.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            camGO.AddComponent<AudioListener>();
            camGO.tag = "MainCamera";
            camGO.transform.position = new Vector3(0, 0, -10f);

            // Setup Event System
            GameObject eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<EventSystem>();
            eventSystemGO.AddComponent<StandaloneInputModule>();

            // Instantiate Systems
            GameObject poolInst = PrefabUtility.InstantiatePrefab(poolPrefab) as GameObject;
            GameObject recorderInst = PrefabUtility.InstantiatePrefab(recorderPrefab) as GameObject;

            // Setup Canvas
            GameObject canvasGO = new GameObject("Canvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Setup Record Button
            GameObject btnGO = new GameObject("RecordButton");
            btnGO.transform.SetParent(canvasGO.transform, false);
            RectTransform btnRT = btnGO.AddComponent<RectTransform>();
            btnRT.anchorMin = new Vector2(0.5f, 0);
            btnRT.anchorMax = new Vector2(0.5f, 0);
            btnRT.pivot = new Vector2(0.5f, 0);
            btnRT.anchoredPosition = new Vector2(0, 50);
            btnRT.sizeDelta = new Vector2(200, 80);
            
            Image btnImg = btnGO.AddComponent<Image>();
            btnImg.color = Color.red;
            Button btn = btnGO.AddComponent<Button>();

            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(btnGO.transform, false);
            Text txt = textGO.AddComponent<Text>();
            txt.text = "HOLD TO RECORD";
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            
            RectTransform textRT = textGO.GetComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.sizeDelta = Vector2.zero;

            // Setup DemoGameManager
            GameObject managerGO = new GameObject("DemoGameManager");
            DemoGameManager dm = managerGO.AddComponent<DemoGameManager>();
            dm.microphoneRecorder = recorderInst.GetComponent<MicrophoneRecorder>();
            dm.voiceClipUIPrefab = voiceUI;
            dm.canvasTransform = canvasGO.transform;

            // Hook EventTrigger for PointerDown/Up instead of standard Button clicked (since we need hold logic)
            EventTrigger trigger = btnGO.AddComponent<EventTrigger>();
            
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => { dm.OnRecordPointerDown(); });
            trigger.triggers.Add(pointerDown);

            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => { dm.OnRecordPointerUp(); });
            trigger.triggers.Add(pointerUp);

            // Instantiate 3 Monsters
            for(int i = -1; i <= 1; i++)
            {
                GameObject mInst = PrefabUtility.InstantiatePrefab(monsterPrefab) as GameObject;
                mInst.transform.position = new Vector3(i * 3f, 1f, 0f);
            }

            // Save Scene
            string scenePath = $"{rootPath}/DemoScene.unity";
            EditorSceneManager.SaveScene(newScene, scenePath);
            Debug.Log($"✅ Demo Scene created and saved to {scenePath}");
        }

        private static void CreatePrefab(string name, string path, System.Action<GameObject> setup)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                Debug.Log($"Prefab already exists: {name}");
                return;
            }

            GameObject go = new GameObject(name);
            setup?.Invoke(go);
            PrefabUtility.SaveAsPrefabAsset(go, path);
            GameObject.DestroyImmediate(go);
        }
    }
}
