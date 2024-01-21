using PimpochkaGames.AppBuilder;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    [SerializeField] private Button _showReward;
    [SerializeField] private Button _showInterstitial;
    [SerializeField] private Button _startInterstitialTimer;
    [SerializeField] private Button _stopInterstitialTimer;
    [SerializeField] private Button _pauseInterstitialTimer;
    [SerializeField] private Button _unpauseInterstitialTimer;
    [SerializeField] private Button _showBanner;
    [SerializeField] private Button _hideBanner;

    private void Awake()
    {
        _showReward.onClick.AddListener(() => AdvertisementModule.ShowReward(null));
        _showInterstitial.onClick.AddListener(() => AdvertisementModule.ShowInterstitial(null));

        _showBanner.onClick.AddListener(() => AdvertisementModule.ShowBanner());
        _hideBanner.onClick.AddListener(() => AdvertisementModule.HideBanner());

        _startInterstitialTimer.onClick.AddListener(() => AdvertisementModule.StartInterstitialTimer(20, true));
        _stopInterstitialTimer.onClick.AddListener(() => AdvertisementModule.StopInterstitialTimer());
        _pauseInterstitialTimer.onClick.AddListener(() => AdvertisementModule.PauseInterstitialTimer());
        _unpauseInterstitialTimer.onClick.AddListener(() => AdvertisementModule.UnpauseInterstitialTimer());
    }
}
