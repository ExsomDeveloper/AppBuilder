using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public abstract class Module<TModule, TConfig> : MonoBehaviour
    where TConfig : ModuleConfig
    {
        public static bool Initialized { get; private set; }

        public void Initialize(TConfig config)
        {
            if (Initialized)
                return;

            OnInitialize(config);
            Initialized = true;
        }

        protected abstract void OnInitialize(TConfig config);
    }

    [System.Serializable]
    public abstract class ModuleConfig
    {
    }
}
