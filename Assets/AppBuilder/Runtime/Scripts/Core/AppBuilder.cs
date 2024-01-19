using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppBuilder
{
    public class AppBuilder : MonoBehaviour
    {
        private static AppBuilder _instance;
        private readonly static Context _context = new Context();

        public static AppBuilder Instance
        {
            get
            {
                if (_instance == null)
                    UnityEngine.Debug.LogError("The core is not initialized!");

                return _instance;
            }
            private set { _instance = value; }
        }

        private static bool _initialized = false;

        public static async UniTask Run(List<Installer> installers, List<InstallerAsync> installersAsync)
        {
            if (_initialized)
                return;

            _initialized = true;

            AppBuilderConfig config = await Resources.LoadAsync(nameof(AppBuilderConfig)) as AppBuilderConfig;

            GameObject gameObjectAppCore = new GameObject(nameof(AppBuilder));
            Instance = gameObjectAppCore.AddComponent<AppBuilder>();
            DontDestroyOnLoad(gameObjectAppCore);

            RegisterModules(config, _context, gameObjectAppCore);

            foreach (var installer in installers)
            {
                installer.Install(_context);
            }

            UniTask[] tasks = new UniTask[installersAsync.Count];
            for (int i = 0; i < installersAsync.Count; i++)
            {
                tasks[i] = installersAsync[i].InstallAsync(_context);
            }

            await UniTask.WhenAll(tasks);

            var loadSceneOperation = SceneManager.LoadSceneAsync(config.NextSceneName, LoadSceneMode.Single).ToUniTask();
#if LOADING_SCREEN_MODULE
            await LoadingScreenModule.SetLoadingProcess(loadSceneOperation, 1);
#else
            await loadSceneOperation;
#endif
        }

        private static void RegisterModules(AppBuilderConfig config, Context context, GameObject root)
        {
#if LOADING_SCREEN_MODULE
            var loadingScreenModule = root.AddComponent<LoadingScreenModule>();
            loadingScreenModule.Initialize(config.LoadingScreenConfig);
#endif

#if ADVERTISEMENT_MODULE
            var advertisementModule = root.AddComponent<AdvertisementModule>();
            advertisementModule.Initialize(config.AdvertisementModuleConfig);
#endif
        }
    }
}
