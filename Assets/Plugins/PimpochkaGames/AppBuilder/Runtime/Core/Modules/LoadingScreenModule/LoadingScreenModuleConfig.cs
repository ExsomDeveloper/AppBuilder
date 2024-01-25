using PimpochkaGames.CoreLibrary;

namespace PimpochkaGames.AppBuilder
{
    [System.Serializable]
    public class LoadingScreenModuleConfig : ConfigPropertyGroup
    {
        public LoadingScreenView LoadingScreenPrefab;

        public LoadingScreenModuleConfig(bool isEnabled = true) : base(Constants.NativeFeatureType.LOADING_SCREEN, isEnabled)
        {
        }
    }
}