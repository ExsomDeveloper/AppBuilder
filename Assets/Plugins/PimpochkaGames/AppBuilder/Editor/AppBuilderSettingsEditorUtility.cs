using System;
using UnityEditor;
using PimpochkaGames.AppBuilder;
using PimpochkaGames.AppBuilder.Editor;

namespace PimpochkaGames.CoreLibrary.Editor
{
    [InitializeOnLoad]
    public static class AppBuilderSettingsEditorUtility
    {
        private static AppBuilderConfig _defaultSettings;

        public static AppBuilderConfig DefaultSettings
        {
            get
            {
                if (_defaultSettings == null)
                {
                    var instance = LoadDefaultSettingsObject(throwError: false);
                    if (null == instance)
                    {
                        instance = CreateDefaultSettingsObject();
                    }
                    _defaultSettings = instance;
                }
                return _defaultSettings;
            }
            set
            {
                if (value == null)
                {
                    throw new Exception(string.Format("Arg {0} is null.", nameof(value)));
                }

                _defaultSettings = value;
            }
        }

        static AppBuilderSettingsEditorUtility()
        {
        }

        private static AppBuilderConfig CreateDefaultSettingsObject()
        {
            return AssetDatabaseUtility.CreateScriptableObject<AppBuilderConfig>(
                assetPath: AppBuilderConfig.DefaultSettingsAssetPath,
                onInit: (instance) => SetDefaultProperties(instance));
        }

        private static AppBuilderConfig LoadDefaultSettingsObject(bool throwError = true)
        {
            return AssetDatabaseUtility.LoadScriptableObject<AppBuilderConfig>(
                assetPath: AppBuilderConfig.DefaultSettingsAssetPath,
                onLoad: (instance) => SetDefaultProperties(instance),
                throwErrorFunc: null);
        }

        private static void SetDefaultProperties(AppBuilderConfig settings)
        {
            if (settings.AdvertisementModuleConfig.AdvertisementDebugPrefab == null)
            {
                settings.AdvertisementModuleConfig.AdvertisementDebugPrefab = UnityAppBuilderUIEditorUtility.LoadAdvertisementDebugPrefab();
            }
        }
    }
}
