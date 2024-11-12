using System;
using System.Collections.Generic;
using UnityEngine;

namespace BennetWang.Module.Timer
{
    /// <summary>
    /// This class is designed to provide a convenient timing feature
    /// in regular unity updates, and is thread-unsafe.
    /// </summary>
    /// <author>王奕然 Bennet Wang (bennet.yr.wang@gmail.com)</author>
    /// <version>1.0</version>
    /// <date>November 10, 2024</date>
    [DefaultExecutionOrder(-999)]
    public sealed class TimerModule : AbstractModule
    {
        #region Module Registration

        private static TimerModule _instance;

        private static TimerModule Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = ModuleManager.Instance.gameObject.AddComponent<TimerModule>();
                    _instance.Init();
                }

                return _instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticInstance()
        {
            _instance = null;
        }


        protected override void InitBehaviour()
        {
            ModuleManager.Instance.TryRegisterModule<TimerModule>(this);
        }

        protected override void OnDestroyBehaviour()
        {
            ModuleManager.Instance.DeregisterModule<TimerModule>();
        }

        #endregion


        private List<Timer> UpdateTimerList = new();
        private List<Timer> LateUpdateTimerList = new();
        private List<Timer> FixedUpdateTimerList = new();

        private void Update()
        {
            if (UpdateTimerList.Count > 0)
            {
                float deltaTime = Time.deltaTime;
                float unscaledDeltaTime = Time.unscaledDeltaTime;
                UpdateTimers(UpdateTimerList, deltaTime, unscaledDeltaTime);
            }
        }

        private void LateUpdate()
        {
            if (LateUpdateTimerList.Count > 0)
            {
                float deltaTime = Time.deltaTime;
                float unscaledDeltaTime = Time.unscaledDeltaTime;
                UpdateTimers(LateUpdateTimerList, deltaTime, unscaledDeltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (FixedUpdateTimerList.Count > 0)
            {
                float deltaTime = Time.fixedDeltaTime;
                float unscaledDeltaTime = Time.fixedUnscaledDeltaTime;
                UpdateTimers(FixedUpdateTimerList, deltaTime, unscaledDeltaTime);
            }
        }

        private void UpdateTimers(List<Timer> timerList, float deltaTime, float unscaledDeltaTime)
        {
            timerList.RemoveAll(timer =>
            {
                if (timer.Paused)
                    return false;
                timer.Update(timer.IgnoreTimeScale ? unscaledDeltaTime : deltaTime, out bool timesUp);
                return timesUp;
            });
        }

        public static Timer CreateTimer(float duration, Timer.UpdateType updateType, Action callback,
            bool ignoreTimeScale = false)
        {
            Timer timer = new Timer(duration, callback, updateType, ignoreTimeScale);
            switch (updateType)
            {
                case Timer.UpdateType.Update:
                    Instance.UpdateTimerList.Add(timer);
                    break;
                case Timer.UpdateType.FixedUpdate:
                    Instance.FixedUpdateTimerList.Add(timer);
                    break;
                case Timer.UpdateType.LateUpdate:
                    Instance.LateUpdateTimerList.Add(timer);
                    break;
            }
            return timer;
        }
    }
}