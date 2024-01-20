using UnityEngine;

namespace AppBuilder
{
    [CreateAssetMenu(fileName = nameof(AppBuilderConfig), menuName = "AppCore/Config")]
    public class AppBuilderConfig : ScriptableObject
    {
#if UNITY_EDITOR
        public bool LoadingScreenModuleEnabled = false;
        public bool AdvertisementModuleEnabled = false;
        public bool NeutralAgeScreenModuleEnabled = false;
#endif
        public string NextSceneName = string.Empty;

        [SerializeField] private LoadingScreenModuleConfig _loadingScreenConfig;
        [SerializeField] private AdvertisementModuleConfig _advertisementModuleConfig;
        [SerializeField] private NeutralAgeScreenConfig _neutralAgeScreenConfig;

        public LoadingScreenModuleConfig LoadingScreenConfig => _loadingScreenConfig;
        public AdvertisementModuleConfig AdvertisementModuleConfig => _advertisementModuleConfig;
        public NeutralAgeScreenConfig NeutralAgeScreenConfig => _neutralAgeScreenConfig;
    }
}