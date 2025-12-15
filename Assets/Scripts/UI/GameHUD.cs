using UnityEngine;
using TMPro;
using StickerSlam.Core;

namespace StickerSlam.UI
{
    /// <summary>
    /// In-game HUD showing level, lives, score, and combo indicators.
    /// </summary>
    public class GameHUD : MonoBehaviour
    {
        [Header("Level Info")]
        [SerializeField] private TextMeshProUGUI levelText;
        
        [Header("Lives")]
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private GameObject[] lifeIcons; // Optional: visual life indicators
        
        [Header("Score")]
        [SerializeField] private TextMeshProUGUI scoreText;
        
        [Header("Progress")]
        [SerializeField] private TextMeshProUGUI stickersProgressText; // "3/8"
        
        [Header("Perfect/Combo")]
        [SerializeField] private GameObject perfectIndicator;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private GameObject comboIndicator;
        
        private void OnEnable()
        {
            GameEvents.OnLevelStart += OnLevelStart;
            GameEvents.OnLivesChanged += OnLivesChanged;
            GameEvents.OnScoreChanged += OnScoreChanged;
            GameEvents.OnStickersPlaced += OnStickersPlaced;
            GameEvents.OnStickersToPlaceChanged += OnStickersToPlaceChanged;
            GameEvents.OnPerfectPlacement += OnPerfectPlacement;
            GameEvents.OnComboChanged += OnComboChanged;
        }
        
        private void OnDisable()
        {
            GameEvents.OnLevelStart -= OnLevelStart;
            GameEvents.OnLivesChanged -= OnLivesChanged;
            GameEvents.OnScoreChanged -= OnScoreChanged;
            GameEvents.OnStickersPlaced -= OnStickersPlaced;
            GameEvents.OnStickersToPlaceChanged -= OnStickersToPlaceChanged;
            GameEvents.OnPerfectPlacement -= OnPerfectPlacement;
            GameEvents.OnComboChanged -= OnComboChanged;
        }
        
        private void OnLevelStart(int level)
        {
            if (levelText != null)
            {
                levelText.text = $"Level {level}";
            }
            
            UpdateStickersProgress();
        }
        
        private void OnLivesChanged(int lives)
        {
            if (livesText != null)
            {
                livesText.text = $"Lives: {lives}";
            }
            
            // Update life icons if available
            if (lifeIcons != null)
            {
                for (int i = 0; i < lifeIcons.Length; i++)
                {
                    if (lifeIcons[i] != null)
                    {
                        lifeIcons[i].SetActive(i < lives);
                    }
                }
            }
        }
        
        private void OnScoreChanged(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score:N0}";
            }
        }
        
        private void OnStickersPlaced(int count)
        {
            UpdateStickersProgress();
        }
        
        private void OnStickersToPlaceChanged(int target)
        {
            UpdateStickersProgress();
        }
        
        private void UpdateStickersProgress()
        {
            if (stickersProgressText != null && GameManager.Instance != null)
            {
                int placed = GameManager.Instance.StickersPlaced;
                int target = GameManager.Instance.StickersToPlace;
                stickersProgressText.text = $"{placed}/{target}";
            }
        }
        
        private void OnPerfectPlacement()
        {
            if (perfectIndicator != null)
            {
                perfectIndicator.SetActive(true);
                // Auto-hide after 1 second
                Invoke(nameof(HidePerfectIndicator), 1f);
            }
        }
        
        private void HidePerfectIndicator()
        {
            if (perfectIndicator != null)
            {
                perfectIndicator.SetActive(false);
            }
        }
        
        private void OnComboChanged(int combo)
        {
            if (combo > 0)
            {
                if (comboText != null)
                {
                    comboText.text = $"COMBO x{combo}";
                }
                
                if (comboIndicator != null)
                {
                    comboIndicator.SetActive(true);
                }
            }
            else
            {
                if (comboIndicator != null)
                {
                    comboIndicator.SetActive(false);
                }
            }
        }
    }
}

