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
                activeMonsters[slotIndex].gameObject.SetActive(false);
                activeMonsters[slotIndex] = null;
            }

            // Spawn or get new monster from pool
            MonsterController controller = GetOrCreateMonster(monsterData);
            if (controller != null)
            {
                controller.Setup(monsterData);
                targetSlot.AssignMonster(controller);
                activeMonsters[slotIndex] = controller;
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
                    activeMonsters[i].gameObject.SetActive(false);
                }
            }
            activeMonsters = null;
        }

        private void Update()
        {
            if (!isActive || bgmSource == null || bgmLoopDuration <= 0) return;

            // Track BGM loop completion using dspTime for precision
            double elapsed = AudioSettings.dspTime - bgmLoopStartDspTime;
            if (elapsed >= bgmLoopDuration)
            {
                bgmLoopStartDspTime += bgmLoopDuration;
                OnBgmLoopCompleted();
            }
        }

        private void OnBgmLoopCompleted()
        {
            if (activeMonsters == null || coinSpawner == null) return;

            int singingCount = 0;
            for (int i = 0; i < activeMonsters.Length; i++)
            {
                if (activeMonsters[i] != null && activeMonsters[i].IsSinging)
                {
                    singingCount++;
                }
            }

            if (singingCount > 0)
            {
                // Formula from Game_Data: X = singingCount * Base_Drop (1)
                coinSpawner.SpawnCoins(singingCount, GetSingingMonsterPositions());
            }
        }

        private Vector3[] GetSingingMonsterPositions()
        {
            int count = 0;
            for (int i = 0; i < activeMonsters.Length; i++)
            {
                if (activeMonsters[i] != null && activeMonsters[i].IsSinging)
                    count++;
            }

            Vector3[] positions = new Vector3[count];
            int idx = 0;
            for (int i = 0; i < activeMonsters.Length; i++)
            {
                if (activeMonsters[i] != null && activeMonsters[i].IsSinging)
                {
                    positions[idx++] = activeMonsters[i].transform.position;
                }
            }
            return positions;
        }

        // FitBackgroundToScreen moved to ThemeController
    }
}
