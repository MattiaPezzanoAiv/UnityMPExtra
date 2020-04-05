namespace MP.Time
{
    using UnityEngine;

    /// <summary>
    /// This class allows you to use deltatime like values but having different time scales for different sources.
    /// </summary>
    public class TimeSource
    {
        public TimeSource(float timeScale = 1f)
        {
            TimeScale = timeScale;
        }

        public float TimeScale { get; set; }

        public float DeltaTime => Time.deltaTime * TimeScale;

        public float FixedDeltaTime => Time.fixedDeltaTime * TimeScale;

        public float UnscaledDeltaTime => Time.unscaledDeltaTime;
        
        public float UnscaledFixedDeltaTime => Time.fixedUnscaledDeltaTime;
    }
}
