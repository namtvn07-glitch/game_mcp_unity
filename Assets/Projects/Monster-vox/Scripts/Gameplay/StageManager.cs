using UnityEngine;
using MonsterVox.Data;
using MonsterVox.Managers;

namespace MonsterVox.Gameplay
{
    /// <summary>
    /// Manages the stage environment: monster slots, BGM playback, background,
    /// and triggers coin drops when BGM loops complete.
    /// </summary>
    public class StageManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private CoinSpawner coinSpawner;
        [SerializeField] private MonsterVox.Data.AudioConfigSO audioConfig;

        private ThemeController currentThemeInstance;
        private SlotController[] stageSlots;
        private MonsterController[] activeMonsters;
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MonsterController>> monsterPools = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MonsterController>>();
        private ThemeDataSO currentTheme;
        private double bgmLoopStartDspTime;
        private double bgmLoopDuration;
        private bool isActive;
        private bool _started;
        private Camera mainCamera;

        public static event System.Action<MonsterController, MonsterDataSO> OnMonsterAssigned;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStateChanged += HandleStateChanged;
                // Sync immediately in case we started mid-state
                HandleStateChanged(GameManager.Instance.CurrentState);
            }
            else
            {
                Debug.LogError("[StageManager] GameManager.Instance is null in Start!");
            }
            _started = true;
        }

        private void OnEnable()
        {
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
        }

        private void HandleStateChanged(GameState state)
        {
            if (state == GameState.Stage)
            {
                ActivateStage(GameManager.Instance.ActiveTheme);
            }
            else
            {
                DeactivateStage();
            }
        }

        private void ActivateStage(ThemeDataSO theme)
        {
            if (theme == null) return;
            currentTheme = theme;
            isActive = true;

            // Spawn Theme Prefab
            if (theme.ThemePrefab != null)
            {
                var themeGO = Instantiate(theme.ThemePrefab, transform);
                currentThemeInstance = themeGO.GetComponent<ThemeController>();
                if (currentThemeInstance != null)
                {
                    currentThemeInstance.FitBackgroundToScreen(mainCamera);
                }
            }
            else
            {
                Debug.LogWarning($"[StageManager] Theme {theme.ThemeID} has no ThemePrefab assigned.");
            }

            // Setup BGM
            if (bgmSource != null && theme.BgmClip != null)
            {
                bgmSource.clip = theme.BgmClip;
                bgmSource.loop = true;
                bgmSource.Play();
                bgmLoopStartDspTime = AudioSettings.dspTime;
                bgmLoopDuration = theme.BgmClip.length;
            }

            // Spawn monster slots based on player's slot level
            SpawnSlots();

            // Reset session coins
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.ResetSessionCoins();
            }
        }

        private void DeactivateStage()
        {
            isActive = false;

            // Stop BGM
            if (bgmSource != null)
            {
                bgmSource.Stop();
            }

            // Save progress when leaving stage
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.SaveProgress();
            }

            // Cleanup monsters and theme
            ClearMonsters();
            stageSlots = null;
            if (currentThemeInstance != null)
            {
                Destroy(currentThemeInstance.gameObject);
                currentThemeInstance = null;
            }
        }

        private void SpawnSlots()
        {
            ClearMonsters();

            int maxSlots = 3; // default
            if (EconomyManager.Instance != null)
            {
                maxSlots = EconomyManager.Instance.Data.GetMaxSlots();
            }

            activeMonsters = new MonsterController[maxSlots];
            stageSlots = new SlotController[maxSlots];

            for (int i = 0; i < maxSlots; i++)
            {
                Transform slotTransform = null;
                Vector3 pos = new Vector3(i * 2.5f, -1f, 0f); // Default fallback

                if (currentThemeInstance != null && currentThemeInstance.SlotTransforms != null && i < currentThemeInstance.SlotTransforms.Count && currentThemeInstance.SlotTransforms[i] != null)
                {
                    slotTransform = currentThemeInstance.SlotTransforms[i];
                    pos = slotTransform.position;
                }
                else
                {
                    // Fallback
                    GameObject fallbackSlot = new GameObject($"Slot_Fallback_{i}");
                    fallbackSlot.transform.SetParent(transform);
                    fallbackSlot.transform.position = pos;
                    slotTransform = fallbackSlot.transform;
                }

                // Add or get SlotController
                SlotController slotCtrl = slotTransform.GetComponent<SlotController>();
                if (slotCtrl == null)
                {
                    slotCtrl = slotTransform.gameObject.AddComponent<SlotController>();
                }
                
                // Initialize placeholder visual
                Sprite placeholderSprite = currentTheme != null ? currentTheme.SlotPlaceholderSprite : null;
                slotCtrl.SetupPlaceholder(placeholderSprite);
                slotCtrl.ClearSlot(); // Ensure it starts empty
                stageSlots[i] = slotCtrl;
            }
        }

        /// <summary>
        /// Called by Drag and Drop UI. Returns true if assigned successfully.
        /// </summary>
        public bool TryAssignMonsterToSlot(MonsterDataSO monsterData, SlotController targetSlot)
        {
            if (monsterData == null || targetSlot == null || !isActive) return false;

            // Find index of the slot
            int slotIndex = System.Array.IndexOf(stageSlots, targetSlot);
            if (slotIndex < 0) return false;

            // If there's already a monster, deactivate it to pool
            if (activeMonsters[slotIndex] != null)
            {
                activeMonsters[slotIndex].OnLoopCompleted -= HandleMonsterLoopCompleted;
                activeMonsters[slotIndex].gameObject.SetActive(false);
                activeMonsters[slotIndex] = null;
            }

            // Spawn or get new monster from pool
            MonsterController controller = GetOrCreateMonster(monsterData);
            if (controller != null)
            {
                controller.OnLoopCompleted -= HandleMonsterLoopCompleted; // Unsubscribe to prevent duplicates
                controller.OnLoopCompleted += HandleMonsterLoopCompleted;

                controller.Setup(monsterData);
                targetSlot.AssignMonster(controller);
                activeMonsters[slotIndex] = controller;
                
                OnMonsterAssigned?.Invoke(controller, monsterData);
                
                return true;
            }

            return false;
        }

        private MonsterController GetOrCreateMonster(MonsterDataSO monsterData)
        {
            if (monsterData == null) return null;

            string poolKey = monsterData.MonsterID;
            if (!monsterPools.ContainsKey(poolKey))
            {
                monsterPools[poolKey] = new System.Collections.Generic.List<MonsterController>();
            }

            var pool = monsterPools[poolKey];
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].gameObject.activeInHierarchy)
                {
                    pool[i].gameObject.SetActive(true);
                    return pool[i];
                }
            }

            GameObject monsterGO;
            if (monsterData.MonsterPrefab != null)
            {
                monsterGO = Instantiate(monsterData.MonsterPrefab, transform);
            }
            else
            {
                // Fallback
                Debug.LogError($"[StageManager] MonsterData {monsterData.MonsterID} missing MonsterPrefab!");
                monsterGO = new GameObject($"Monster_Slot_Fallback_{monsterData.MonsterID}");
                monsterGO.transform.SetParent(transform);

                SpriteRenderer sr = monsterGO.AddComponent<SpriteRenderer>();
                sr.color = new Color(0.6f, 0.2f, 0.8f);
                sr.sortingOrder = 5;
                monsterGO.transform.localScale = Vector3.one * 1.2f;

                monsterGO.AddComponent<MonsterVox.Utils.SpriteColliderGenerator>();

                var qap = monsterGO.AddComponent<MonsterVox.Audio.QuantizedAudioPlayer>();
                if (audioConfig != null) qap.SetAudioConfig(audioConfig);
            }

            var controller = monsterGO.GetComponent<MonsterController>();
            if (controller == null)
            {
                controller = monsterGO.AddComponent<MonsterController>();
            }

            pool.Add(controller);
            return controller;
        }

        private void ClearMonsters()
        {
            if (activeMonsters == null) return;
            for (int i = 0; i < activeMonsters.Length; i++)
            {
                if (activeMonsters[i] != null)
                {
                    activeMonsters[i].OnLoopCompleted -= HandleMonsterLoopCompleted;
                    activeMonsters[i].gameObject.SetActive(false);
                }
            }
            activeMonsters = null;
        }

        private void HandleMonsterLoopCompleted(MonsterController monster)
        {
            if (monster != null && monster.IsSinging && coinSpawner != null)
            {
                Debug.Log($"[StageManager] Monster {monster.name} finished loop. Spawning 1 visual coin.");
                
                // Spawn 1 visual coin exactly at this monster's position
                coinSpawner.SpawnCoins(1, new Vector3[] { monster.transform.position });
                
                // Auto-collect instantly so UI updates immediately
                if (EconomyManager.Instance != null)
                {
                    Debug.Log($"[StageManager] Adding 1 coin to EconomyManager. Current total: {EconomyManager.Instance.Data.totalCoins}");
                    EconomyManager.Instance.AddCoins(1);
                }
                else
                {
                    Debug.LogWarning("[StageManager] EconomyManager.Instance is NULL! Cannot add coins.");
                }
            }
            else
            {
                Debug.LogWarning($"[StageManager] HandleMonsterLoopCompleted failed. Monster: {monster != null}, IsSinging: {(monster != null ? monster.IsSinging : false)}, CoinSpawner: {coinSpawner != null}");
            }
        }

        // FitBackgroundToScreen moved to ThemeController
    }
}
