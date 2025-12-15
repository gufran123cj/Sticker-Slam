using UnityEngine;
using TMPro;
using StickerSlam.Core;

namespace StickerSlam.UI
{
    /// <summary>
    /// Win screen shown when level is completed.
    /// </summary>
    public class WinScreen : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI levelText;
        
        [Header("Buttons")]
        [SerializeField] private UnityEngine.UI.Button nextButton;
        [SerializeField] private UnityEngine.UI.Button doubleRewardButton; // Rewarded ad button
        
        private int currentLevel = 1;
        private int currentScore = 0;
        
        private void Start()
        {
            if (winPanel != null)
            {
                winPanel.SetActive(false);
            }
            
            if (nextButton != null)
            {
                nextButton.onClick.AddListener(OnNextClicked);
            }
            
            if (doubleRewardButton != null)
            {
                doubleRewardButton.onClick.AddListener(OnDoubleRewardClicked);
            }
        }
        
        private void OnEnable()
        {
            GameEvents.OnShowWinScreen += ShowWinScreen;
            GameEvents.OnLevelComplete += OnLevelComplete;
        }
        
        private void OnDisable()
        {
            GameEvents.OnShowWinScreen -= ShowWinScreen;
            GameEvents.OnLevelComplete -= OnLevelComplete;
        }
        
        private void OnLevelComplete(int level, int score)
        {
            currentLevel = level;
            currentScore = score;
        }
        
        /// <summary>
        /// Show win screen
        /// </summary>
        private void ShowWinScreen()
        {
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
            
            // Update UI
            if (scoreText != null)
            {
                scoreText.text = $"Score: {currentScore:N0}";
            }
            
            if (levelText != null)
            {
                levelText.text = $"Level {currentLevel} Complete!";
            }
        }
        
        /// <summary>
        /// Next button clicked - continue to next level
        /// </summary>
        private void OnNextClicked()
        {
            if (winPanel != null)
            {
                winPanel.SetActive(false);
            }
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NextLevel();
            }
        }
        
        /// <summary>
        /// Double reward button clicked - show rewarded ad
        /// </summary>
        private void OnDoubleRewardClicked()
        {
            // Request rewarded ad
            GameEvents.InvokeAdRequested("rewarded_double_reward");
            
            // Mock: For MVP, just double the score immediately
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(currentScore);
            }
            
            Debug.Log("WinScreen: Double reward activated (mock)");
        }
    }
}

