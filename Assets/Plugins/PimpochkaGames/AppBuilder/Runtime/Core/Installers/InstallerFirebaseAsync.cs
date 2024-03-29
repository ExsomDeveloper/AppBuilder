#if FIREBASE_MODULE
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public class InstallerFirebaseAsync : IInstallerAsync
    {
#if AB_FIREBASE_REMOTE_CONFIG
        [SerializeField] private RemoteConfigDefaultProperty<bool>[] _boolProperties;
        [SerializeField] private RemoteConfigDefaultProperty<int>[] _integersProperties;
        [SerializeField] private RemoteConfigDefaultProperty<string>[] _stringsProperties;
#endif

        private FirebaseApp _app = null;
        private InitializationStatus _firebaseStatus;
        private InitializationStatus _remoteConfig;

        public async UniTask InstallAsync(Context context)
        {
            _ = FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    _app = FirebaseApp.DefaultInstance;
#if AB_FIREBASE_CRASHLYTICS
                    Crashlytics.ReportUncaughtExceptionsAsFatal = true;
#endif

#if AB_FIREBASE_REMOTE_CONFIG
                    SetRemoteConfigDefaults();
#endif

                    _firebaseStatus = InitializationStatus.Ready;
                }
                else
                {
                    _firebaseStatus = InitializationStatus.Failed;
                    Debug.LogError(String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });

            var firebaseInitOperation = UniTask.WaitUntilValueChanged(this, x => x._firebaseStatus == InitializationStatus.Ready || x._firebaseStatus == InitializationStatus.Failed);
            var remoteConfigInitOperation = UniTask.WaitUntilValueChanged(this, x => x._remoteConfig == InitializationStatus.Ready || x._remoteConfig == InitializationStatus.Failed);

            await UniTask.WhenAll(firebaseInitOperation, remoteConfigInitOperation)
                .TimeoutWithoutException(TimeSpan.FromSeconds(5));
        }

#if AB_FIREBASE_REMOTE_CONFIG
        private void SetRemoteConfigDefaults()
        {
            var defaultValues = new Dictionary<string, object>(
                _boolProperties.Length + _integersProperties.Length + _stringsProperties.Length);

            AddDefaultProperties(defaultValues, _boolProperties);
            AddDefaultProperties(defaultValues, _integersProperties);
            AddDefaultProperties(defaultValues, _stringsProperties);

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            remoteConfig.SetDefaultsAsync(defaultValues).ContinueWithOnMainThread(
               previousTask =>
               {
                   FetchRemoteConfig();
               }
            );

            void AddDefaultProperties<T>(Dictionary<string, object> dictionary, RemoteConfigDefaultProperty<T>[] defaultProperties)
            {
                for (int i = 0; i < defaultProperties.Length; i++)
                {
                    dictionary.Add(
                        defaultProperties[i].Key,
                        defaultProperties[i].Value);
                }
            }
        }

        private void FetchRemoteConfig()
        {
            if (_app == null)
            {
                _remoteConfig = InitializationStatus.Failed;
                Debug.LogError($"Do not use Firebase until it is properly initialized by calling {nameof(InstallerFirebase)}.");
                return;
            }

            Debug.Log("Fetching data...");
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            remoteConfig.FetchAsync(TimeSpan.FromHours(6)).ContinueWithOnMainThread(
               previousTask =>
               {
                   if (!previousTask.IsCompleted)
                   {
                       _remoteConfig = InitializationStatus.Failed;
                       Debug.LogError($"{nameof(remoteConfig.FetchAsync)} incomplete: Status '{previousTask.Status}'");
                       return;
                   }
                   ActivateRetrievedRemoteConfigValues();
               });
        }

        private void ActivateRetrievedRemoteConfigValues()
        {
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus == LastFetchStatus.Success)
            {
                remoteConfig.ActivateAsync().ContinueWithOnMainThread(
                   previousTask =>
                   {
                       Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                       _remoteConfig = InitializationStatus.Ready;
                   });
            }
            else
            {
                _remoteConfig = InitializationStatus.Failed;
            }
        }
#endif
    }
}
#endif
