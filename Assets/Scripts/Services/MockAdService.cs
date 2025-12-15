using System;
using UnityEngine;
using StickerSlam.Core;

namespace StickerSlam.Services
{
    /// <summary>
    /// Mock ad service for MVP. Simulates ad behavior without actual SDK.
    /// </summary>
    public class MockAdService : MonoBehaviour, IAdService
    {
        [Header("Settings")]
        [SerializeField] private float mockLoadTime = 1f; // Simulate ad load time
        
        private bool isInterstitialReady = true;
        private bool isRewardedReady = true;
        private bool isBannerReady = true;
        
        private float lastInterstitialTime = 0f;
        private const float INTERSTITIAL_COOLDOWN = 300f; // 5 minutes
        
        public event Action OnInterstitialReady;
        public event Action OnRewardedReady;
        
        public bool IsInterstitialReady()
        {
            // Check cooldown (max 1 per 5 minutes)
            float timeSinceLastAd = Time.time - lastInterstitialTime;
            return isInterstitialReady && timeSinceLastAd >= INTERSTITIAL_COOLDOWN;
        }
        
        public bool IsRewardedReady()
        {
            return isRewardedReady;
        }
        
        public bool IsBannerReady()
        {
            return isBannerReady;
        }
        
        public void ShowInterstitial(Action<bool> onComplete)
        {
            if (!IsInterstitialReady())
            {
                onComplete?.Invoke(false);
                return;
            }
            
            Debug.Log("MockAdService: Showing Interstitial Ad");
            GameEvents.InvokeAdRequested("interstitial");
            
            // Simulate ad display
            StartCoroutine(SimulateAdDisplay(() =>
            {
                lastInterstitialTime = Time.time;
                GameEvents.InvokeAdShown("interstitial");
                onComplete?.Invoke(true);
            }));
        }
        
        public void ShowRewarded(string rewardType, Action<bool> onRewarded)
        {
            if (!IsRewardedReady())
            {
                onRewarded?.Invoke(false);
                return;
            }
            
            Debug.Log($"MockAdService: Showing Rewarded Ad - {rewardType}");
            GameEvents.InvokeAdRequested($"rewarded_{rewardType}");
            
            // Simulate ad display
            StartCoroutine(SimulateAdDisplay(() =>
            {
                GameEvents.InvokeAdShown($"rewarded_{rewardType}");
                GameEvents.InvokeAdRewarded(rewardType);
                onRewarded?.Invoke(true);
            }));
        }
        
        public void ShowBanner()
        {
            if (!IsBannerReady())
            {
                Debug.LogWarning("MockAdService: Banner not ready");
                return;
            }
            
            Debug.Log("MockAdService: Showing Banner Ad");
            GameEvents.InvokeAdRequested("banner");
            GameEvents.InvokeAdShown("banner");
        }
        
        public void HideBanner()
        {
            Debug.Log("MockAdService: Hiding Banner Ad");
        }
        
        private System.Collections.IEnumerator SimulateAdDisplay(Action onComplete)
        {
            yield return new WaitForSeconds(mockLoadTime);
            onComplete?.Invoke();
        }
    }
}

