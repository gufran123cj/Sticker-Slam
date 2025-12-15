using UnityEngine;

namespace StickerSlam.ScriptableObjects
{
    /// <summary>
    /// Surface visual data. Contains sprite/background for each surface type.
    /// </summary>
    [CreateAssetMenu(fileName = "SurfaceData", menuName = "Sticker Slam/Surface Data")]
    public class SurfaceData : ScriptableObject
    {
        public SurfaceType surfaceType;
        public Sprite backgroundSprite;
        public Color backgroundColor = Color.white;
        
        [Tooltip("Center point of the circular track in world space")]
        public Vector2 trackCenter = Vector2.zero;
        
        [Tooltip("Radius of the circular track in world units")]
        public float trackRadius = 200f;
    }
}

