using UnityEngine;
using StickerSlam.ScriptableObjects;
using StickerSlam.Core;

namespace StickerSlam.Game
{
    /// <summary>
    /// Main game controller that coordinates all game systems.
    /// Initializes systems when level starts.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private RotationSystem rotationSystem;
        [SerializeField] private CollisionSystem collisionSystem;
        [SerializeField] private ScoringSystem scoringSystem;
        [SerializeField] private StickerPlacementSystem stickerPlacementSystem;
        
        [Header("Surface Settings")]
        [SerializeField] private Vector2 trackCenter = Vector2.zero;
        [SerializeField] private float trackRadius = 200f;
        
        private void OnEnable()
        {
            GameEvents.OnLevelStart += OnLevelStart;
            GameEvents.OnGameEnd += OnGameEnd;
        }
        
        private void OnDisable()
        {
            GameEvents.OnLevelStart -= OnLevelStart;
            GameEvents.OnGameEnd -= OnGameEnd;
        }
        
        /// <summary>
        /// Initialize all systems when level starts
        /// </summary>
        private void OnLevelStart(int level)
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentLevelConfig == null)
            {
                Debug.LogError("GameController: GameManager or LevelConfig not available!");
                return;
            }
            
            LevelConfig config = GameManager.Instance.CurrentLevelConfig;
            
            // Initialize rotation system
            if (rotationSystem != null)
            {
                rotationSystem.Initialize(config, trackCenter, trackRadius);
            }
            
            // Initialize collision system
            if (collisionSystem != null)
            {
                collisionSystem.Initialize(config);
            }
            
            // Initialize scoring system
            if (scoringSystem != null)
            {
                scoringSystem.Reset();
            }
            
            // Initialize sticker placement system
            if (stickerPlacementSystem != null)
            {
                stickerPlacementSystem.Initialize(config);
            }
            
            // Clear any existing stickers
            if (stickerPlacementSystem != null)
            {
                stickerPlacementSystem.Clear();
            }
            
            Debug.Log($"GameController: Level {level} initialized");
        }
        
        /// <summary>
        /// Clean up when game ends
        /// </summary>
        private void OnGameEnd()
        {
            // Clear stickers
            if (stickerPlacementSystem != null)
            {
                stickerPlacementSystem.Clear();
            }
        }
    }
}

