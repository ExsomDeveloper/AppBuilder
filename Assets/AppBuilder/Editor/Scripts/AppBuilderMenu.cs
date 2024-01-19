using UnityEditor;
using UnityEngine;
using AppBuilder;

namespace AppBuilderEditor
{
    public class AppBuilderMenu : Editor
    {
        [MenuItem("App Builder/Settings", false, 2)]
        public static void OpenBuildWindow()
        {
            AppBuilderConfig config = Resources.Load(nameof(AppBuilderConfig)) as AppBuilderConfig;
            Selection.activeObject = config;
        }

        [MenuItem("App Builder/Documentation", false, 0)]
        public static void Documentation()
        {

        }
    }
}
