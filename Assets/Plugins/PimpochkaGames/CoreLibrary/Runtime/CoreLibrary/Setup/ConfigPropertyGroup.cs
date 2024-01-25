using System;
using UnityEngine;

namespace PimpochkaGames.CoreLibrary
{
    [SerializeField]
    public abstract class ConfigPropertyGroup
    {
        [SerializeField] private bool _isEnabled = true;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
#if UNITY_EDITOR
                //if (!value && !CanToggleFeatureUsageState())
                //{
                //    Debug.LogWarning(string.Format("Ignoring feature: {0} state value change request because current build configuration provides limited support for stripping code. Check EssentialKitSettings file for fix.", Name));
                //    return;
                //}
                _isEnabled = value;
#endif
            }
        }

        public string Name { get; private set; }

        protected ConfigPropertyGroup(string name, bool isEnabled = true)
        {
            _isEnabled = isEnabled;
            Name = name;
        }

//#if UNITY_EDITOR
//        public static bool CanToggleFeatureUsageState()
//        {
//            var target = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
//            var targetGroup = UnityEditor.BuildPipeline.GetBuildTargetGroup(target);
//            var strippingLevel = UnityEditor.PlayerSettings.GetManagedStrippingLevel(targetGroup);
//#if !UNITY_2019_3_OR_NEWER
//            return (UnityEditor.PlayerSettings.scriptingRuntimeVersion == UnityEditor.ScriptingRuntimeVersion.Latest) && ((strippingLevel == UnityEditor.ManagedStrippingLevel.Medium) || (strippingLevel == UnityEditor.ManagedStrippingLevel.High));
//#else
//            return (strippingLevel == UnityEditor.ManagedStrippingLevel.Medium) || (strippingLevel == UnityEditor.ManagedStrippingLevel.High);
//#endif
//        }
//#endif
    }
}
