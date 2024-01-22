using UnityEngine;

namespace PimpochkaGames.CoreLibrary
{
    [System.Serializable]
    public class UnityPackageDefinition
    {
        private string _persistentDataRelativePath;

        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string Version { get; private set; }
        public string DefaultInstallPath { get; private set; }
        public string UpmInstallPath { get; private set; }
        public string MutableResourcesPath { get; private set; }
        public string MutableResourcesRelativePath { get; private set; }
        public UnityPackageDefinition[] Dependencies { get; private set; }

        public string PersistentDataRelativePath
        {
            get
            {
                EnsurePersistentDataPathExists();
                return _persistentDataRelativePath;
            }
        }

        public string PersistentDataPath
        {
            get
            {
                EnsurePersistentDataPathExists();
                return GetPersistentDataPathInternal();
            }
        }


        public UnityPackageDefinition(string name, string displayName,
            string version, string defaultInstallPath = null,
            string mutableResourcesPath = "Assets/Resources", string persistentDataRelativePath = null,
            params UnityPackageDefinition[] dependencies)
        {
            // Set properties
            Name                            = name;
            DisplayName                     = displayName;
            Version                         = version;
            DefaultInstallPath              = defaultInstallPath ?? $"Assets/{Name}";
            UpmInstallPath                  = $"Packages/{Name}";
            MutableResourcesPath            = mutableResourcesPath;
            Dependencies                    = dependencies;

            // Derived properties
            MutableResourcesRelativePath    = mutableResourcesPath.Replace("Assets/Resources", "").TrimStart('/');
            _persistentDataRelativePath     = persistentDataRelativePath ?? $"VoxelBusters/{string.Join("", displayName.Split(' '))}";
        }

        private void EnsurePersistentDataPathExists()
        {
            var fullPath = GetPersistentDataPathInternal();
            IOServices.CreateDirectory(fullPath, overwrite: false);
        }

        private string GetPersistentDataPathInternal()
        {
            return IOServices.CombinePath(Application.persistentDataPath, _persistentDataRelativePath);
        }
    }
}