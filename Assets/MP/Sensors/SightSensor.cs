namespace MP.Unity.Sensors
{
    using MP.Extensions;
    using MP.Unity.Physics;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Implement this interface if you want to receive sight related callbacks
    /// IE implement this on player script to know if he's seen or not.
    /// </summary>
    public interface ISightTarget
    {
        void OnStartBeingSeen(SightSensor sensor);

        void OnEndBeingSeen(SightSensor sensor);

        Transform Transform { get; }

        bool IsInvisible { get; }

        /// <summary>
        /// This value should be between 0 and 1. 1 means character is fully visible. 0 means is not visible at all.
        /// </summary>
        float StealthMultiplier { get; }
    }

    [System.Serializable]
    public class UnityEventSightTarget : UnityEvent<ISightTarget> { }

    public class SightSensor : MonoBehaviour
    {
        [System.Serializable]
        private struct SightData
        {
            public Transform Eyes;

            public float SightDistance;

            public float SightAngle;
        }

        [System.Serializable]
        private struct SightDebug
        {
            public bool Active;

            public Color SightRaysColor;
        }

        [Header("DEBUG")]
        [SerializeField]
        private SightDebug m_debugData;

        [Header("DATA")]
        [SerializeField]
        private SightData m_sightData;

        public UnityEventSightTarget StartSeeingTarget;

        public UnityEventSightTarget StopSeeingTarget;

        private Collider[] m_inRangeColliders = new Collider[10];

        private RaycastHit m_cachedHit = new RaycastHit();

        private Ray m_cachedRay = new Ray();

        private readonly HashSet<ISightTarget> m_seenTargets = new HashSet<ISightTarget>();

        private readonly HashSet<ISightTarget> m_seenTargetsThisFrame = new HashSet<ISightTarget>();

        private float HalfAngle => m_sightData.SightAngle * 0.5f;

        private Vector3 EyesPosition => m_sightData.Eyes.position;

        private Vector3 EyesForward => m_sightData.Eyes.forward;

        private Vector3 EyesUp => m_sightData.Eyes.up;

        private void Update()
        {
            m_seenTargetsThisFrame.Clear();

            if(m_debugData.Active)
            {
                float length = m_sightData.SightDistance;
                var color = m_debugData.SightRaysColor;
                Debug.DrawRay(EyesPosition, EyesForward * length * 0.5f, color);
                Debug.DrawRay(EyesPosition, Quaternion.Euler(EyesUp * m_sightData.SightAngle * 0.5f) * EyesForward * length, color);
                Debug.DrawRay(EyesPosition, Quaternion.Euler(EyesUp * -m_sightData.SightAngle * 0.5f) * EyesForward * length, color);
            }

            int count = PhysicsUtility.OverlapSphereNonAlloc(
                m_sightData.Eyes.position,
                m_sightData.SightDistance,
                ref m_inRangeColliders);

            if (count < 0)
            {
                ClearAllCurrentlySeenTargets();
                return;
            }

            // set ray origin just once since eyes position can't change
            m_cachedRay.origin = EyesPosition;

            for (int i = 0; i < count; i++)
            {
                var c = m_inRangeColliders[i];
                var target = c.GetComponent<ISightTarget>();
                if (target == null || target.IsInvisible)
                {
                    continue;
                }

                var dirToTarget = EyesPosition - target.Transform.position;
                var distance = dirToTarget.magnitude;

                if (distance > m_sightData.SightDistance * target.StealthMultiplier)
                {
                    // in stealth if too far is not seeable
                    continue;
                }

                // check angle
                var angle = Vector3.Angle(dirToTarget.normalized, EyesForward);
                if (angle > HalfAngle)
                {
                    // out of sight
                    continue;
                }

                // ray to check occlusion
                m_cachedRay.direction = dirToTarget.normalized;
                if (!Physics.Raycast(m_cachedRay, out m_cachedHit, dirToTarget.magnitude))
                {
                    // is occluded
                    continue;
                }

                m_seenTargetsThisFrame.Add(target);
                if (!m_seenTargets.Contains(target))
                {
                    m_seenTargets.Add(target);
                    target.OnStartBeingSeen(this);
                    StartSeeingTarget?.Invoke(target);
                }
            }

            m_seenTargets.RemoveElementIfContained(m_seenTargetsThisFrame, t =>
            {
                t.OnEndBeingSeen(this);
                StopSeeingTarget?.Invoke(t);
            });
        }

        private void ClearAllCurrentlySeenTargets(bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                foreach (var t in m_seenTargets)
                {
                    t.OnEndBeingSeen(this);
                    StopSeeingTarget?.Invoke(t);
                }
            }

            m_seenTargets.Clear();
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}
