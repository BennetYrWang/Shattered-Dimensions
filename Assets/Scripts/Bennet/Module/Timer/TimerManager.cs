using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bennet.Module.Timer
{
    [DefaultExecutionOrder(-999)]
    public class TimerManager : MonoBehaviour
    {
        private static TimerManager _instance;
        private static TimerManager Instance
        {
            get
            {
                if (_instance == null)
                    return ModuleManager.Instance.gameObject.AddComponent<TimerManager>();
                return _instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticInstance()
        {
            _instance = null;
        }


        private List<Timer> UpdateTimerList = new List<Timer>();
        private List<Timer> LateUpdateTimerList = new List<Timer>();
        private List<Timer> FixedUpdateTimerList = new List<Timer>();

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            float unscaledDeltaTime = Time.unscaledDeltaTime;
            for (int i = UpdateTimerList.Count; i >= 0; i--)
            {
                Timer timer = UpdateTimerList[i];
                timer.Update(timer.ignoreTimeScale? unscaledDeltaTime : deltaTime,out bool timeUP);
                if (timeUP)
                {
                    timer.callback?.Invoke();
                    UpdateTimerList.RemoveAt(i);
                }
            }
        }
        private void FixedUpdate()
        {
            float deltaTime = Time.fixedUnscaledDeltaTime;
            float unscaledDeltaTime = Time.fixedDeltaTime;
            for (int i = UpdateTimerList.Count; i >= 0; i--)
            {
                Timer timer = UpdateTimerList[i];
                timer.Update(timer.ignoreTimeScale? unscaledDeltaTime : deltaTime,out bool timeUP);
                if (timeUP)
                {
                    timer.callback?.Invoke();
                    UpdateTimerList.RemoveAt(i);
                }
            }
        }

        private void UpdateTimers(List<Timer> timerList, float deltaTime, float unscaledDeltaTime)
        {
            for (int i = timerList.Count; i >= 0; i--)
            {
                Timer timer = UpdateTimerList[i];
                timer.Update(timer.ignoreTimeScale? unscaledDeltaTime : deltaTime,out bool timeUP);
                if (timeUP)
                {
                    timer.callback?.Invoke();
                    UpdateTimerList.RemoveAt(i);
                }
            }
        }


        public Timer CreateTimer(float duration, Action callback,
            bool ignoreTimeScale = true, Timer.UpdateType updateType = Timer.UpdateType.Update)
        {
            Timer timer = new Timer(duration, callback,ignoreTimeScale, updateType);
            switch (updateType)
            {
                case Timer.UpdateType.Update:
                    UpdateTimerList.Add(timer);
                    break;
                case Timer.UpdateType.FixedUpdate:
                    FixedUpdateTimerList.Add(timer);
                    break;
                case Timer.UpdateType.LateUpdate:
                    LateUpdateTimerList.Add(timer);
                    break;
            }

            return timer;
        }
        
    }
}