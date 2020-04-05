namespace MP.Time
{
    using System.Collections;
    using UnityEngine;
    using System;
    using MP.Coroutines;
    using MP.Extensions;

    public class Timer
    {
        private float m_duration;

        private Coroutine m_coroutine;

        private TimeSource m_timeSource;

        public float Duration
        {
            get
            {
                return m_duration;
            }

            set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Negative values are not allowed. Value is: " + value);
                }

                m_duration = value;
            }
        }

        public Action Elapsed { get; set; }

        private Func<float> InternalDeltaTime;

        public Timer(TimeSource ts = null)
        {
            m_timeSource = ts;
            InternalDeltaTime = m_timeSource != null
                ? new Func<float>(() => m_timeSource.DeltaTime)
                : new Func<float>(() => Time.deltaTime);
        }

        /// <summary>
        /// Starts a new timer, if another o ne is already running, will be killed without triggering <see cref="Elapsed"/> callback
        /// </summary>
        public void Start()
        {
            Stop();
            m_coroutine = GlobalCoroutineRunner.Instance.StartCoroutine(TimerCoroutine());
        }

        /// <summary>
        /// Stops the running timer without calling <see cref="Elapsed"/> callback.
        /// </summary>
        public void Stop()
        {
            GlobalCoroutineRunner.Instance.SafeStopCoroutine(ref m_coroutine);
        }

        private IEnumerator TimerCoroutine()
        {
            for (float i = 0; i < m_duration; i += InternalDeltaTime())
            {
                yield return null;
            }

            Elapsed?.Invoke();
            Stop();
        }
    }
}
