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
            _rewardedAdsStatus?.Invoke(status);
            _rewardedAdsStatus = null;
        }

        private void InterstitialClosedWithStatus(AdvertisementStatus status)
        {
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
                    InitializeCallbacks();
                };

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

        public void EnableYoungMode()
        {
            IronSource.Agent.setMetaData("is_child_directed", "true");
            IronSource.Agent.setMetaData("AdMob_TFCD", "true");
            IronSource.Agent.setMetaData("AdColony_COPPA", "true");
            IronSource.Agent.setMetaData("AppLovin_AgeRestrictedUser", "true");
            IronSource.Agent.setMetaData("InMobi_AgeRestricted", "true");
            IronSource.Agent.setMetaData("Mintegral_COPPA", "true");
            IronSource.Agent.setMetaData("Vungle_coppa", "true"); //LiftoffMonetize (old Vungle)
            IronSource.Agent.setMetaData("Pangle_COPPA", "1");
            IronSource.Agent.setMetaData("UnityAds_coppa", "true");
            IronSource.Agent.setMetaData("AdMob_MaxContentRating", "MAX_AD_CONTENT_RATING_PG"); // - Children with parent. MAX_AD_CONTENT_RATING_PG - only children
            SetupSegment("children", 12);
        }

        public void DisableYoungMode()
        {
            IronSource.Agent.setMetaData("is_child_directed", "false");
            IronSource.Agent.setMetaData("AdMob_TFCD", "false");
            IronSource.Agent.setMetaData("AdColony_COPPA", "false");
            IronSource.Agent.setMetaData("AppLovin_AgeRestrictedUser", "false");
            IronSource.Agent.setMetaData("InMobi_AgeRestricted", "false");
            IronSource.Agent.setMetaData("Mintegral_COPPA", "false");
            IronSource.Agent.setMetaData("Vungle_coppa", "false"); //LiftoffMonetize (old Vungle)
            IronSource.Agent.setMetaData("Pangle_COPPA", "0");
            IronSource.Agent.setMetaData("UnityAds_coppa", "false");
            IronSource.Agent.setMetaData("AdMob_MaxContentRating", "MAX_AD_CONTENT_RATING_MA");
        }

        private void SetupSegment(string segmentName, int userAge)
        {
            IronSourceSegment ironSourceSegment = new IronSourceSegment();
            ironSourceSegment.segmentName = segmentName;
            ironSourceSegment.age = userAge;
            IronSource.Agent.setSegment(ironSourceSegment);
        }


        public void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }
    }
}
#endif