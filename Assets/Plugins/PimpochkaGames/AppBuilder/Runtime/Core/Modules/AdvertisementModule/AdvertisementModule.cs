#if ADVERTISEMENT_MODULE
using PimpochkaGames.NeutralAgeScreen;
using System;
using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public class AdvertisementModule : Module<AdvertisementModule, AdvertisementModuleConfig>
    {
        private const string ADVERTISEMENT_DISABLED_KEY = "advertisement_disabled";
        public const int MAX_CHILDREN_AGE = 12;
        private static AdvertisementTimer _timer;
        private static IAdvertisingSource _advertisingSource;
        private static bool _advertisementDisabled = false;

        protected override void OnInitialize(AdvertisementModuleConfig config)
        {
            _advertisementDisabled = PlayerPrefs.HasKey(ADVERTISEMENT_DISABLED_KEY);
            _timer = new AdvertisementTimer(this);

            _advertisingSource = GetAdvertisementSource(config);

            if (config.NeutralAgeScreen)
            {
                if (NeutralAgeScreenManager.TryGetUserAge(out int age))
                {
                    SetupAgeMode(age);
                    _advertisingSource.Initialize();
                }
                else
                {
                    NeutralAgeScreenManager.OnApplyUserAge += (selectedAge) =>
                    {
                        SetupAgeMode(selectedAge);
                        _advertisingSource.Initialize();
                    };

                    SpawnNeutralAgeScreenManager(config);
                }
            }
            else
            {
                _advertisingSource.Initialize();
            }

            void SetupAgeMode(int age)
            {
                if (age <= MAX_CHILDREN_AGE)
                    _advertisingSource.EnableYoungMode();
                else
                    _advertisingSource.DisableYoungMode();
            }
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

#pragma warning disable CS0162
            return config.AdvertisingSourceType switch
            {
                AdvertisingSourceType.IronSource => new IronSourceAdapter(),

                //TODO define
                _ => Instantiate(config.AdvertisementDebugPrefab, gameObject.transform)
            };
#pragma warning restore CS0162
        }

        private void SpawnNeutralAgeScreenManager(AdvertisementModuleConfig config)
        {
            Instantiate(config.NeutralAgeScreenManagerPrefab, gameObject.transform);
        }
    }
}
#endif