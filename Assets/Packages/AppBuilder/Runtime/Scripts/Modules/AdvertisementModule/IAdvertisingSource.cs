using System;

namespace PimpochkaGames.AppBuilder
{
    public interface IAdvertisingSource
    {
        public void Initialize();
        public void ShowInterstitial(Action<AdvertisementStatus> callbackStatus, string placementName = "");
        public void ShowReward(Action<AdvertisementStatus> callbackStatus, string placementName = "");
        public void ShowBanner();
        public void HideBanner();
        public void DestroyBanner();
    }
}