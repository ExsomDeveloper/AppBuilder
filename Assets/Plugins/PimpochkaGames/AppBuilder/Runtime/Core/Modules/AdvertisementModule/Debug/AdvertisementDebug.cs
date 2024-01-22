using Cysharp.Threading.Tasks;
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

        public void Initialize() { }

        public async void ShowInterstitialPanel()
        {
            await _rewardCloseButton.OnClickAsync();
        }

        public void ShowInterstitial(Action<AdvertisementStatus> callbackStatus, string placementName = "")
        {
            UnityEngine.Debug.Log("Interstitial");
            _interstitialPanel.gameObject.SetActive(true);
            _interstitialCloseButton.onClick.AddListener(() =>
            {
                UnityEngine.Debug.Log("Interstitial completed!");
                _interstitialPanel.gameObject.SetActive(false);
                callbackStatus?.Invoke(AdvertisementStatus.Completed);
                _interstitialCloseButton.onClick.RemoveAllListeners();
            });
        }

        public void ShowReward(Action<AdvertisementStatus> callbackStatus, string placementName = "")
        {
            UnityEngine.Debug.Log("Reward");
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
            Debug.LogWarning("Test banner not implemented!");
        }

        public void HideBanner()
        {
            Debug.LogWarning("Test banner not implemented!");
        }

        public void DestroyBanner()
        {
            Debug.LogWarning("Test banner not implemented!");
        }

        public void EnableYoungMode() {}
        public void DisableYoungMode() {}
    }
}
