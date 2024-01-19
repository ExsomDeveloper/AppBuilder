#if ADVERTISEMENT_MODULE
using System.Collections;
using UnityEngine;

namespace AppBuilder
{
    public class InterstitialTimerShowHandler : ITimerHandler
    {
        private readonly WaitUntil _waitCondition;
        private bool _interstitialFinished = false;
        private string _placementName;

        public InterstitialTimerShowHandler(string placementName = "")
        {
            _placementName = placementName;
            _waitCondition = new WaitUntil(() => _interstitialFinished == true);
        }

        public IEnumerator Execute()
        {
            AdvertisementModule.ShowInterstitial( 
                (_) => _interstitialFinished = true,
                _placementName);

            yield return _waitCondition;
            _interstitialFinished = false;
        }
    }
}
#endif
