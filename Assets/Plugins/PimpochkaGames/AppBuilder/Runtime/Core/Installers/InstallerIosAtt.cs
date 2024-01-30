using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_IOS
// Include the IosSupport namespace if running on iOS:
using Unity.Advertisement.IosSupport;
#endif

namespace PimpochkaGames.AppBuilder
{
    [CreateAssetMenu(fileName = nameof(InstallerIosAtt), menuName = "AppBuilder/Installer/iOS ATT")]
    public class InstallerIosAtt : Installer
    {
        public override void Install(Context context)
        {
            RequestAuthorizationTracking();
        }

        public void RequestAuthorizationTracking()
        {
#if UNITY_IOS
        // Check the user's consent status.
        // If the status is undetermined, display the request request:
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            ATTrackingStatusBinding.RequestAuthorizationTracking();
#endif
        }
    }
}
