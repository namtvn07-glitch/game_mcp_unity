using UnityEngine;

namespace MonsterVox.Data
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "MonsterVox/MonsterData")]
    public class MonsterDataSO : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string monsterID;
        [SerializeField] private string monsterName;

        [Header("Assets")]
        [SerializeField] private Sprite monsterSprite;
        [SerializeField] private GameObject monsterPrefab;

        [Header("Audio")]
        [SerializeField] private VoiceFilterType voiceFilter = VoiceFilterType.Normal;

        [Header("Economy")]
        [SerializeField] private int unlockCostCoins;
        [SerializeField] private int unlockCostAds;

        public string MonsterID => monsterID;
        public string MonsterName => monsterName;
        public Sprite MonsterSprite => monsterSprite;
        public GameObject MonsterPrefab => monsterPrefab;
        public VoiceFilterType VoiceFilter => voiceFilter;
        public int UnlockCostCoins => unlockCostCoins;
        public int UnlockCostAds => unlockCostAds;
    }
}
