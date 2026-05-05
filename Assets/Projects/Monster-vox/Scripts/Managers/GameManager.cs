using System;
using UnityEngine;
using MonsterVox.Data;

namespace MonsterVox.Managers
{
    /// <summary>
    /// Singleton state machine managing transitions between Home and Stage.
    /// Other systems subscribe to OnStateChanged to react accordingly.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Home;
        public ThemeDataSO ActiveTheme { get; private set; }

        /// <summary>
        /// Fired whenever the game state changes. Listeners receive the new state.
        /// </summary>
        public event Action<GameState> OnStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            // Ensure we start at Home
            SetState(GameState.Home);
        }

        public void GoToStage(ThemeDataSO theme)
        {
            if (theme == null)
            {
                Debug.LogWarning("[GameManager] Cannot enter Stage with null theme.");
                return;
            }

            ActiveTheme = theme;
            SetState(GameState.Stage);
        }

        public void GoToHome()
        {
            ActiveTheme = null;
            SetState(GameState.Home);
        }

        private void SetState(GameState newState)
        {
            if (CurrentState == newState && newState != GameState.Home) return;
            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
