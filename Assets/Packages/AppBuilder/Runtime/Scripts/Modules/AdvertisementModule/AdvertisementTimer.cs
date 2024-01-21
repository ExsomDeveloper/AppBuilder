using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public class AdvertisementTimer
    {
        private readonly WaitUntil _waitPauseCondition;
        private readonly WaitForEndOfFrame _waitEndFrame;
        private readonly WaitForSecondsRealtime _waitForSecondsRealtime;
        private readonly MonoBehaviour _runner;
        private bool _running = false;
        private bool _paused = false;
        private bool _loop = false;
        private float _currentValue = 0;
        private float _startValue = 0;

        public AdvertisementTimer(MonoBehaviour runner)
        {
            _runner = runner;
            _waitForSecondsRealtime = new WaitForSecondsRealtime(1);
            _waitPauseCondition = new WaitUntil(() => _paused == false);
            _waitEndFrame = new WaitForEndOfFrame();
        }

        public void Start(int seconds, bool loop, ITimerHandler timerHandler)
        {
            StartCO(seconds, loop, timerHandler)
                .ToUniTask(_runner)
                .Forget();
        }

        public void Reset()
        {
            UnityEngine.Debug.Log("[Test] Reset");
            _currentValue = _startValue;
            Unpause();
        }

        public void Pause()
        {
            if (!_running)
                return;

            UnityEngine.Debug.Log("[Test] Pause");
            _paused = true;
        }

        public void Unpause()
        {
            UnityEngine.Debug.Log("[Test] Unpause");
            _paused = false;
        }

        public void Stop()
        {
            UnityEngine.Debug.Log("[Test] Stop");
            _running = false;
            _paused = false;
        }

        private IEnumerator StartCO(int seconds, bool loop, ITimerHandler timerHandler)
        {
            UnityEngine.Debug.Log("[Test]: StartCO");
            _loop = loop;
            _startValue = seconds;

            if (_running)
                yield break;

            _currentValue = seconds;
            _running = true;

            UnityEngine.Debug.Log("Start timer");
            while (_running)
            {
                yield return _waitForSecondsRealtime;
                _currentValue -= 1;

                yield return _waitPauseCondition;
                if (_currentValue >= 0)
                {
                    yield return _waitEndFrame;
                    continue;
                }

                yield return timerHandler.Execute();

                if (_loop)
                {
                    UnityEngine.Debug.Log("[Test]: Stop Timer -> Reset timer");
                    Reset();
                }
                else
                {
                    UnityEngine.Debug.Log("[Test]: Stop timer");
                    Stop();
                    break;
                }
            }
        }
    }
}
