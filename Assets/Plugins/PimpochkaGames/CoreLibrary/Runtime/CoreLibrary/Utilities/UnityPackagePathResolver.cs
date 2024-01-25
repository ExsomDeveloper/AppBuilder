using UnityEngine;

namespace PimpochkaGames.CoreLibrary
{
    public static class UnityPackagePathResolver
    {
        public static string GetMutableResourcesPath(this UnityPackageDefinition package)
        {
            return package.MutableResourcesPath;
        }

        public static string GetFullPath(this UnityPackageDefinition package, string relativePath)
        {
            return CombinePath(pathA: GetInstallPath(package), pathB: relativePath);
        }

        public static string GetInstallPath(this UnityPackageDefinition package)
        {
            if (IsSupported())
            {
                return IsInstalledWithinAssets(package) ? package.DefaultInstallPath : package.UpmInstallPath;
            }
            return null;
        }

        public static bool IsInstalledWithinAssets(this UnityPackageDefinition package)
        {
            return IOServices.DirectoryExists(package.DefaultInstallPath) &&
                IOServices.FileExists($"{package.DefaultInstallPath}/package.json");
        }

        private static bool IsSupported() => Application.isEditor;

        private static string CombinePath(string pathA, string pathB)
        {
            if (pathA == null)
            {
                return null;
            }
            else if (pathA == "")
            {
                return pathB;
            }
            else
            {
                return $"{pathA}/{pathB}";
            }
        }
    }
}
