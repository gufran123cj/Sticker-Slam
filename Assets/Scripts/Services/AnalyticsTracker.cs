using System.Collections.Generic;
using UnityEngine;
using StickerSlam.Core;

namespace StickerSlam.Services
{
    /// <summary>
    /// Analytics tracker. Logs events to Debug for MVP, stores last 50 events in memory.
    /// </summary>
    public class AnalyticsTracker : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int maxStoredEvents = 50;
        
        private Queue<AnalyticsEvent> eventHistory = new Queue<AnalyticsEvent>();
        
        private void OnEnable()
        {
            // Subscribe to game events
            GameEvents.OnLevelStart += OnLevelStart;
            GameEvents.OnLevelComplete += OnLevelComplete;
            GameEvents.OnLevelFail += OnLevelFail;
            GameEvents.OnStickerPlaceAttempt += OnStickerPlaceAttempt;
            GameEvents.OnAdShown += OnAdShown;
        }
        
        private void OnDisable()
        {
            GameEvents.OnLevelStart -= OnLevelStart;
            GameEvents.OnLevelComplete -= OnLevelComplete;
            GameEvents.OnLevelFail -= OnLevelFail;
            GameEvents.OnStickerPlaceAttempt -= OnStickerPlaceAttempt;
            GameEvents.OnAdShown -= OnAdShown;
        }
        
        /// <summary>
        /// Track level start
        /// </summary>
        public void TrackLevelStart(int level, string surfaceType, bool isBoss)
        {
            LogEvent("level_start", new Dictionary<string, object>
            {
                { "level", level },
                { "surface_type", surfaceType },
                { "is_boss", isBoss }
            });
        }
        
        /// <summary>
        /// Track level complete
        /// </summary>
        public void TrackLevelComplete(int level, int score, int perfectCount, int comboMax)
        {
            LogEvent("level_complete", new Dictionary<string, object>
            {
                { "level", level },
                { "score", score },
                { "perfect_count", perfectCount },
                { "combo_max", comboMax }
            });
        }
        
        /// <summary>
        /// Track level fail
        /// </summary>
        public void TrackLevelFail(int level, int failCount)
        {
            LogEvent("level_fail", new Dictionary<string, object>
            {
                { "level", level },
                { "fail_count", failCount }
            });
        }
        
        /// <summary>
        /// Track sticker placement
        /// </summary>
        public void TrackStickerPlace(bool perfect, bool collided)
        {
            LogEvent("sticker_place", new Dictionary<string, object>
            {
                { "perfect", perfect },
                { "collided", collided }
            });
        }
        
        /// <summary>
        /// Track ad impression
        /// </summary>
        public void TrackAdImpression(string adType)
        {
            LogEvent("ad_impression", new Dictionary<string, object>
            {
                { "ad_type", adType }
            });
        }
        
        /// <summary>
        /// Log event to console and store in history
        /// </summary>
        private void LogEvent(string eventName, Dictionary<string, object> parameters)
        {
            AnalyticsEvent evt = new AnalyticsEvent
            {
                eventName = eventName,
                parameters = parameters,
                timestamp = Time.time
            };
            
            // Store in history
            eventHistory.Enqueue(evt);
            if (eventHistory.Count > maxStoredEvents)
            {
                eventHistory.Dequeue();
            }
            
            // Log to console (MVP)
            string paramString = string.Join(", ", System.Linq.Enumerable.Select(parameters, p => $"{p.Key}={p.Value}"));
            Debug.Log($"[Analytics] {eventName}({paramString})");
        }
        
        /// <summary>
        /// Get event history (for debugging)
        /// </summary>
        public List<AnalyticsEvent> GetEventHistory()
        {
            return new List<AnalyticsEvent>(eventHistory);
        }
        
        // Event handlers
        private void OnLevelStart(int level)
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentLevelConfig != null)
            {
                TrackLevelStart(level, GameManager.Instance.CurrentLevelConfig.surfaceType.ToString(), 
                    GameManager.Instance.CurrentLevelConfig.isBoss);
            }
        }
        
        private void OnLevelComplete(int level, int score)
        {
            // Get perfect count and combo from ScoringSystem (would need reference)
            TrackLevelComplete(level, score, 0, 0); // Placeholder
        }
        
        private void OnLevelFail(int level)
        {
            TrackLevelFail(level, 1); // Placeholder fail count
        }
        
        private void OnStickerPlaceAttempt(bool perfect, bool collided)
        {
            TrackStickerPlace(perfect, collided);
        }
        
        private void OnAdShown(string adType)
        {
            TrackAdImpression(adType);
        }
    }
    
    /// <summary>
    /// Analytics event data structure
    /// </summary>
    [System.Serializable]
    public class AnalyticsEvent
    {
        public string eventName;
        public Dictionary<string, object> parameters;
        public float timestamp;
    }
}

