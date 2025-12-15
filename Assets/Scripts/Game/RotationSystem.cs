using UnityEngine;
using StickerSlam.ScriptableObjects;
using StickerSlam.Core;

namespace StickerSlam.Game
{
    /// <summary>
    /// Handles rotation of stickers around a circular track.
    /// Supports variable speed, direction reversal, and freeze mechanics.
    /// </summary>
    public class RotationSystem : MonoBehaviour
    {
        [Header("Track Settings")]
        [SerializeField] private Vector2 trackCenter = Vector2.zero;
        [SerializeField] private float trackRadius = 200f;
        
        [Header("Rotation")]
        private float currentAngle = 0f; // 0-360 degrees
        private float currentRps = 2f; // rotations per second
        private bool isClockwise = true;
        private bool isFrozen = false;
        
        [Header("Dynamic Settings")]
        private LevelConfig levelConfig;
        private float speedChangeTimer = 0f;
        private float reverseTimer = 0f;
        private float freezeTimer = 0f;
        private float nextFreezeTime = 0f;
        
        // Boss mechanics
        private bool isBossLevel = false;
        private float bossTelegraphTimer = 0f;
        private bool isTelegraphing = false;
        private int perfectCount = 0;
        
        // Properties
        public float CurrentAngle => currentAngle;
        public Vector2 CurrentPosition => GetPositionAtAngle(currentAngle);
        public bool IsFrozen => isFrozen;
        
        private void Update()
        {
            if (isFrozen) return;
            
            // Update rotation
            float rotationSpeed = currentRps * 360f; // degrees per second
            if (!isClockwise) rotationSpeed = -rotationSpeed;
            
            currentAngle += rotationSpeed * Time.deltaTime;
            
            // Normalize angle to 0-360
            if (currentAngle >= 360f) currentAngle -= 360f;
            if (currentAngle < 0f) currentAngle += 360f;
            
            // Update dynamic mechanics
            UpdateSpeedVariation();
            UpdateDirectionReversal();
            UpdateFreezeMechanics();
        }
        
        /// <summary>
        /// Initialize rotation system with level config
        /// </summary>
        public void Initialize(LevelConfig config, Vector2 center, float radius)
        {
            levelConfig = config;
            trackCenter = center;
            trackRadius = radius;
            
            currentRps = config.baseRps;
            isClockwise = true;
            isFrozen = false;
            currentAngle = 0f;
            
            isBossLevel = config.isBoss;
            perfectCount = 0;
            
            // Initialize timers
            speedChangeTimer = 0f;
            reverseTimer = 0f;
            freezeTimer = 0f;
            nextFreezeTime = Random.Range(config.freezeIntervalRange.x, config.freezeIntervalRange.y);
        }
        
        /// <summary>
        /// Update speed variation (levels 6-15)
        /// </summary>
        private void UpdateSpeedVariation()
        {
            if (levelConfig == null) return;
            
            // Only vary speed if enabled and in appropriate level range
            if (currentRps == levelConfig.baseRps) return; // Not using variation yet
            
            speedChangeTimer += Time.deltaTime;
            if (speedChangeTimer >= 3f) // Change speed every 3 seconds
            {
                currentRps = Random.Range(levelConfig.rpsRandomRange.x, levelConfig.rpsRandomRange.y);
                speedChangeTimer = 0f;
            }
        }
        
        /// <summary>
        /// Update direction reversal (levels 16-30)
        /// </summary>
        private void UpdateDirectionReversal()
        {
            if (levelConfig == null || !levelConfig.allowReverse) return;
            
            reverseTimer += Time.deltaTime;
            float nextReverse = Random.Range(levelConfig.reverseIntervalRange.x, levelConfig.reverseIntervalRange.y);
            
            if (reverseTimer >= nextReverse)
            {
                isClockwise = !isClockwise;
                reverseTimer = 0f;
            }
        }
        
        /// <summary>
        /// Update freeze mechanics (levels 31+)
        /// </summary>
        private void UpdateFreezeMechanics()
        {
            if (levelConfig == null || !levelConfig.freezeEnabled) return;
            
            if (isBossLevel)
            {
                UpdateBossFreeze();
            }
            else
            {
                UpdateNormalFreeze();
            }
        }
        
        /// <summary>
        /// Normal freeze (level 31+)
        /// </summary>
        private void UpdateNormalFreeze()
        {
            freezeTimer += Time.deltaTime;
            
            if (freezeTimer >= nextFreezeTime && !isFrozen)
            {
                StartFreeze(levelConfig.freezeDuration);
                freezeTimer = 0f;
                nextFreezeTime = Random.Range(levelConfig.freezeIntervalRange.x, levelConfig.freezeIntervalRange.y);
            }
        }
        
        /// <summary>
        /// Boss freeze mechanics
        /// </summary>
        private void UpdateBossFreeze()
        {
            // Telegraph before freeze
            if (isTelegraphing)
            {
                bossTelegraphTimer += Time.deltaTime;
                if (bossTelegraphTimer >= levelConfig.bossTelegraphTime)
                {
                    StartFreeze(Random.Range(levelConfig.bossFreezeDurationRange.x, levelConfig.bossFreezeDurationRange.y));
                    isTelegraphing = false;
                    bossTelegraphTimer = 0f;
                }
            }
            else
            {
                freezeTimer += Time.deltaTime;
                if (freezeTimer >= nextFreezeTime && !isFrozen)
                {
                    // Start telegraph
                    isTelegraphing = true;
                    GameEvents.InvokeBossFreezeTelegraph();
                    freezeTimer = 0f;
                    nextFreezeTime = Random.Range(levelConfig.freezeIntervalRange.x, levelConfig.freezeIntervalRange.y);
                }
            }
        }
        
        /// <summary>
        /// Start freeze
        /// </summary>
        private void StartFreeze(float duration)
        {
            isFrozen = true;
            GameEvents.InvokeBossFreezeStart();
            
            Invoke(nameof(EndFreeze), duration);
        }
        
        /// <summary>
        /// End freeze
        /// </summary>
        private void EndFreeze()
        {
            isFrozen = false;
            GameEvents.InvokeBossFreezeEnd();
        }
        
        /// <summary>
        /// Trigger stamina freeze (boss only, after perfects)
        /// </summary>
        public void TriggerStaminaFreeze()
        {
            if (!isBossLevel) return;
            
            perfectCount++;
            if (perfectCount >= levelConfig.bossStaminaPerfectThreshold)
            {
                perfectCount = 0;
                StartFreeze(Random.Range(levelConfig.bossFreezeDurationRange.x, levelConfig.bossFreezeDurationRange.y));
                GameEvents.InvokeBossStaminaFreeze();
            }
        }
        
        /// <summary>
        /// Get world position at given angle
        /// </summary>
        public Vector2 GetPositionAtAngle(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = trackCenter.x + trackRadius * Mathf.Cos(radians);
            float y = trackCenter.y + trackRadius * Mathf.Sin(radians);
            return new Vector2(x, y);
        }
        
        /// <summary>
        /// Get angle for perfect placement (12/3/6/9 o'clock)
        /// </summary>
        public float GetNearestPerfectAngle()
        {
            float[] perfectAngles = { 0f, 90f, 180f, 270f }; // 12, 3, 6, 9 o'clock
            
            float nearest = perfectAngles[0];
            float minDistance = Mathf.Abs(Mathf.DeltaAngle(currentAngle, perfectAngles[0]));
            
            for (int i = 1; i < perfectAngles.Length; i++)
            {
                float distance = Mathf.Abs(Mathf.DeltaAngle(currentAngle, perfectAngles[i]));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = perfectAngles[i];
                }
            }
            
            return nearest;
        }
        
        /// <summary>
        /// Check if current angle is perfect
        /// </summary>
        public bool IsPerfectAngle(float tolerance = 5f)
        {
            float nearestPerfect = GetNearestPerfectAngle();
            float distance = Mathf.Abs(Mathf.DeltaAngle(currentAngle, nearestPerfect));
            return distance <= tolerance;
        }
    }
}

