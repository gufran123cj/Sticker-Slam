using System;
using UnityEngine;

namespace StickerSlam.Services
{
    /// <summary>
    /// Mock IAP service for MVP. Simulates purchase behavior without actual SDK.
    /// </summary>
    public class MockIAPService : MonoBehaviour, IIAPService
    {
        [Header("Settings")]
        [SerializeField] private float mockPurchaseTime = 0.5f;
        
        private const string REMOVE_ADS_KEY = "RemoveAdsPurchased";
        
        public void PurchaseGems(int gemAmount, Action<bool> onComplete)
        {
            Debug.Log($"MockIAPService: Purchasing {gemAmount} gems");
            
            // Simulate purchase
            StartCoroutine(SimulatePurchase(() =>
            {
                // In real implementation, would add gems to save data
                Debug.Log($"MockIAPService: {gemAmount} gems purchased (mock)");
                onComplete?.Invoke(true);
            }));
        }
        
        public void PurchaseRemoveAds(Action<bool> onComplete)
        {
            Debug.Log("MockIAPService: Purchasing Remove Ads");
            
            // Simulate purchase
            StartCoroutine(SimulatePurchase(() =>
            {
                PlayerPrefs.SetInt(REMOVE_ADS_KEY, 1);
                PlayerPrefs.Save();
                Debug.Log("MockIAPService: Remove Ads purchased (mock)");
                onComplete?.Invoke(true);
            }));
        }
        
        public bool IsRemoveAdsPurchased()
        {
            return PlayerPrefs.GetInt(REMOVE_ADS_KEY, 0) == 1;
        }
        
        private System.Collections.IEnumerator SimulatePurchase(Action onComplete)
        {
            yield return new WaitForSeconds(mockPurchaseTime);
            onComplete?.Invoke();
        }
    }
}

