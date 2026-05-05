using UnityEngine;

namespace MonsterVox.Data
{
    [CreateAssetMenu(fileName = "ThemeData", menuName = "MonsterVox/ThemeData")]
    public class ThemeDataSO : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string themeID;
        [SerializeField] private string themeName;

        [Header("Assets")]
        [SerializeField] private AudioClip bgmClip;
        [SerializeField] private Sprite backgroundSprite;
        [SerializeField] private GameObject themePrefab;

        [Header("Audio")]
        [SerializeField] private float bpm = 120f;

        [Header("Economy")]
        [SerializeField] private int unlockCostCoins;
        [SerializeField] private int unlockCostAds;

        public string ThemeID => themeID;
        public string ThemeName => themeName;
        public AudioClip BgmClip => bgmClip;
        public Sprite BackgroundSprite => backgroundSprite;
        public GameObject ThemePrefab => themePrefab;
        public float BPM => bpm;
        public int UnlockCostCoins => unlockCostCoins;
        public int UnlockCostAds => unlockCostAds;
    }
}
