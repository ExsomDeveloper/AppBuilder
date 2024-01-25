using PimpochkaGames.CoreLibrary;
using UnityEditor;
using UnityEngine;

namespace PimpochkaGames.AppBuilder.Editor
{
    public class UnityAppBuilderUIEditorUtility
    {
        private static T LoadPrefab<T>(string name) where T : Object
        {
            string prefabPath = CoreLibrarySettings.Package.GetFullPath($"Prefabs/{name}");
            return AssetDatabase.LoadAssetAtPath<T>(prefabPath);
        }

        public static AdvertisementDebug LoadAdvertisementDebugPrefab()
        {
            return LoadPrefab<AdvertisementDebug>($"{nameof(AdvertisementDebug)}.prefab");
        }
    }
}