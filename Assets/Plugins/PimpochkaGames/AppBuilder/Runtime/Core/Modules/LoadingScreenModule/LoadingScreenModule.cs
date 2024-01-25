#if LOADING_SCREEN_MODULE
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public class LoadingScreenModule : Module<LoadingScreenModule, LoadingScreenModuleConfig>
    {
        private static LoadingScreenView _loadingScreenView;
        private static bool _loading = false;
        private static bool _preloading = false;

        protected override void OnInitialize(LoadingScreenModuleConfig config)
        {
            _loadingScreenView = Instantiate(config.LoadingScreenPrefab, gameObject.transform);
        }

        public static async UniTask SetLoadingProcess(UniTask task, float additionalDelaySeconds)
        {
            await SetLoadingProcess(new[] { task }, additionalDelaySeconds);
        }

        public static async UniTask SetLoadingProcess(UniTask[] tasks, float additionalDelaySeconds)
        {
            if (_loading)
                return;

            _loading = true;
            _preloading = true;

            ActivateManualLoadingProcess(true);
            var process = LoadingProcess();

            await UniTask.WhenAll(tasks);
            await UniTask.Delay(TimeSpan.FromSeconds(additionalDelaySeconds));
            _preloading = false;
            await process;

            DeactivateManualLoadingProcess();
            _loading = false;
        }

        public static void ActivateManualLoadingProcess(bool withSlider)
        {
            _loadingScreenView.Slider.gameObject.SetActive(withSlider);
            _loadingScreenView.gameObject.SetActive(true);
        }

        public static void DeactivateManualLoadingProcess()
        {
            _loadingScreenView.gameObject.SetActive(false);
        }

        private static async UniTask LoadingProcess()
        {
            _loadingScreenView.Slider.value = 0;
            float targetValue = 0.8f;
            float currentValue = 0f;
            float speed = 0.02f;

            while (_preloading)
            {
                if (currentValue < targetValue)
                    SetSliderValue();
                else
                    _loadingScreenView.Slider.value = targetValue;

                await UniTask.NextFrame(PlayerLoopTiming.LastPostLateUpdate);
            }

            targetValue = 1f;
            speed = 1f;
            while (_loading)
            {
                if (currentValue <= targetValue)
                {
                    SetSliderValue();
                }
                else
                {
                    _loadingScreenView.Slider.value = targetValue;
                    break;
                }

                await UniTask.NextFrame(PlayerLoopTiming.LastPostLateUpdate);
            }

            void SetSliderValue()
            {
                currentValue += speed * Time.unscaledDeltaTime;
                _loadingScreenView.Slider.value = currentValue;
            }
        }
    }
}
#endif