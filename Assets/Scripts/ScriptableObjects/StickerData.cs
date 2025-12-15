using UnityEngine;

namespace StickerSlam.ScriptableObjects
{
    /// <summary>
    /// Data for a single sticker. Used in sticker collection and album.
    /// </summary>
    [CreateAssetMenu(fileName = "StickerData", menuName = "Sticker Slam/Sticker Data")]
    public class StickerData : ScriptableObject
    {
        [Header("Basic Info")]
        public string stickerId;
        public string displayName;
        public Sprite stickerSprite;
        
        [Header("Collection")]
        public StickerSet set;
        public int rarity; // 0 = common, 1 = rare, 2 = epic, 3 = legendary
        
        [Header("Unlock")]
        public bool isUnlocked = false;
        public int unlockLevel = 0; // Level at which this sticker unlocks
    }
    
    /// <summary>
    /// Sticker sets/categories
    /// </summary>
    public enum StickerSet
    {
        Fire = 0,
        Nature = 1,
        Space = 2,
        Food = 3,
        Retro = 4
    }
}

