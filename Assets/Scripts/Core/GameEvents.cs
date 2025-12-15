using System;
using UnityEngine;

namespace StickerSlam.Core
{
    /// <summary>
    /// Central event system for game communication. Decouples systems.
    /// </summary>
    public static class GameEvents
    {
        // Game State Events
        public static event Action OnGameStart;
        public static event Action OnGamePause;
        public static event Action OnGameResume;
        public static event Action OnGameEnd;
        
        // Level Events
        public static event Action<int> OnLevelStart; // level number
        public static event Action<int, int> OnLevelComplete; // level, score
        public static event Action<int> OnLevelFail; // level
        
        // Sticker Events
        public static event Action<Vector2, float> OnStickerPlace; // position, angle
        public static event Action<bool, bool> OnStickerPlaceAttempt; // perfect, collided
        public static event Action OnStickerOverlap; // collision detected
        
        // Scoring Events
        public static event Action<int> OnScoreChanged; // new score
        public static event Action OnPerfectPlacement; // perfect hit
        public static event Action<int> OnComboChanged; // combo count
        
        // Lives Events
        public static event Action<int> OnLivesChanged; // current lives
        public static event Action OnLivesDepleted; // game over
        
        // Progression Events
        public static event Action<int> OnStickersPlaced; // count
        public static event Action<int> OnStickersToPlaceChanged; // target count
        
        // Boss Events
        public static event Action OnBossFreezeTelegraph; // freeze coming
        public static event Action OnBossFreezeStart;
        public static event Action OnBossFreezeEnd;
        public static event Action OnBossStaminaFreeze; // perfect-based freeze
        
        // UI Events
        public static event Action OnShowWinScreen;
        public static event Action OnShowLoseScreen;
        public static event Action OnShowMainMenu;
        
        // Ad Events
        public static event Action<string> OnAdRequested; // ad type
        public static event Action<string> OnAdShown; // ad type
        public static event Action<string> OnAdRewarded; // reward type
        
        // Methods to invoke events
        public static void InvokeGameStart() => OnGameStart?.Invoke();
        public static void InvokeGamePause() => OnGamePause?.Invoke();
        public static void InvokeGameResume() => OnGameResume?.Invoke();
        public static void InvokeGameEnd() => OnGameEnd?.Invoke();
        
        public static void InvokeLevelStart(int level) => OnLevelStart?.Invoke(level);
        public static void InvokeLevelComplete(int level, int score) => OnLevelComplete?.Invoke(level, score);
        public static void InvokeLevelFail(int level) => OnLevelFail?.Invoke(level);
        
        public static void InvokeStickerPlace(Vector2 position, float angle) => OnStickerPlace?.Invoke(position, angle);
        public static void InvokeStickerPlaceAttempt(bool perfect, bool collided) => OnStickerPlaceAttempt?.Invoke(perfect, collided);
        public static void InvokeStickerOverlap() => OnStickerOverlap?.Invoke();
        
        public static void InvokeScoreChanged(int score) => OnScoreChanged?.Invoke(score);
        public static void InvokePerfectPlacement() => OnPerfectPlacement?.Invoke();
        public static void InvokeComboChanged(int combo) => OnComboChanged?.Invoke(combo);
        
        public static void InvokeLivesChanged(int lives) => OnLivesChanged?.Invoke(lives);
        public static void InvokeLivesDepleted() => OnLivesDepleted?.Invoke();
        
        public static void InvokeStickersPlaced(int count) => OnStickersPlaced?.Invoke(count);
        public static void InvokeStickersToPlaceChanged(int target) => OnStickersToPlaceChanged?.Invoke(target);
        
        public static void InvokeBossFreezeTelegraph() => OnBossFreezeTelegraph?.Invoke();
        public static void InvokeBossFreezeStart() => OnBossFreezeStart?.Invoke();
        public static void InvokeBossFreezeEnd() => OnBossFreezeEnd?.Invoke();
        public static void InvokeBossStaminaFreeze() => OnBossStaminaFreeze?.Invoke();
        
        public static void InvokeShowWinScreen() => OnShowWinScreen?.Invoke();
        public static void InvokeShowLoseScreen() => OnShowLoseScreen?.Invoke();
        public static void InvokeShowMainMenu() => OnShowMainMenu?.Invoke();
        
        public static void InvokeAdRequested(string adType) => OnAdRequested?.Invoke(adType);
        public static void InvokeAdShown(string adType) => OnAdShown?.Invoke(adType);
        public static void InvokeAdRewarded(string rewardType) => OnAdRewarded?.Invoke(rewardType);
    }
}

