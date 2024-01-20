namespace PimpochkaGames.CoreLibrary
{
    public class CoreLibrarySettings
    {
        private static UnityPackageDefinition _package;

        internal static UnityPackageDefinition Package => ObjectHelper.CreateInstanceIfNull(
            ref _package,
            () =>
            {
                return new UnityPackageDefinition(
                    name: "com.pimpochkagames.corelibrary",
                    displayName: "Core Library",
                    version: "1.0.0",
                    defaultInstallPath: $"Assets/Plugins/PimpochkaGames/CoreLibrary");
            });

        public static string Name => Package.Name;
        public static string DisplayName => Package.DisplayName;
        public static string Version => Package.Version;
    }
}