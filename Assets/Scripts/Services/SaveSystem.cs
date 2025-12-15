using System;
using System.IO;
using UnityEngine;

namespace StickerSlam.Services
{
    /// <summary>
    /// Save system using JSON file at Application.persistentDataPath.
    /// PlayerPrefs only for tiny flags.
    /// </summary>
    public class SaveSystem : MonoBehaviour
    {
        private const string SAVE_FILE_NAME = "save.json";
        private const int SAVE_VERSION = 1;
        
        private string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        
        /// <summary>
        /// Save game data
        /// </summary>
        public void SaveGame(GameSaveData data)
        {
            try
            {
                data.version = SAVE_VERSION;
                data.saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SaveFilePath, json);
                
                Debug.Log($"SaveSystem: Game saved to {SaveFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"SaveSystem: Failed to save game - {e.Message}");
            }
        }
        
        /// <summary>
        /// Load game data
        /// </summary>
        public GameSaveData LoadGame()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    Debug.Log("SaveSystem: No save file found, creating default");
                    return CreateDefaultSave();
                }
                
                string json = File.ReadAllText(SaveFilePath);
                GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
                
                // Validate version
                if (data.version != SAVE_VERSION)
                {
                    Debug.LogWarning($"SaveSystem: Save version mismatch ({data.version} vs {SAVE_VERSION}), using default");
                    return CreateDefaultSave();
                }
                
                Debug.Log($"SaveSystem: Game loaded from {SaveFilePath}");
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"SaveSystem: Failed to load game - {e.Message}");
                return CreateDefaultSave();
            }
        }
        
        /// <summary>
        /// Create default save data
        /// </summary>
        private GameSaveData CreateDefaultSave()
        {
            return new GameSaveData
            {
                version = SAVE_VERSION,
                currentLevel = 1,
                highScore = 0,
                totalScore = 0,
                unlockedStickers = new System.Collections.Generic.List<string>(),
                completedLevels = new System.Collections.Generic.List<int>(),
                gems = 0,
                saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
        
        /// <summary>
        /// Delete save file
        /// </summary>
        public void DeleteSave()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    File.Delete(SaveFilePath);
                    Debug.Log("SaveSystem: Save file deleted");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"SaveSystem: Failed to delete save - {e.Message}");
            }
        }
        
        /// <summary>
        /// Save tiny flag to PlayerPrefs
        /// </summary>
        public void SaveFlag(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Load tiny flag from PlayerPrefs
        /// </summary>
        public bool LoadFlag(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }
    }
    
    /// <summary>
    /// Game save data structure
    /// </summary>
    [System.Serializable]
    public class GameSaveData
    {
        public int version;
        public int currentLevel;
        public int highScore;
        public int totalScore;
        public System.Collections.Generic.List<string> unlockedStickers;
        public System.Collections.Generic.List<int> completedLevels;
        public int gems;
        public string saveTime;
    }
}

