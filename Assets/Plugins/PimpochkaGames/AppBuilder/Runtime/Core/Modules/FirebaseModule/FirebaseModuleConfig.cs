using PimpochkaGames.CoreLibrary;

namespace PimpochkaGames.AppBuilder
{
    [System.Serializable]
    public class FirebaseModuleConfig : ConfigPropertyGroup
    {
        public bool RemoteConfig = false;
        public bool Crashlytics = false;

        public FirebaseModuleConfig(string name, bool isEnabled = true) : base(nameof(Constants.Define.FIREBASE), isEnabled)
        {
        }
    }
}
