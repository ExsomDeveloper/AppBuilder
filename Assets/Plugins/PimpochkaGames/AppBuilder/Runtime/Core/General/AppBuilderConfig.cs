using PimpochkaGames.CoreLibrary;
using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    [CreateAssetMenu(fileName = nameof(AppBuilderConfig), menuName = "AppCore/Config")]
    public class AppBuilderConfig : ScriptableObject
    {
        //[ClearOnReload]
        private static AppBuilderConfig _instance;
        //[ClearOnReload]
        private static UnityPackageDefinition _package;
        [SerializeField] private LoadingScreenModuleConfig _loadingScreenConfig;
        [SerializeField] private AdvertisementModuleConfig _advertisementModuleConfig;
        [SerializeField] private FirebaseModuleConfig _firebaseModuleConfig;
        [SerializeField] private string _nextSceneName = string.Empty;

        public string NextSceneName { get => _nextSceneName; set => _nextSceneName = value; }
        public LoadingScreenModuleConfig LoadingScreenConfig => _loadingScreenConfig;
        public AdvertisementModuleConfig AdvertisementModuleConfig => _advertisementModuleConfig;
        public FirebaseModuleConfig FirebaseModuleConfig => _firebaseModuleConfig;
        public static AppBuilderConfig Instance => GetSharedInstanceInternal();
        public static string DefaultSettingsAssetName => nameof(AppBuilderConfig);
        public static string PackageName => Package.Name;
        public static string DisplayName => Package.DisplayName;
        public static string Version => Package.Version;
        public static string DefaultSettingsAssetPath => $"{Package.GetMutableResourcesPath()}/{DefaultSettingsAssetName}.asset";
        public static string PersistentDataPath => Package.PersistentDataPath;

        internal static UnityPackageDefinition Package => ObjectHelper.CreateInstanceIfNull(
            ref _package,
            () =>
            {
                return new UnityPackageDefinition(name: "com.pimpochkagames.appbuilder",
                                          displayName: "App Builder",
                                          version: "1.0.0",
                                          defaultInstallPath: $"Assets/Plugins/PimpochkaGames/AppBuilder",
                                          dependencies: CoreLibrarySettings.Package);
            });

        private static AppBuilderConfig GetSharedInstanceInternal(bool throwError = true)
        {
            if (null == _instance)
            {
                // check whether we are accessing in edit or play mode
                var assetPath = DefaultSettingsAssetName;
                var settings = Resources.Load<AppBuilderConfig>(assetPath);
                if (throwError && (null == settings))
                {
                    UnityEngine.Debug.Log("AppBuilder config error!");
                }

                // store reference
                _instance = settings;
            }

            return _instance;
        }
    }
}