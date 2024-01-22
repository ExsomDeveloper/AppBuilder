using UnityEditor;

namespace PimpochkaGames.CoreLibrary.Editor
{
    public class SettingsObjectEditorManager
    {
        [InitializeOnLoadMethod]
        private static void OnEditorReload()
        {
             var settingsObjects = AssetDatabaseUtility.FindAssetObjects<SettingsObject>();  
             foreach (var obj in settingsObjects)         
             {
                obj.OnEditorReload();
             }
        }
    }
}