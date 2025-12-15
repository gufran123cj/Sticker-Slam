using UnityEngine;
using StickerSlam.ScriptableObjects;
using StickerSlam.Core;

namespace StickerSlam.Game
{
    /// <summary>
    /// Handles sticker placement logic. Detects input and places stickers.
    /// </summary>
    public class StickerPlacementSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RotationSystem rotationSystem;
        [SerializeField] private CollisionSystem collisionSystem;
        [SerializeField] private ScoringSystem scoringSystem;
        
        [Header("Sticker Prefab")]
        [SerializeField] private GameObject stickerPrefab;
        
        [Header("Settings")]
        [SerializeField] private LayerMask inputLayer;
        
        private LevelConfig currentLevelConfig;
        private bool waitingForFreezeEnd = false;
        
        private void Update()
        {
            HandleInput();
            
            // Process buffered input when freeze ends
            if (waitingForFreezeEnd && !rotationSystem.IsFrozen)
            {
                waitingForFreezeEnd = false;
                // Execute buffered placement immediately after freeze ends
                ExecutePlacement();
            }
        }
        
        /// <summary>
        /// Initialize placement system
        /// </summary>
        public void Initialize(LevelConfig config)
        {
            currentLevelConfig = config;
            collisionSystem.Initialize(config);
            waitingForFreezeEnd = false;
        }
        
        /// <summary>
        /// Handle input (tap/click)
        /// </summary>
        private void HandleInput()
        {
            bool inputPressed = false;
            
            // Mobile touch
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    inputPressed = true;
                }
            }
            // Editor mouse
            else if (Input.GetMouseButtonDown(0))
            {
                inputPressed = true;
            }
            
            if (inputPressed)
            {
                AttemptPlaceSticker();
            }
        }
        
        /// <summary>
        /// Attempt to place a sticker at current rotation angle
        /// </summary>
        public void AttemptPlaceSticker()
        {
            // Check if frozen (boss freeze with input buffering)
            if (rotationSystem.IsFrozen)
            {
                // Buffer the input - will execute when freeze ends
                waitingForFreezeEnd = true;
                return;
            }
            
            // Not frozen - execute immediately
            ExecutePlacement();
        }
        
        /// <summary>
        /// Execute sticker placement (called immediately or after freeze)
        /// </summary>
        private void ExecutePlacement()
        {
            // Get current position and angle
            Vector2 position = rotationSystem.CurrentPosition;
            float angle = rotationSystem.CurrentAngle;
            
            // Check for perfect placement
            bool isPerfect = rotationSystem.IsPerfectAngle(currentLevelConfig.perfectAngleTolerance);
            
            // Check for collision
            bool hasCollision = collisionSystem.CheckOverlap(position);
            
            if (hasCollision)
            {
                // Overlap - fail
                GameEvents.InvokeStickerPlaceAttempt(false, true);
                GameManager.Instance.OnStickerPlaced(false, true);
                return;
            }
            
            // Place sticker
            PlaceSticker(position, angle, isPerfect);
        }
        
        /// <summary>
        /// Place a sticker at the given position
        /// </summary>
        private void PlaceSticker(Vector2 position, float angle, bool perfect)
        {
            // Create sticker GameObject
            GameObject stickerObj = Instantiate(stickerPrefab, position, Quaternion.Euler(0, 0, angle));
            
            // Add to collision system
            collisionSystem.AddPlacedSticker(position, angle, stickerObj);
            
            // Calculate score
            int score = scoringSystem.CalculateScore(perfect);
            GameManager.Instance.AddScore(score);
            
            // Invoke events
            GameEvents.InvokeStickerPlace(position, angle);
            GameEvents.InvokeStickerPlaceAttempt(perfect, false);
            
            if (perfect)
            {
                GameEvents.InvokePerfectPlacement();
                
                // Trigger stamina freeze for boss
                if (currentLevelConfig.isBoss)
                {
                    rotationSystem.TriggerStaminaFreeze();
                }
            }
            
            // Notify game manager
            GameManager.Instance.OnStickerPlaced(perfect, false);
            
            Debug.Log($"Sticker placed at angle {angle:F1}° - Perfect: {perfect}, Score: {score}");
        }
        
        /// <summary>
        /// Clear all placed stickers
        /// </summary>
        public void Clear()
        {
            collisionSystem.Clear();
        }
    }
}

