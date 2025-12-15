using UnityEngine;
using StickerSlam.ScriptableObjects;

namespace StickerSlam.Core
{
    /// <summary>
    /// Bootstrapper that initializes the game on scene load.
    /// Ensures GameManager exists and systems are ready.
    /// </summary>
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Required Assets")]
        [SerializeField] private LevelDatabase levelDatabase;
        
        [Header("Game Manager Prefab")]
        [SerializeField] private GameObject gameManagerPrefab;
        
        private void Awake()
        {
            // Ensure GameManager exists
            if (GameManager.Instance == null)
            {
                if (gameManagerPrefab != null)
                {
                    Instantiate(gameManagerPrefab);
                }
                else
                {
                    // Create GameManager manually if no prefab
                    GameObject gmObj = new GameObject("GameManager");
                    GameManager gm = gmObj.AddComponent<GameManager>();
                    // Note: LevelDatabase must be assigned in Inspector
                }
            }
            
            // Validate level database
            if (levelDatabase == null)
            {
                Debug.LogWarning("GameBootstrapper: LevelDatabase not assigned. Some features may not work.");
            }
        }
    }
}

