using System.Collections.Generic;
using UnityEngine;
using StickerSlam.ScriptableObjects;

namespace StickerSlam.Game
{
    /// <summary>
    /// Handles collision detection between placed stickers.
    /// Uses distance-based overlap detection.
    /// </summary>
    public class CollisionSystem : MonoBehaviour
    {
        private List<PlacedSticker> placedStickers = new List<PlacedSticker>();
        private float collisionRadius = 50f;
        
        /// <summary>
        /// Initialize with collision radius from level config
        /// </summary>
        public void Initialize(LevelConfig config)
        {
            collisionRadius = config.collisionRadiusWorld;
            placedStickers.Clear();
        }
        
        /// <summary>
        /// Check if a new sticker position would overlap with any placed sticker
        /// </summary>
        public bool CheckOverlap(Vector2 newPosition)
        {
            foreach (var sticker in placedStickers)
            {
                float distance = Vector2.Distance(newPosition, sticker.position);
                if (distance < collisionRadius)
                {
                    return true; // Overlap detected
                }
            }
            
            return false; // No overlap
        }
        
        /// <summary>
        /// Add a placed sticker to the system
        /// </summary>
        public void AddPlacedSticker(Vector2 position, float angle, GameObject stickerObject)
        {
            PlacedSticker sticker = new PlacedSticker
            {
                position = position,
                angle = angle,
                gameObject = stickerObject
            };
            
            placedStickers.Add(sticker);
        }
        
        /// <summary>
        /// Clear all placed stickers
        /// </summary>
        public void Clear()
        {
            foreach (var sticker in placedStickers)
            {
                if (sticker.gameObject != null)
                {
                    Destroy(sticker.gameObject);
                }
            }
            
            placedStickers.Clear();
        }
        
        /// <summary>
        /// Get count of placed stickers
        /// </summary>
        public int GetPlacedCount()
        {
            return placedStickers.Count;
        }
    }
    
    /// <summary>
    /// Data structure for a placed sticker
    /// </summary>
    public class PlacedSticker
    {
        public Vector2 position;
        public float angle;
        public GameObject gameObject;
    }
}

