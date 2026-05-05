#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MonsterVox.UI;
using MonsterVox.Audio;

namespace MonsterVox.EditorScripts
{
    public class PopupGenerator : EditorWindow
    {
        [MenuItem("MonsterVox/Generate New Animal Popup UI")]
        public static void GenerateNewAnimalPopup()
        {
            // 1. Create root GameObject
            GameObject root = new GameObject("Popup_NewAnimal");
            RectTransform rootRt = root.AddComponent<RectTransform>();
            rootRt.sizeDelta = new Vector2(800, 600);
            
            Image bg = root.AddComponent<Image>();
            bg.color = new Color(0.95f, 0.93f, 0.88f); // Paper color

            // 2. Create Portrait Image
            GameObject portraitObj = new GameObject("Img_Portrait");
            portraitObj.transform.SetParent(root.transform, false);
            RectTransform portraitRt = portraitObj.AddComponent<RectTransform>();
            portraitRt.sizeDelta = new Vector2(250, 250);
            portraitRt.anchoredPosition = new Vector2(-200, 50);
            Image imgPortrait = portraitObj.AddComponent<Image>();
            imgPortrait.color = Color.white;

            // 3. Create Element Image
            GameObject elementObj = new GameObject("Img_Element");
            elementObj.transform.SetParent(portraitObj.transform, false);
            RectTransform elementRt = elementObj.AddComponent<RectTransform>();
            elementRt.sizeDelta = new Vector2(64, 64);
            elementRt.anchorMin = new Vector2(1, 1);
            elementRt.anchorMax = new Vector2(1, 1);
            elementRt.anchoredPosition = new Vector2(-10, -10);
            Image imgElement = elementObj.AddComponent<Image>();
            imgElement.color = Color.cyan;

            // 4. Create Name InputField
            GameObject inputObj = DefaultControls.CreateInputField(new DefaultControls.Resources());
            inputObj.name = "Input_Name";
            inputObj.transform.SetParent(root.transform, false);
            RectTransform inputRt = inputObj.GetComponent<RectTransform>();
            inputRt.sizeDelta = new Vector2(300, 50);
            inputRt.anchoredPosition = new Vector2(150, 100);
            
            // Convert to TMP_InputField since DefaultControls gives standard InputField
            DestroyImmediate(inputObj.GetComponent<InputField>());
            DestroyImmediate(inputObj.GetComponent<Text>());
            TMP_InputField tmpInput = inputObj.AddComponent<TMP_InputField>();
            
            GameObject textComponentObj = new GameObject("Text Area");
            textComponentObj.transform.SetParent(inputObj.transform, false);
            RectTransform textRt = textComponentObj.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero; textRt.anchorMax = Vector2.one;
            textRt.sizeDelta = Vector2.zero;
            
            GameObject tmpTextObj = new GameObject("Text");
            tmpTextObj.transform.SetParent(textComponentObj.transform, false);
            RectTransform tmpTextRt = tmpTextObj.AddComponent<RectTransform>();
            tmpTextRt.anchorMin = Vector2.zero; tmpTextRt.anchorMax = Vector2.one;
            tmpTextRt.sizeDelta = Vector2.zero;
            TextMeshProUGUI tmpText = tmpTextObj.AddComponent<TextMeshProUGUI>();
            tmpText.color = Color.black;
            tmpText.fontSize = 24;
            tmpInput.textComponent = tmpText;
            tmpInput.textViewport = textRt;

            // 5. Create Timer/Hint Text
            GameObject hintObj = new GameObject("Txt_Hint");
            hintObj.transform.SetParent(root.transform, false);
            RectTransform hintRt = hintObj.AddComponent<RectTransform>();
            hintRt.sizeDelta = new Vector2(300, 40);
            hintRt.anchoredPosition = new Vector2(150, 0);
            TextMeshProUGUI txtHint = hintObj.AddComponent<TextMeshProUGUI>();
            txtHint.text = "05:00";
            txtHint.color = Color.black;
            txtHint.alignment = TextAlignmentOptions.Center;

            // 6. Create Buttons
            Button btnRecord = CreateButton(root.transform, "Btn_Record", new Vector2(-50, -80), "REC");
            Button btnPlay = CreateButton(root.transform, "Btn_Play", new Vector2(50, -80), "PLAY");
            Button btnTryAgain = CreateButton(root.transform, "Btn_TryAgain", new Vector2(50, -180), "TRY AGAIN", new Vector2(140, 50));
            Button btnConfirm = CreateButton(root.transform, "Btn_Confirm", new Vector2(210, -180), "CONFIRM", new Vector2(140, 50));

            // User requested MicrophoneRecorder to be ON the record button
            MicrophoneRecorder micRecorder = btnRecord.gameObject.AddComponent<MicrophoneRecorder>();

            // 7. Add NewAnimalPopupUI and assign references
            NewAnimalPopupUI popupUI = root.AddComponent<NewAnimalPopupUI>();
            var so = new SerializedObject(popupUI);
            so.FindProperty("imgPortrait").objectReferenceValue = imgPortrait;
            so.FindProperty("imgElement").objectReferenceValue = imgElement;
            so.FindProperty("inputName").objectReferenceValue = tmpInput;
            so.FindProperty("btnRecord").objectReferenceValue = btnRecord;
            so.FindProperty("btnPlay").objectReferenceValue = btnPlay;
            so.FindProperty("btnTryAgain").objectReferenceValue = btnTryAgain;
            so.FindProperty("btnConfirm").objectReferenceValue = btnConfirm;
            so.FindProperty("txtTimer").objectReferenceValue = txtHint;
            so.FindProperty("recorder").objectReferenceValue = micRecorder;
            
            // Try to find a dummy test clip to assign
            string[] guids = AssetDatabase.FindAssets("test t:AudioClip");
            if (guids.Length > 0)
            {
                AudioClip dummyClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guids[0]));
                if (dummyClip != null)
                {
                    so.FindProperty("dummyTestClip").objectReferenceValue = dummyClip;
                    Debug.Log($"[PopupGenerator] Assigned {dummyClip.name} as dummy test clip.");
                }
            }
            
            so.ApplyModifiedProperties();

            // 8. Find Canvas and UIManager in Scene
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                root.transform.SetParent(canvas.transform, false);
                rootRt.anchoredPosition = Vector2.zero;
            }

            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                var uiSo = new SerializedObject(uiManager);
                uiSo.FindProperty("popupNewAnimal").objectReferenceValue = popupUI;
                uiSo.ApplyModifiedProperties();
                Debug.Log("[PopupGenerator] Successfully assigned to UIManager.");
            }

            // 9. Save as Prefab
            string dir = "Assets/Projects/Monster-vox/Prefabs/UI";
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            string path = $"{dir}/Popup_NewAnimal.prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(root, path, InteractionMode.UserAction);
            
            root.SetActive(false); // Popups start inactive
            
            Debug.Log($"[PopupGenerator] Generated Popup at {path}");
        }

        private static Button CreateButton(Transform parent, string name, Vector2 pos, string text, Vector2? size = null)
        {
            GameObject btnObj = DefaultControls.CreateButton(new DefaultControls.Resources());
            btnObj.name = name;
            btnObj.transform.SetParent(parent, false);
            RectTransform rt = btnObj.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;
            if (size.HasValue) rt.sizeDelta = size.Value;
            
            Text oldText = btnObj.GetComponentInChildren<Text>();
            if (oldText != null)
            {
                GameObject textObj = oldText.gameObject;
                DestroyImmediate(oldText);
                TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
                tmp.text = text;
                tmp.color = Color.black;
                tmp.alignment = TextAlignmentOptions.Center;
            }
            
            return btnObj.GetComponent<Button>();
        }
    }
}
#endif
