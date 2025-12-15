using System;

namespace StickerSlam.Services
{
    /// <summary>
    /// Interface for ad service. Allows mock implementation for MVP.
    /// </summary>
    public interface IAdService
    {
        bool IsInterstitialReady();
        bool IsRewardedReady();
        bool IsBannerReady();
        
        void ShowInterstitial(Action<bool> onComplete);
        void ShowRewarded(string rewardType, Action<bool> onRewarded);
        void ShowBanner();
        void HideBanner();
        
        // Ad availability callbacks
        event Action OnInterstitialReady;
        event Action OnRewardedReady;
    }
}

