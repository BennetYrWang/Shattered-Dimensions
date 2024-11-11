using System;
using UnityEngine;

namespace BennetWang.Module.Timer
{
    /// <summary>
    /// This class represents the data of a Timer for timing feature
    /// in regular unity updates, and is thread-unsafe. It will automatically
    /// dispose itself when time's up. Or you can dispose a Timer manually.
    /// </summary>
    /// <author>王奕然 Bennet Wang (bennet.yr.wang@gmail.com)</author>
    /// <version>1.0</version>
    /// <date>November 10, 2024</date>
    public sealed class Timer
    {
        public float TimeLimit { get; set; }
        public UpdateType Type { get; private set; }
        public float TimePassed { get; private set; }
        public bool IgnoreTimeScale { get; set; }
        public bool Paused { get; set; }
        /// <summary>
        /// If a timer is Disposed, it won't be useful forever.
        /// </summary>
        public bool Disposed { get; private set; }
        public Action callback;
        
        private Timer() { }

        public Timer(float timeLimit, Action callback,
            UpdateType updateType, bool ignoreTimeScale = false)
        {
            TimeLimit = timeLimit;
            this.callback = callback;
            IgnoreTimeScale = ignoreTimeScale;
            Type = updateType;
            Paused = false;
        }

        public void Update(float deltaTime, out bool timesUp)
        {
            if (Disposed)
            {
                timesUp = true;
                return;
            }
            
            TimePassed += deltaTime;
            timesUp = TimePassed >= TimeLimit;

            if (timesUp)
            {
                Disposed = true;
                callback?.Invoke();
                callback = null;
            }
        }
        /// <summary>
        /// Reset the time Passed, but this is not allowed when Timer is already disposed
        /// </summary>
        /// <returns></returns>
        public bool Reset()
        {
            if (Disposed)
                return false;
            TimePassed = 0f;
            return true;
        }

        public void Dispose()
        {
            if (Disposed)
            {
                Debug.Log($"Timer{this} already disposed.");
                return;
            }
            Disposed = true;
            callback = null;
        }
        
        public enum UpdateType
        {
            Update,
            LateUpdate,
            FixedUpdate,
        }
    }
}