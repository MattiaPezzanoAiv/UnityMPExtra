namespace MP.Unity.Sensors
{
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public class UnityEventNoiseHearedData : UnityEvent<NoiseMaker.NoiseHearedData> { }

    [System.Serializable]
    public class UnityEventNoiseMaker : UnityEvent<NoiseMaker> { }

    /// <summary>
    /// Interface to have custom noise listener
    /// </summary>
    public interface INoiseListener
    {
        void OnStartHearNoise(NoiseMaker.NoiseHearedData data);

        void OnStopHearNoise(NoiseMaker source);

        Transform Transform { get; }

        bool Enabled { get; }
    }

    /// <summary>
    /// Base class to hear noise. You can always implement <see cref="INoiseListener"/> in your classes to have custom listener classes
    /// </summary>
    public class NoiseListener : MonoBehaviour, INoiseListener
    {
        [SerializeField]
        private UnityEventNoiseHearedData OnStartHearNoise;

        [SerializeField]
        private UnityEventNoiseMaker OnStopHearNoise;

        public Transform Transform => transform;

        public bool Enabled => enabled;

        void INoiseListener.OnStopHearNoise(NoiseMaker source)
        {
            OnStopHearNoise?.Invoke(source);
        }

        void INoiseListener.OnStartHearNoise(NoiseMaker.NoiseHearedData data)
        {
            OnStartHearNoise?.Invoke(data);
        }
    }
}
