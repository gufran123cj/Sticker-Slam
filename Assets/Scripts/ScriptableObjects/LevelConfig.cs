using UnityEngine;

namespace StickerSlam.ScriptableObjects
{
    /// <summary>
    /// Level configuration data. Contains all parameters for a single level.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Sticker Slam/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Basic Settings")]
        [Tooltip("Number of stickers to place to win")]
        public int stickersToPlace = 8;
        
        [Tooltip("Base rotation speed in rotations per second")]
        public float baseRps = 2f;
        
        [Header("Speed Variation")]
        [Tooltip("Random speed range (min, max) in RPS. Only used if speed varies.")]
        public Vector2 rpsRandomRange = new Vector2(2f, 4f);
        
        [Header("Direction Reversal")]
        [Tooltip("Allow direction reversals (clockwise <-> counter-clockwise)")]
        public bool allowReverse = false;
        
        [Tooltip("Time range between reversals in seconds (min, max)")]
        public Vector2 reverseIntervalRange = new Vector2(3f, 6f);
        
        [Header("Freeze Mechanics")]
        [Tooltip("Enable freeze mechanics (pause rotation temporarily)")]
        public bool freezeEnabled = false;
        
        [Tooltip("Time range between freezes in seconds (min, max)")]
        public Vector2 freezeIntervalRange = new Vector2(5f, 10f);
        
        [Tooltip("Freeze duration in seconds")]
        public float freezeDuration = 0.5f;
        
        [Header("Collision")]
        [Tooltip("Collision radius in world units. Stickers overlap if distance < this value.")]
        public float collisionRadiusWorld = 50f;
        
        [Header("Perfect Placement")]
        [Tooltip("Angle tolerance in degrees for perfect placement (±5 = 5 degrees)")]
        public float perfectAngleTolerance = 5f;
        
        [Header("Surface")]
        [Tooltip("Surface type for this level")]
        public SurfaceType surfaceType = SurfaceType.Laptop;
        
        [Header("Boss Level")]
        [Tooltip("Is this a boss level?")]
        public bool isBoss = false;
        
        [Tooltip("Telegraph time before freeze (boss only)")]
        public float bossTelegraphTime = 0.4f;
        
        [Tooltip("Freeze duration range for boss (min, max)")]
        public Vector2 bossFreezeDurationRange = new Vector2(0.25f, 0.4f);
        
        [Tooltip("Number of perfects in a row to trigger stamina freeze (boss only)")]
        public int bossStaminaPerfectThreshold = 2;
    }
    
    /// <summary>
    /// Surface types available in the game
    /// </summary>
    public enum SurfaceType
    {
        Laptop = 0,      // Level 1
        Phone = 1,       // Level 15
        Notebook = 2,   // Level 30
        Box = 3,        // Level 45
        Skateboard = 4  // Level 60
    }
}

