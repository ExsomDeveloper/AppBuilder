using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    [CreateAssetMenu(fileName = nameof(AppBuilderConfig), menuName = "AppCore/Config")]
    public class AppBuilderConfig : ScriptableObject
    {
#if UNITY_EDITOR
        public bool LoadingScreenModuleEnabled = false;
        public bool AdvertisementModuleEnabled = false;
#endif
        public string NextSceneName = string.Empty;

        [SerializeField] private LoadingScreenModuleConfig _loadingScreenConfig;
        [SerializeField] private AdvertisementModuleConfig _advertisementModuleConfig;

        public LoadingScreenModuleConfig LoadingScreenConfig => _loadingScreenConfig;
        public AdvertisementModuleConfig AdvertisementModuleConfig => _advertisementModuleConfig;
    }
}