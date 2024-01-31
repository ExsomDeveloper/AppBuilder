using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PimpochkaGames.NeutralAgeScreen;

namespace PimpochkaGames.AppBuilder
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

            AppBuilderConfig config = AppBuilderConfig.Instance;
            GameObject gameObjectAppCore = new GameObject(nameof(AppBuilder));
            Instance = gameObjectAppCore.AddComponent<AppBuilder>();
            DontDestroyOnLoad(gameObjectAppCore);

            RegisterModules(config, _context, gameObjectAppCore);
            await RunInstallers(installers, installersAsync, _context);

#if ADVERTISEMENT_MODULE
            if (config.AdvertisementModuleConfig.NeutralAgeScreen)
            {
                if (NeutralAgeScreenManager.IsAgeReady == false)
                    await UniTask.WaitUntil(() => NeutralAgeScreenManager.IsAgeReady == true);
            }
#endif

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

        private async static UniTask RunInstallers(List<Installer> installers, List<InstallerAsync> installersAsync, Context context)
        {
            foreach (var installer in installers)
                installer.Install(_context);

            foreach (var installer in GetCoreInstallers())
                installer.Install(_context);

            var coreInstallersAsync = GetCoreInstallersAsync();
            UniTask[] tasks = new UniTask[coreInstallersAsync.Length + installersAsync.Count];
            int index = 0;
            for (; index < coreInstallersAsync.Length; index++)
                tasks[index] = coreInstallersAsync[index].InstallAsync(_context);

            for (; index < tasks.Length; index++)
                tasks[index] = installersAsync[index].InstallAsync(_context);

            await UniTask.WhenAll(tasks);
        }

        private static IInstaller[] GetCoreInstallers()
        {
            return new IInstaller[]
            {
#if UNITY_IOS
                new InstallerIosAtt()
#endif
            };
        }

        private static IInstallerAsync[] GetCoreInstallersAsync()
        {
            return new IInstallerAsync[]
            {
#if FIREBASE_MODULE
                new InstallerFirebaseAsync()
#endif
            };
        }
    }
}
