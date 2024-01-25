using PimpochkaGames.CoreLibrary;
using PimpochkaGames.NeutralAgeScreen;

namespace PimpochkaGames.AppBuilder
{
    public enum AdvertisingSourceType
    {
        IronSource,
        Debug
    }

    [System.Serializable]
    public class AdvertisementModuleConfig : ConfigPropertyGroup
    {
        public AdvertisingSourceType AdvertisingSourceType = AdvertisingSourceType.IronSource;
        public bool SuccessfulRewardResetInterstitial = true;
        public AdvertisementDebug AdvertisementDebugPrefab;

        public bool NeutralAgeScreen = true;
        public NeutralAgeScreenManager NeutralAgeScreenManagerPrefab;

        public AdvertisementModuleConfig(bool isEnabled = true) : base(Constants.NativeFeatureType.ADVERTISEMENT, isEnabled)
        {
        }
    }
}