#if ADVERTISEMENT_MODULE
using System;
using UnityEngine;

namespace AppBuilder
{
    public class AdvertisementModule : Module<AdvertisementModule, AdvertisementModuleConfig>
    {
        private const string ADVERTISEMENT_DISABLED_KEY = "advertisement_disabled";
        private static AdvertisementTimer _timer;
        private static IAdvertisingSource _advertisingSource;
        private static bool _advertisementDisabled = false;

        protected override void OnInitialize(AdvertisementModuleConfig config)
        {
            _advertisementDisabled = PlayerPrefs.HasKey(ADVERTISEMENT_DISABLED_KEY);
            _timer = new AdvertisementTimer(this);

            UnityEngine.Debug.Log("[Test]: AdvertisementModule init");
            _advertisingSource = GetAdvertisementSource(config);
            _advertisingSource.Initialize();
        }

        public static void StartInterstitialTimer(int seconds, bool loop, ITimerHandler timerHandler = null)
        {
            if (timerHandler == null)
                timerHandler = new InterstitialTimerShowHandler();

            _timer.Start(seconds, loop, timerHandler);
        }

        public static void DisableAdvertisement()
        {
            _advertisementDisabled = false;
            DestroyBanner();
            PlayerPrefs.SetInt(ADVERTISEMENT_DISABLED_KEY, 1);
        }

        public static void PauseInterstitialTimer() => _timer.Pause();
        public static void UnpauseInterstitialTimer() => _timer.Unpause();
        public static void ResetInterstitialTimer() => _timer.Reset();
        public static void StopInterstitialTimer() => _timer.Stop();

        public static void ShowBanner()
        {
            if (_advertisementDisabled)
                return;

            _advertisingSource.ShowBanner();
        }

        public static void HideBanner()
        {
            if (_advertisementDisabled)
                return;

            _advertisingSource.HideBanner();
        }

        public static void DestroyBanner()
        {
            if (_advertisementDisabled)
                return;

            _advertisingSource.DestroyBanner();
        }

        public static void ShowInterstitial(Action<AdvertisementStatus> callbackStatus, string placementName = "")
        {
            if (_advertisementDisabled)
            {
                callbackStatus?.Invoke(AdvertisementStatus.Disabled);
                return;
            }

            _advertisingSource.ShowInterstitial(callbackStatus, placementName);
        }

        public static void ShowReward(Action<AdvertisementStatus> callbackStatus, string placementName = "", bool resetInterstitialTimer = true)
        {
            callbackStatus += (status) =>
            {
                UnityEngine.Debug.Log("[Test] Reward callback " + status);
                if (status != AdvertisementStatus.Completed)
                {
                    _timer.Unpause();
                }
                else if (resetInterstitialTimer)
                {
                    _timer.Reset();
                }
                else
                {
                    _timer.Unpause();
                }
            };

            _timer.Pause();
            _advertisingSource.ShowReward(callbackStatus, placementName);
        }

        private IAdvertisingSource GetAdvertisementSource(AdvertisementModuleConfig config)
        {
#if UNITY_EDITOR
            return Instantiate(config.AdvertisementDebugPrefab, gameObject.transform);
#endif

            return config.AdvertisingSourceType switch
            {
                AdvertisingSourceType.IronSource => new IronSourceAdapter(),

                //TODO define
                _ => Instantiate(config.AdvertisementDebugPrefab, gameObject.transform)
            };
        }
    }
}
#endif

public enum AdvertisingSourceType
{
    IronSource,
    Debug
}

namespace AppBuilder
{
    [System.Serializable]
    public class AdvertisementModuleConfig : ModuleConfig
    {
        public AdvertisingSourceType AdvertisingSourceType = AdvertisingSourceType.IronSource;
        public bool SuccessfulRewardResetInterstitial = true;

        public AdvertisementDebug AdvertisementDebugPrefab;
    }
}