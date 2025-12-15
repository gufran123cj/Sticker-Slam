using System.Collections.Generic;
using UnityEngine;

namespace StickerSlam.ScriptableObjects
{
    /// <summary>
    /// Database containing all level configurations. Provides lookup by level number.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Sticker Slam/Level Database")]
    public class LevelDatabase : ScriptableObject
    {
        [Tooltip("List of all level configurations. Index = level number - 1")]
        public List<LevelConfig> levels = new List<LevelConfig>();
        
        /// <summary>
        /// Get level config for a specific level number (1-indexed)
        /// </summary>
        public LevelConfig GetLevelConfig(int levelNumber)
        {
            int index = levelNumber - 1;
            if (index < 0 || index >= levels.Count)
            {
                Debug.LogWarning($"Level {levelNumber} not found in database. Using default config.");
                return GenerateDefaultConfig(levelNumber);
            }
            
            return levels[index];
        }
        
        /// <summary>
        /// Generate a default config for levels beyond the database
        /// </summary>
        private LevelConfig GenerateDefaultConfig(int levelNumber)
        {
            LevelConfig config = ScriptableObject.CreateInstance<LevelConfig>();
            config.stickersToPlace = 8 + (levelNumber / 5); // Increase difficulty
            config.baseRps = 2f;
            config.rpsRandomRange = new Vector2(2f, 4f);
            config.allowReverse = levelNumber >= 16;
            config.reverseIntervalRange = new Vector2(3f, 6f);
            config.freezeEnabled = levelNumber >= 31;
            config.freezeIntervalRange = new Vector2(5f, 10f);
            config.freezeDuration = 0.5f;
            config.collisionRadiusWorld = 50f;
            config.perfectAngleTolerance = 5f;
            config.surfaceType = GetSurfaceTypeForLevel(levelNumber);
            config.isBoss = (levelNumber % 10 == 0);
            
            if (config.isBoss)
            {
                config.bossTelegraphTime = 0.4f;
                config.bossFreezeDurationRange = new Vector2(0.25f, 0.4f);
                config.bossStaminaPerfectThreshold = 2;
            }
            
            return config;
        }
        
        /// <summary>
        /// Determine surface type based on level number
        /// </summary>
        private SurfaceType GetSurfaceTypeForLevel(int levelNumber)
        {
            if (levelNumber <= 5) return SurfaceType.Laptop;
            if (levelNumber <= 15) return SurfaceType.Phone;
            if (levelNumber <= 30) return SurfaceType.Notebook;
            if (levelNumber <= 45) return SurfaceType.Box;
            if (levelNumber <= 60) return SurfaceType.Skateboard;
            
            // After level 60, random
            return (SurfaceType)Random.Range(0, 5);
        }
    }
}

