using UnityEngine;
using MonsterVox.Data;
using MonsterVox.Managers;

namespace MonsterVox.UI
{
    /// <summary>
    /// Central UI orchestrator. Toggles panels based on GameManager state changes.
    /// All button bindings are set up programmatically in Start().
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject panelMainMenu;
        [SerializeField] private GameObject panelStage;

        [Header("Popups")]
        [SerializeField] private GameObject popupStore;
        [SerializeField] private GameObject popupSettings;
        [SerializeField] private NewAnimalPopupUI popupNewAnimal;

        [Header("Buttons")]
        [SerializeField] private UnityEngine.UI.Button btnBack;
        [SerializeField] private UnityEngine.UI.Button btnStore;
        [SerializeField] private UnityEngine.UI.Button btnSettings;

        private bool _started;

        private void Start()
        {
            // Ensure popups start hidden
            if (popupStore != null) popupStore.SetActive(false);
            if (popupSettings != null) popupSettings.SetActive(false);
            if (popupNewAnimal != null) popupNewAnimal.gameObject.SetActive(false);

            if (btnBack != null) btnBack.onClick.AddListener(GoBackToHome);
            if (btnStore != null) btnStore.onClick.AddListener(OpenStore);
            if (btnSettings != null) btnSettings.onClick.AddListener(OpenSettings);

            // Subscribe after all Managers have initialized in their Awake()
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStateChanged += HandleStateChanged;
                // Sync to current state immediately
                HandleStateChanged(GameManager.Instance.CurrentState);
            }
            else
            {
                Debug.LogError("[UIManager] GameManager.Instance is null in Start! Check scene setup.");
            }

            _started = true;
            
            MonsterVox.Gameplay.StageManager.OnMonsterAssigned += HandleMonsterAssigned;
        }

        private void OnEnable()
        {
            // Re-subscribe only after Start() has run (handles panel being toggled on/off)
            if (_started && GameManager.Instance != null)
            {
                GameManager.Instance.OnStateChanged -= HandleStateChanged;
                GameManager.Instance.OnStateChanged += HandleStateChanged;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStateChanged -= HandleStateChanged;
            }
            MonsterVox.Gameplay.StageManager.OnMonsterAssigned -= HandleMonsterAssigned;
        }

        private void HandleStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Home:
                    ShowPanel(panelMainMenu);
                    HidePanel(panelStage);
                    CloseAllPopups();
                    break;
                case GameState.Stage:
                    HidePanel(panelMainMenu);
                    ShowPanel(panelStage);
                    CloseAllPopups();
                    break;
            }
        }

        // Called by UI Buttons
        public void OpenStore()
        {
            if (popupStore != null) popupStore.SetActive(true);
        }

        public void CloseStore()
        {
            if (popupStore != null) popupStore.SetActive(false);
        }

        public void OpenSettings()
        {
            if (popupSettings != null) popupSettings.SetActive(true);
        }

        public void CloseSettings()
        {
            if (popupSettings != null) popupSettings.SetActive(false);
        }

        public void GoBackToHome()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GoToHome();
            }
        }

        private void CloseAllPopups()
        {
            if (popupStore != null) popupStore.SetActive(false);
            if (popupSettings != null) popupSettings.SetActive(false);
            if (popupNewAnimal != null) popupNewAnimal.gameObject.SetActive(false);
        }

        private void HandleMonsterAssigned(MonsterVox.Gameplay.MonsterController controller, MonsterDataSO data)
        {
            if (popupNewAnimal != null)
            {
                popupNewAnimal.Open(controller, data);
            }
        }

        private void ShowPanel(GameObject panel)
        {
            if (panel != null) panel.SetActive(true);
        }

        private void HidePanel(GameObject panel)
        {
            if (panel != null) panel.SetActive(false);
        }
    }
}
