#if ADVERTISEMENT_MODULE
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public class IronSourceAdapter : MonoBehaviour, IAdvertisingSource
    {
        private Action<AdvertisementStatus> _rewardedAdsStatus;
        private Action<AdvertisementStatus> _interstitialAdsStatus;
        private bool _initialized = false;
        private bool _initializationInProcess = false;

        public void Initialize()
        {
            if (_initialized)
                throw new Exception("Already initialized");

            if (_initializationInProcess)
                throw new Exception("Initialization in process");

            InitializeSdk().Forget();
        }

        public void ShowInterstitial(Action<AdvertisementStatus> callbackStatus, string placementName = "")
        {
            UnityEngine.Debug.Log("[Test]: ShowInterstitial");
            if (!IronSource.Agent.isInterstitialReady())
                callbackStatus?.Invoke(AdvertisementStatus.NotLoaded);

            _interstitialAdsStatus = callbackStatus;
            IronSource.Agent.showInterstitial(placementName);
        }

        public void ShowReward(Action<AdvertisementStatus> callbackStatus, string placementName = "")
        {
            if (!IronSource.Agent.isRewardedVideoAvailable())
                callbackStatus?.Invoke(AdvertisementStatus.NotLoaded);

            _rewardedAdsStatus = callbackStatus;
            IronSource.Agent.showRewardedVideo(placementName);
        }

        public void ShowBanner()
        {
            IronSource.Agent.displayBanner();
        }

        public void HideBanner()
        {
            IronSource.Agent.hideBanner();
        }

        public void DestroyBanner()
        {
            IronSource.Agent.destroyBanner();
        }

        private void RewardClosedWithStatus(AdvertisementStatus status)
        {
            Debug.Log($"Rewarded end with {status}");
            _rewardedAdsStatus?.Invoke(status);
            _rewardedAdsStatus = null;
        }

        private void InterstitialClosedWithStatus(AdvertisementStatus status)
        {
            Debug.Log($"[Test]: Interstitial end with {status}");
            _interstitialAdsStatus?.Invoke(status);
            _interstitialAdsStatus = null;
            LoadInterstitial();
        }

        private async UniTaskVoid InitializeSdk()
        {
            var developerSettings = await Resources.LoadAsync(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME) as IronSourceMediationSettings;
#if UNITY_ANDROID
            string appKey = developerSettings.AndroidAppKey;
#elif UNITY_IOS
        string appKey = developerSettings.IOSAppKey;
#else
        string appKey = "";
#endif

            if (appKey.Equals(string.Empty))
            {
                Debug.LogWarning("IronSourceInitilizer Cannot init without AppKey");
            }
            else
            {
                IronSourceEvents.onSdkInitializationCompletedEvent += () =>
                {
                    //Debug.Log("Ads_service: IronSource initialized!");
                    UnityEngine.Debug.Log("[Test]: onSdkInitializationCompletedEvent");
                    InitializeCallbacks();
                };

                UnityEngine.Debug.Log("[Test]: init");
                IronSource.Agent.init(appKey);
                IronSource.UNITY_PLUGIN_VERSION = "7.2.1-ri";
            }

            IronSource.Agent.validateIntegration();
        }

        private void InitializeCallbacks()
        {
            IronSourceRewardedVideoEvents.onAdRewardedEvent += (placement, adInfo) => RewardClosedWithStatus(AdvertisementStatus.Completed);
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += (error, info) => RewardClosedWithStatus(AdvertisementStatus.Failed);
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += (error) => RewardClosedWithStatus(AdvertisementStatus.Failed);

            IronSourceInterstitialEvents.onAdClosedEvent += (error) => InterstitialClosedWithStatus(AdvertisementStatus.Completed);
            IronSourceInterstitialEvents.onAdShowFailedEvent += (error, info) => InterstitialClosedWithStatus(AdvertisementStatus.Failed);
            IronSourceInterstitialEvents.onAdLoadFailedEvent += (error) => InterstitialClosedWithStatus(AdvertisementStatus.Failed);
            //IronSourceBannerEvents.onAdLoadedEvent += (arg) => UpdateBannerState();

            IronSource.Agent.loadInterstitial();
            IronSource.Agent.loadRewardedVideo();
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
            IronSource.Agent.hideBanner();
        }

        private void LoadInterstitial()
        {
            if (!IronSource.Agent.isInterstitialReady())
                IronSource.Agent.loadInterstitial();
        }

        public void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }
    }
}
#endif