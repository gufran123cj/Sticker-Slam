using System;

namespace StickerSlam.Services
{
    /// <summary>
    /// Interface for In-App Purchase service. Allows mock implementation for MVP.
    /// </summary>
    public interface IIAPService
    {
        void PurchaseGems(int gemAmount, Action<bool> onComplete);
        void PurchaseRemoveAds(Action<bool> onComplete);
        bool IsRemoveAdsPurchased();
    }
}

