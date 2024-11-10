using System;

namespace Bennet.Module.Timer
{
    public class Timer
    {
        private float _duration;
        private float timePassed;
        public bool ignoreTimeScale;
        private UpdateType _updateType;
        public Action callback;
        public bool active;

        public bool TimesUp { get; private set; }
        
        private Timer() { }
        public Timer(float duration, Action callback,
            bool ignoreTimeScale = false, UpdateType updateType = UpdateType.Update)
        {
            _duration = duration;
            this.callback = callback;
            this.ignoreTimeScale = ignoreTimeScale;
            _updateType = updateType;
        }

        public void Update(float deltaTime, out bool timesUp)
        {
            timePassed += deltaTime;
            if (timePassed > _duration)
                timesUp = true;
            timesUp = TimesUp;
        }

        public enum UpdateType
        {
            Update,
            LateUpdate,
            FixedUpdate,
        }
    }
}