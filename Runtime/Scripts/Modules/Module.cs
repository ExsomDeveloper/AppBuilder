using UnityEngine;

namespace AppBuilder
{
    public abstract class Module<TModule, TConfig> : MonoBehaviour
    where TConfig : ModuleConfig
    {
        //private static TModule _instance;
        //public static TModule Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            Debug.LogError($"The {nameof(TModule)} is not initialized!");

        //        return _instance;
        //    }
        //    private set { _instance = value; }
        //}
        public static bool Initialized { get; private set; }

        public void Initialize(TConfig config)
        {
            if (Initialized)
                return;

            //Instance = GetComponent<TModule>();
            OnInitialize(config);
            Initialized = true;
        }

        protected abstract void OnInitialize(TConfig config);
    }

    public abstract class ModuleConfig
    {
    }
}
