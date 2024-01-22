using UnityEngine;

namespace PimpochkaGames.CoreLibrary
{
    public class SettingsObject : ScriptableObject
    {
        public event Callback OnSettingsUpdated;

        protected virtual void Awake()
        {
            UpdateLoggerSettings();
        }

        protected virtual void OnValidate()
        {
            UpdateLoggerSettings();

            OnSettingsUpdated?.Invoke();
        }

        protected virtual void UpdateLoggerSettings() { }

        public virtual void OnEditorReload()
        {
            UpdateLoggerSettings();
        }
    }
}