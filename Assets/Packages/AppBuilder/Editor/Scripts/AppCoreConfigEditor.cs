using UnityEditor;
using UnityEngine;
using AppBuilder;
using PimpochkaGames.NeutralAgeScreen;

namespace AppBuilderEditor
{
    [CustomEditor(typeof(AppBuilderConfig))]
    public class AppCoreConfigEditor : Editor
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
            EditorGUILayout.Space(10);
            DrawNeutralAgeScreenModuleInspector(config);

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
                config.AdvertisementModuleEnabled = GUILayout.Toggle(config.AdvertisementModuleEnabled, "Enabled", GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    if (config.AdvertisementModuleEnabled)
                        AssetDefinePostprocessor.AddCompileDefine(Constants.Define.ADVERTISEMENT, GetBuildTargetGroup());
                    else
                        AssetDefinePostprocessor.RemoveCompileDefine(Constants.Define.ADVERTISEMENT, GetBuildTargetGroup());

                    EditorUtility.SetDirty(config);
                }
                EditorGUILayout.Space(5);

                //if (GUILayout.Button("TEst", GUILayout.ExpandWidth(true), GUILayout.Height(60)))
                //{};

                if (config.AdvertisementModuleEnabled)
                {
                    config.AdvertisementModuleConfig.AdvertisingSourceType = (AdvertisingSourceType)EditorGUILayout.EnumPopup("Advertising source", config.AdvertisementModuleConfig.AdvertisingSourceType);
                    config.AdvertisementModuleConfig.SuccessfulRewardResetInterstitial = EditorGUILayout.Toggle("Successful reward reset interstitial", config.AdvertisementModuleConfig.SuccessfulRewardResetInterstitial);
                    config.AdvertisementModuleConfig.AdvertisementDebugPrefab = EditorGUILayout.ObjectField(config.AdvertisementModuleConfig.AdvertisementDebugPrefab, typeof(AdvertisementDebug), true) as AdvertisementDebug;
                }
            }
        }

        private void DrawLoadingScreenModuleInspector(AppBuilderConfig config)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Loading screen module", GetTitleStyle());
                EditorGUI.BeginChangeCheck();
                config.LoadingScreenModuleEnabled = GUILayout.Toggle(config.LoadingScreenModuleEnabled, "Enabled");
                if (EditorGUI.EndChangeCheck())
                {
                    if (config.LoadingScreenModuleEnabled)
                        AssetDefinePostprocessor.AddCompileDefine(Constants.Define.LOADING_SCREEN, GetBuildTargetGroup());
                    else
                        AssetDefinePostprocessor.RemoveCompileDefine(Constants.Define.LOADING_SCREEN, GetBuildTargetGroup());

                    EditorUtility.SetDirty(config);
                }
                EditorGUILayout.Space(5);

                if (config.LoadingScreenModuleEnabled)
                {
                    config.LoadingScreenConfig.LoadingScreenPrefab = EditorGUILayout.ObjectField(config.LoadingScreenConfig.LoadingScreenPrefab, typeof(LoadingScreenView), true) as LoadingScreenView;
                }
            }
        }

        private void DrawNeutralAgeScreenModuleInspector(AppBuilderConfig config)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Neutral age screen module", GetTitleStyle());
                EditorGUI.BeginChangeCheck();
                config.NeutralAgeScreenModuleEnabled = GUILayout.Toggle(config.NeutralAgeScreenModuleEnabled, "Enabled");
                if (EditorGUI.EndChangeCheck())
                {
                    if (config.NeutralAgeScreenModuleEnabled)
                        AssetDefinePostprocessor.AddCompileDefine(Constants.Define.NEUTRAL_AGE_SCREEN, GetBuildTargetGroup());
                    else
                        AssetDefinePostprocessor.RemoveCompileDefine(Constants.Define.NEUTRAL_AGE_SCREEN, GetBuildTargetGroup());

                    EditorUtility.SetDirty(config);
                }
                EditorGUILayout.Space(5);

                if (config.NeutralAgeScreenModuleEnabled)
                {
                    config.NeutralAgeScreenConfig.AutoStart = EditorGUILayout.Toggle("Auto start", config.NeutralAgeScreenConfig.AutoStart);
                    config.NeutralAgeScreenConfig.NeutralAgeScreenManagerPrefab = EditorGUILayout.ObjectField(config.NeutralAgeScreenConfig.NeutralAgeScreenManagerPrefab, typeof(NeutralAgeScreenManager), true) as NeutralAgeScreenManager;
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
