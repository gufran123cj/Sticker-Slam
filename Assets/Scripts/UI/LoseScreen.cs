using UnityEngine;
using TMPro;
using StickerSlam.Core;

namespace StickerSlam.UI
{
    /// <summary>
    /// Lose screen shown when player runs out of lives.
    /// </summary>
    public class LoseScreen : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject losePanel;
        [SerializeField] private TextMeshProUGUI levelText;
        
        [Header("Buttons")]
        [SerializeField] private UnityEngine.UI.Button retryButton;
        [SerializeField] private UnityEngine.UI.Button continueButton; // Rewarded ad button
        
        private int currentLevel = 1;
        private bool continueUsed = false;
        
        private void Start()
        {
            if (losePanel != null)
            {
                losePanel.SetActive(false);
            }
            
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryClicked);
            }
            
            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueClicked);
            }
        }
        
        private void OnEnable()
        {
            GameEvents.OnShowLoseScreen += ShowLoseScreen;
            GameEvents.OnLevelFail += OnLevelFail;
        }
        
        private void OnDisable()
        {
            GameEvents.OnShowLoseScreen -= ShowLoseScreen;
            GameEvents.OnLevelFail -= OnLevelFail;
        }
        
        private void OnLevelFail(int level)
        {
            currentLevel = level;
        }
        
        /// <summary>
        /// Show lose screen
        /// </summary>
        private void ShowLoseScreen()
        {
            if (losePanel != null)
            {
                losePanel.SetActive(true);
            }
            
            // Update UI
            if (levelText != null)
            {
                levelText.text = $"Level {currentLevel} Failed";
            }
            
            // Show/hide continue button based on usage
            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(!continueUsed);
            }
        }
        
        /// <summary>
        /// Retry button clicked - restart current level
        /// </summary>
        private void OnRetryClicked()
        {
            if (losePanel != null)
            {
                losePanel.SetActive(false);
            }
            
            continueUsed = false; // Reset continue usage for new attempt
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RetryLevel();
            }
        }
        
        /// <summary>
        /// Continue button clicked - show rewarded ad to continue
        /// </summary>
        private void OnContinueClicked()
        {
            if (continueUsed)
            {
                Debug.LogWarning("LoseScreen: Continue already used!");
                return;
            }
            
            // Request rewarded ad
            GameEvents.InvokeAdRequested("rewarded_continue");
            
            // Mock: For MVP, just continue immediately
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ContinueWithAd();
                continueUsed = true;
                
                if (continueButton != null)
                {
                    continueButton.gameObject.SetActive(false);
                }
                
                if (losePanel != null)
                {
                    losePanel.SetActive(false);
                }
            }
            
            Debug.Log("LoseScreen: Continue activated (mock)");
        }
    }
}

