using PimpochkaGames.CoreLibrary;
using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public abstract class Module<TModule, TConfig> : MonoBehaviour
    where TConfig : ConfigPropertyGroup
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
}
