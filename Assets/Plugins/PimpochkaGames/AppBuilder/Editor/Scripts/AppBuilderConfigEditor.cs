using UnityEditor;
using UnityEngine;
using PimpochkaGames.NeutralAgeScreen;
using PimpochkaGames.CoreLibrary.Editor;

namespace PimpochkaGames.AppBuilder.Editor
{
    [CustomEditor(typeof(AppBuilderConfig))]
    public class AppBuilderConfigEditor : UnityEditor.Editor
    {
        private string _buildPath = string.Empty;

        public override void OnInspectorGUI()
        {
            AppBuilderConfig config = (target as AppBuilderConfig);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            DrawGeneral(config);
            EditorGUILayout.Space(10);
            //DrawAppSettings();
            EditorGUILayout.Space(10);
            DrawLoadingScreenModuleInspector(config);
            EditorGUILayout.Space(10);
            DrawAdvertisementModuleInspector(config);

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(config);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGeneral(AppBuilderConfig config)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("General settings", GetTitleStyle());
                config.NextSceneName = EditorGUILayout.TextField("First scene", config.NextSceneName);
            }
        }

        private void DrawAppSettings()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.BeginHorizontal();
                using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true))) //EditorStyles.toolbar
                {
                    using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
                    {
                        GUILayout.Label("Build settings", GetTitleStyle());
                        GUILayout.Label($"<color=orange>{EditorUserBuildSettings.activeBuildTarget}</color>", new GUIStyle(EditorStyles.label) { fontSize = 10, richText = true });
                        EditorGUILayout.Space(5);
                        PlayerSettings.bundleVersion = EditorGUILayout.TextField("Application version", PlayerSettings.bundleVersion);
                        PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField("Bundle number", PlayerSettings.Android.bundleVersionCode);
                        EditorUserBuildSettings.development = EditorGUILayout.Toggle("Development Build", EditorUserBuildSettings.development);

                        EditorGUILayout.Space(20);
                        EditorGUILayout.LabelField("Build path");
                        GUILayout.BeginHorizontal();
                        _buildPath = GUILayout.TextField(_buildPath, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                        if (GUILayout.Button("Browser", GUILayout.MaxWidth(60)))
                            _buildPath = EditorUtility.OpenFolderPanel("Choose build path", "", "");
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Build"))
                        {
                            AssetDatabase.Refresh();
                            //Debug.Log("Build " + _versionControl.GetAppVersion());
                        }

                        if (GUILayout.Button("Build and run"))
                        {
                            AssetDatabase.Refresh();
                            Debug.Log("Build and run");
                        }

                        GUILayout.EndHorizontal();
                    }
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawAdvertisementModuleInspector(AppBuilderConfig config)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Advertisement module", GetTitleStyle(), GUILayout.ExpandWidth(true));

                EditorGUI.BeginChangeCheck();
                config.AdvertisementModuleConfig.IsEnabled = GUILayout.Toggle(config.AdvertisementModuleConfig.IsEnabled, "Enabled", GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    if (config.AdvertisementModuleConfig.IsEnabled)
                        ScriptingDefinesManager.AddDefine(Constants.Define.ADVERTISEMENT, GetBuildTargetGroup());
                    else
                        ScriptingDefinesManager.RemoveDefine(Constants.Define.ADVERTISEMENT, GetBuildTargetGroup());

                    EditorUtility.SetDirty(config);
                }
                EditorGUILayout.Space(5);

                //if (GUILayout.Button("TEst", GUILayout.ExpandWidth(true), GUILayout.Height(60)))
                //{};

                if (config.AdvertisementModuleConfig.IsEnabled)
                {
                    config.AdvertisementModuleConfig.AdvertisingSourceType = (AdvertisingSourceType)EditorGUILayout.EnumPopup("Advertising source", config.AdvertisementModuleConfig.AdvertisingSourceType);
                    config.AdvertisementModuleConfig.SuccessfulRewardResetInterstitial = EditorGUILayout.Toggle("Successful reward reset interstitial", config.AdvertisementModuleConfig.SuccessfulRewardResetInterstitial);
                    config.AdvertisementModuleConfig.AdvertisementDebugPrefab = EditorGUILayout.ObjectField(config.AdvertisementModuleConfig.AdvertisementDebugPrefab, typeof(AdvertisementDebug), true) as AdvertisementDebug;
                    config.AdvertisementModuleConfig.NeutralAgeScreen = EditorGUILayout.Toggle("Neutral age screen", config.AdvertisementModuleConfig.NeutralAgeScreen);

                    if (config.AdvertisementModuleConfig.NeutralAgeScreen)
                    {

                        config.AdvertisementModuleConfig.NeutralAgeScreenManagerPrefab = EditorGUILayout.ObjectField(config.AdvertisementModuleConfig.NeutralAgeScreenManagerPrefab, typeof(NeutralAgeScreenManager), true) as NeutralAgeScreenManager;
                    }
                }
            }
        }

        private void DrawLoadingScreenModuleInspector(AppBuilderConfig config)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Loading screen module", GetTitleStyle());
                EditorGUI.BeginChangeCheck();
                config.LoadingScreenConfig.IsEnabled = GUILayout.Toggle(config.LoadingScreenConfig.IsEnabled, "Enabled");
                if (EditorGUI.EndChangeCheck())
                {
                    if (config.LoadingScreenConfig.IsEnabled)
                        ScriptingDefinesManager.AddDefine(Constants.Define.LOADING_SCREEN, GetBuildTargetGroup());
                    else
                        ScriptingDefinesManager.RemoveDefine(Constants.Define.LOADING_SCREEN, GetBuildTargetGroup());

                    EditorUtility.SetDirty(config);
                }
                EditorGUILayout.Space(5);

                if (config.LoadingScreenConfig.IsEnabled)
                {
                    config.LoadingScreenConfig.LoadingScreenPrefab = EditorGUILayout.ObjectField(config.LoadingScreenConfig.LoadingScreenPrefab, typeof(LoadingScreenView), true) as LoadingScreenView;
                }
            }
        }

        private BuildTargetGroup[] GetBuildTargetGroup()
        {
            return new BuildTargetGroup[] {
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.Standalone,
            BuildTargetGroup.WebGL
        };
        }

        private GUIStyle GetTitleStyle()
        {
            return new GUIStyle(EditorStyles.label)
            {
                fontStyle = UnityEngine.FontStyle.Bold,
                fontSize = 14
            };
        }
    }
}
