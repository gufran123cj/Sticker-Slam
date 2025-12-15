using UnityEngine;
using StickerSlam.ScriptableObjects;
using StickerSlam.Core;

namespace StickerSlam.Core
{
    /// <summary>
    /// Main game manager. Singleton that coordinates all game systems.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("References")]
        [SerializeField] private LevelDatabase levelDatabase;
        
        [Header("Game State")]
        private int currentLevel = 1;
        private int currentScore = 0;
        private int currentLives = 3;
        private int stickersPlaced = 0;
        private int stickersToPlace = 8;
        
        private bool isGameActive = false;
        private bool isPaused = false;
        
        private LevelConfig currentLevelConfig;
        
        // Properties
        public int CurrentLevel => currentLevel;
        public int CurrentScore => currentScore;
        public int CurrentLives => currentLives;
        public int StickersPlaced => stickersPlaced;
        public int StickersToPlace => stickersToPlace;
        public bool IsGameActive => isGameActive;
        public bool IsPaused => isPaused;
        public LevelConfig CurrentLevelConfig => currentLevelConfig;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Validate references
            if (levelDatabase == null)
            {
                Debug.LogError("GameManager: LevelDatabase is not assigned!");
            }
        }
        
        private void Start()
        {
            // Start at main menu
            ShowMainMenu();
        }
        
        /// <summary>
        /// Start a new game or level
        /// </summary>
        public void StartLevel(int levelNumber = -1)
        {
            if (levelNumber > 0)
            {
                currentLevel = levelNumber;
            }
            
            // Load level config
            currentLevelConfig = levelDatabase.GetLevelConfig(currentLevel);
            stickersToPlace = currentLevelConfig.stickersToPlace;
            stickersPlaced = 0;
            
            // Reset game state
            isGameActive = true;
            isPaused = false;
            
            // Reset lives if starting new game (level 1)
            if (currentLevel == 1)
            {
                currentLives = 3;
                currentScore = 0;
            }
            
            // Invoke events
            GameEvents.InvokeLevelStart(currentLevel);
            GameEvents.InvokeStickersToPlaceChanged(stickersToPlace);
            GameEvents.InvokeLivesChanged(currentLives);
            GameEvents.InvokeScoreChanged(currentScore);
            
            Debug.Log($"GameManager: Started Level {currentLevel}");
        }
        
        /// <summary>
        /// Called when a sticker is successfully placed
        /// </summary>
        public void OnStickerPlaced(bool perfect, bool collided)
        {
            if (!isGameActive) return;
            
            if (collided)
            {
                // Overlap detected - lose a life
                LoseLife();
                GameEvents.InvokeStickerOverlap();
                return;
            }
            
            // Successful placement
            stickersPlaced++;
            GameEvents.InvokeStickersPlaced(stickersPlaced);
            
            // Check win condition
            if (stickersPlaced >= stickersToPlace)
            {
                CompleteLevel();
            }
        }
        
        /// <summary>
        /// Lose a life
        /// </summary>
        public void LoseLife()
        {
            currentLives--;
            GameEvents.InvokeLivesChanged(currentLives);
            
            if (currentLives <= 0)
            {
                currentLives = 0;
                GameOver();
            }
        }
        
        /// <summary>
        /// Complete current level
        /// </summary>
        private void CompleteLevel()
        {
            isGameActive = false;
            GameEvents.InvokeLevelComplete(currentLevel, currentScore);
            GameEvents.InvokeShowWinScreen();
            
            Debug.Log($"GameManager: Level {currentLevel} completed with score {currentScore}");
        }
        
        /// <summary>
        /// Game over - no lives left
        /// </summary>
        private void GameOver()
        {
            isGameActive = false;
            GameEvents.InvokeLivesDepleted();
            GameEvents.InvokeLevelFail(currentLevel);
            GameEvents.InvokeShowLoseScreen();
            
            Debug.Log($"GameManager: Game Over at Level {currentLevel}");
        }
        
        /// <summary>
        /// Add score
        /// </summary>
        public void AddScore(int points)
        {
            currentScore += points;
            GameEvents.InvokeScoreChanged(currentScore);
        }
        
        /// <summary>
        /// Continue to next level
        /// </summary>
        public void NextLevel()
        {
            currentLevel++;
            StartLevel();
        }
        
        /// <summary>
        /// Retry current level
        /// </summary>
        public void RetryLevel()
        {
            StartLevel(currentLevel);
        }
        
        /// <summary>
        /// Continue after watching ad (restore 1 life)
        /// </summary>
        public void ContinueWithAd()
        {
            currentLives = 1;
            GameEvents.InvokeLivesChanged(currentLives);
            isGameActive = true;
        }
        
        /// <summary>
        /// Pause game
        /// </summary>
        public void PauseGame()
        {
            if (!isGameActive) return;
            isPaused = true;
            Time.timeScale = 0f;
            GameEvents.InvokeGamePause();
        }
        
        /// <summary>
        /// Resume game
        /// </summary>
        public void ResumeGame()
        {
            if (!isGameActive) return;
            isPaused = false;
            Time.timeScale = 1f;
            GameEvents.InvokeGameResume();
        }
        
        /// <summary>
        /// Show main menu
        /// </summary>
        public void ShowMainMenu()
        {
            isGameActive = false;
            isPaused = false;
            Time.timeScale = 1f;
            GameEvents.InvokeShowMainMenu();
        }
    }
}

