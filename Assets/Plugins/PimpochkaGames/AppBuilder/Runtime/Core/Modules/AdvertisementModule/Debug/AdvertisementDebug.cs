using System;
using UnityEngine;
using UnityEngine.UI;

namespace PimpochkaGames.AppBuilder
{
    public class AdvertisementDebug : MonoBehaviour, IAdvertisingSource
    {
        [Header("Interstitial")]
        [SerializeField] private GameObject _interstitialPanel;
        [SerializeField] private Button _interstitialCloseButton;

        [Header("Reward")]
        [SerializeField] private GameObject _rewardPanel;
        [SerializeField] private Button _rewardCloseButton;
        [SerializeField] private Button _rewardGetRewardButton;

        private DateTime _lastClickTime;
        private int _clickCount = 0;
        private bool _testAdDisabled = false;

        public void Initialize()
        {
            _lastClickTime = DateTime.Now;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var delta = DateTime.Now.Subtract(_lastClickTime);
                if (delta.Seconds < 1)
                    _clickCount++;
                else
                    _clickCount = 1;

                _lastClickTime = DateTime.Now;
                if (_clickCount >= 6)
                {
                    _clickCount = 0;
                    _testAdDisabled = !_testAdDisabled;
                    UnityEngine.Debug.LogWarning($"[AdvertisementDebug] Advertisement disabled - {_testAdDisabled}");
                }
            }
        }

        public void ShowInterstitial(Action<AdvertisementStatus> callbackStatus, string placementName = "")
        {
            UnityEngine.Debug.Log("Interstitial");
            if (_testAdDisabled)
            {
                callbackStatus?.Invoke(AdvertisementStatus.Completed);
                return;
            }

            _interstitialPanel.gameObject.SetActive(true);
            _interstitialCloseButton.onClick.AddListener(() =>
            {
                _interstitialPanel.gameObject.SetActive(false);
                callbackStatus?.Invoke(AdvertisementStatus.Completed);
                _interstitialCloseButton.onClick.RemoveAllListeners();
            });
        }

        public void ShowReward(Action<AdvertisementStatus> callbackStatus, string placementName = "")
        {
            UnityEngine.Debug.Log("Reward");
            if (_testAdDisabled)
            {
                callbackStatus?.Invoke(AdvertisementStatus.Completed);
                return;
            }

            _rewardPanel.gameObject.SetActive(true);
            _rewardCloseButton.onClick.AddListener(() =>
            {
                _rewardPanel.gameObject.SetActive(false);
                ClearRewardButtons();
                callbackStatus?.Invoke(AdvertisementStatus.Failed);
            });

            _rewardGetRewardButton.onClick.AddListener(() =>
            {
                _rewardPanel.gameObject.SetActive(false);
                ClearRewardButtons();
                callbackStatus?.Invoke(AdvertisementStatus.Completed);
            });
        }
        private void ClearRewardButtons()
        {
            _rewardCloseButton.onClick.RemoveAllListeners();
            _rewardGetRewardButton.onClick.RemoveAllListeners();
        }

        public void ShowBanner()
        {
            if (_testAdDisabled)
                return;

            Debug.LogWarning("Test banner not implemented!");
        }

        public void HideBanner()
        {
            if (_testAdDisabled)
                return;

            Debug.LogWarning("Test banner not implemented!");
        }

        public void DestroyBanner()
        {
            if (_testAdDisabled)
                return;

            Debug.LogWarning("Test banner not implemented!");
        }

        public void EnableYoungMode() {}
        public void DisableYoungMode() {}
    }
}
