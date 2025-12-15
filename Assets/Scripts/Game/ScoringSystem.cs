using UnityEngine;
using StickerSlam.Core;

namespace StickerSlam.Game
{
    /// <summary>
    /// Handles scoring, perfect detection, and combo system.
    /// </summary>
    public class ScoringSystem : MonoBehaviour
    {
        [Header("Scoring")]
        private const int BASE_SCORE_PER_STICKER = 100;
        private const int PERFECT_BONUS = 50;
        
        private int currentCombo = 0;
        private int maxCombo = 0;
        private int perfectCount = 0;
        
        // Combo multipliers
        private const int COMBO_3_MULTIPLIER = 2;
        private const int COMBO_5_MULTIPLIER = 3;
        
        // Properties
        public int CurrentCombo => currentCombo;
        public int MaxCombo => maxCombo;
        public int PerfectCount => perfectCount;
        
        private void OnEnable()
        {
            GameEvents.OnStickerPlaceAttempt += OnStickerPlaceAttempt;
            GameEvents.OnPerfectPlacement += OnPerfectPlacement;
        }
        
        private void OnDisable()
        {
            GameEvents.OnStickerPlaceAttempt -= OnStickerPlaceAttempt;
            GameEvents.OnPerfectPlacement -= OnPerfectPlacement;
        }
        
        /// <summary>
        /// Calculate and award score for a sticker placement
        /// </summary>
        public int CalculateScore(bool perfect)
        {
            int score = BASE_SCORE_PER_STICKER;
            
            if (perfect)
            {
                score += PERFECT_BONUS;
                
                // Apply combo multiplier
                int multiplier = GetComboMultiplier();
                score *= multiplier;
            }
            
            return score;
        }
        
        /// <summary>
        /// Get current combo multiplier
        /// </summary>
        private int GetComboMultiplier()
        {
            if (currentCombo >= 5) return COMBO_5_MULTIPLIER;
            if (currentCombo >= 3) return COMBO_3_MULTIPLIER;
            return 1;
        }
        
        /// <summary>
        /// Handle sticker placement attempt
        /// </summary>
        private void OnStickerPlaceAttempt(bool perfect, bool collided)
        {
            if (collided)
            {
                // Reset combo on collision
                ResetCombo();
                return;
            }
            
            if (perfect)
            {
                OnPerfectPlacement();
            }
            else
            {
                // Not perfect - reset combo
                ResetCombo();
            }
        }
        
        /// <summary>
        /// Handle perfect placement
        /// </summary>
        private void OnPerfectPlacement()
        {
            currentCombo++;
            perfectCount++;
            
            if (currentCombo > maxCombo)
            {
                maxCombo = currentCombo;
            }
            
            GameEvents.InvokeComboChanged(currentCombo);
            
            // Trigger special effects for high combos
            if (currentCombo == 3)
            {
                Debug.Log("Combo x2 activated!");
            }
            else if (currentCombo == 5)
            {
                Debug.Log("Combo x3 activated! Special animation!");
            }
        }
        
        /// <summary>
        /// Reset combo counter
        /// </summary>
        private void ResetCombo()
        {
            if (currentCombo > 0)
            {
                currentCombo = 0;
                GameEvents.InvokeComboChanged(0);
            }
        }
        
        /// <summary>
        /// Reset all scoring data
        /// </summary>
        public void Reset()
        {
            currentCombo = 0;
            maxCombo = 0;
            perfectCount = 0;
        }
    }
}

