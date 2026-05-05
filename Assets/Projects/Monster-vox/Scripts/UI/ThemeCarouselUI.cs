using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MonsterVox.Data;
using MonsterVox.Managers;

namespace MonsterVox.UI
{
    /// <summary>
    /// Horizontal theme carousel on the Home screen.
    /// Shows one theme at a time with left/right navigation buttons.
    /// </summary>
    public class ThemeCarouselUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image themePreviewImage;
        [SerializeField] private TextMeshProUGUI themeNameText;
        [SerializeField] private TextMeshProUGUI themeStatusText;
        [SerializeField] private Button btnPrevious;
        [SerializeField] private Button btnNext;
        [SerializeField] private Button btnPlay;
        [SerializeField] private Button btnUnlock;
        [SerializeField] private GameObject lockOverlay;

        private int currentIndex;

        private void Awake()
        {
            // Register listeners in Awake so they exist before OnEnable fires RefreshCurrentTheme
            if (btnPrevious != null) btnPrevious.onClick.AddListener(ShowPrevious);
            if (btnNext != null) btnNext.onClick.AddListener(ShowNext);
            if (btnPlay != null) btnPlay.onClick.AddListener(PlaySelectedTheme);
            if (btnUnlock != null) btnUnlock.onClick.AddListener(UnlockSelectedTheme);
        }

        private void Start()
        {
            // Subscribe to inventory changes after Managers have initialized in Awake
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnInventoryChanged += RefreshCurrentTheme;
            }
            RefreshCurrentTheme();
        }

        private void OnEnable()
        {
            // Re-subscribe if re-enabled after Start() has already run
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnInventoryChanged -= RefreshCurrentTheme;
                EconomyManager.Instance.OnInventoryChanged += RefreshCurrentTheme;
            }
            RefreshCurrentTheme();
        }

        private void OnDisable()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnInventoryChanged -= RefreshCurrentTheme;
            }
        }

        private void ShowPrevious()
        {
            var catalog = GameDataCatalog.Instance;
            if (catalog == null || catalog.AllThemes.Length == 0) return;

            currentIndex--;
            if (currentIndex < 0) currentIndex = catalog.AllThemes.Length - 1;
            RefreshCurrentTheme();
        }

        private void ShowNext()
        {
            var catalog = GameDataCatalog.Instance;
            if (catalog == null || catalog.AllThemes.Length == 0) return;

            currentIndex++;
            if (currentIndex >= catalog.AllThemes.Length) currentIndex = 0;
            RefreshCurrentTheme();
        }

        private void RefreshCurrentTheme()
        {
            var catalog = GameDataCatalog.Instance;
            if (catalog == null || catalog.AllThemes.Length == 0) return;

            if (currentIndex >= catalog.AllThemes.Length) currentIndex = 0;
            ThemeDataSO theme = catalog.AllThemes[currentIndex];
            if (theme == null) return;

            // Update visuals
            if (themeNameText != null) themeNameText.text = theme.ThemeName;

            if (themePreviewImage != null && theme.BackgroundSprite != null)
            {
                themePreviewImage.sprite = theme.BackgroundSprite;
                themePreviewImage.color = Color.white;
            }
            else if (themePreviewImage != null)
            {
                themePreviewImage.color = new Color(0.3f, 0.1f, 0.4f); // placeholder purple
            }

            // Check unlock status
            bool isUnlocked = false;
            if (EconomyManager.Instance != null)
            {
                isUnlocked = EconomyManager.Instance.Data.IsThemeUnlocked(theme.ThemeID);
            }

            if (lockOverlay != null) lockOverlay.SetActive(!isUnlocked);
            if (btnPlay != null) btnPlay.gameObject.SetActive(isUnlocked);
            if (btnUnlock != null) btnUnlock.gameObject.SetActive(!isUnlocked);

            if (themeStatusText != null)
            {
                themeStatusText.text = isUnlocked
                    ? "UNLOCKED"
                    : $"{theme.UnlockCostCoins} Coins";
            }
        }

        private void PlaySelectedTheme()
        {
            var catalog = GameDataCatalog.Instance;
            if (catalog == null || catalog.AllThemes.Length == 0) return;

            ThemeDataSO theme = catalog.AllThemes[currentIndex];
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GoToStage(theme);
            }
        }

        private void UnlockSelectedTheme()
        {
            var catalog = GameDataCatalog.Instance;
            if (catalog == null || catalog.AllThemes.Length == 0) return;

            ThemeDataSO theme = catalog.AllThemes[currentIndex];
            if (EconomyManager.Instance != null)
            {
                if (EconomyManager.Instance.TryUnlockTheme(theme.ThemeID))
                {
                    RefreshCurrentTheme();
                }
            }
        }

        private void OnDestroy()
        {
            if (btnPrevious != null) btnPrevious.onClick.RemoveListener(ShowPrevious);
            if (btnNext != null) btnNext.onClick.RemoveListener(ShowNext);
            if (btnPlay != null) btnPlay.onClick.RemoveListener(PlaySelectedTheme);
            if (btnUnlock != null) btnUnlock.onClick.RemoveListener(UnlockSelectedTheme);
        }
    }
}
