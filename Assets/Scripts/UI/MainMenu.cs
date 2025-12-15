using UnityEngine;
using UnityEngine.SceneManagement;
using StickerSlam.Core;

namespace StickerSlam.UI
{
    /// <summary>
    /// Main menu screen with Play, Stickers, Settings buttons.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private UnityEngine.UI.Button playButton;
        [SerializeField] private UnityEngine.UI.Button stickersButton;
        [SerializeField] private UnityEngine.UI.Button settingsButton;
        
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject stickersPanel; // Placeholder
        [SerializeField] private GameObject settingsPanel; // Placeholder
        
        [Header("Ad Placeholder")]
        [SerializeField] private GameObject bannerAdPlaceholder; // Visual placeholder for banner ad
        
        private void Start()
        {
            // Setup button listeners
            if (playButton != null)
            {
                playButton.onClick.AddListener(OnPlayClicked);
            }
            
            if (stickersButton != null)
            {
                stickersButton.onClick.AddListener(OnStickersClicked);
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.AddListener(OnSettingsClicked);
            }
            
            // Show main menu
            ShowMainMenu();
        }
        
        private void OnEnable()
        {
            GameEvents.OnShowMainMenu += ShowMainMenu;
        }
        
        private void OnDisable()
        {
            GameEvents.OnShowMainMenu -= ShowMainMenu;
        }
        
        /// <summary>
        /// Play button clicked - start game
        /// </summary>
        private void OnPlayClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartLevel(1);
            }
            else
            {
                Debug.LogError("MainMenu: GameManager instance not found!");
            }
        }
        
        /// <summary>
        /// Stickers button clicked - show sticker album
        /// </summary>
        private void OnStickersClicked()
        {
            if (stickersPanel != null)
            {
                stickersPanel.SetActive(true);
            }
            
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(false);
            }
            
            Debug.Log("MainMenu: Stickers panel opened (placeholder)");
        }
        
        /// <summary>
        /// Settings button clicked - show settings
        /// </summary>
        private void OnSettingsClicked()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
            
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(false);
            }
            
            Debug.Log("MainMenu: Settings panel opened (placeholder)");
        }
        
        /// <summary>
        /// Show main menu panel
        /// </summary>
        private void ShowMainMenu()
        {
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }
            
            if (stickersPanel != null)
            {
                stickersPanel.SetActive(false);
            }
            
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }
    }
}

