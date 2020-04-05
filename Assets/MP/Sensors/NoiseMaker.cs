namespace MP.Unity.Sensors
{
    using MP.Extensions;
    using System.Collections.Generic;
    using UnityEngine;
    using MP.Unity.Physics;
    using UnityEngine.Events;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class NoiseMaker : MonoBehaviour
    {
        [System.Serializable]
        private struct NoiseData
        {
            public Transform NoiseEmissionPoint;

            public AnimationCurve NoiseCurve;

            public LayerMask OccludedByPhysicalLayers;

            public LayerMask CanBeHeardByLayers;

            public Transform[] RayRoots;
        }

        [System.Serializable]
        private struct NoiseDebug
        {
            public bool Active;

            public Color NoiseSphereColor;

            public Color NoiseSphereColorIfHeared;

            public Color OcclusionRaysColor;

            public bool UseVelocityDebug;

            public Vector3 DebugVelocity;
        }

        [System.Serializable]
        public struct NoiseHearedData
        {
            /// <summary>
            /// The noise source
            /// </summary>
            public NoiseMaker Source;

            /// <summary>
            /// The direction from listener to noise source
            /// </summary>
            public Vector3 Direction;

            /// <summary>
            /// The distance from the noise source
            /// </summary>
            public float Distance;
        }

        [Header("DEBUG")]
        [SerializeField]
        private NoiseDebug m_debugData;

        [Header("DATA")]
        [SerializeField]
        private NoiseData m_noiseData;

        public UnityEvent StartBeingHeared;

        public UnityEvent StopBeingHeared;

        private Collider[] m_inRangeColliders = new Collider[10];

        private RaycastHit m_cachedHit = new RaycastHit();

        private Ray m_cachedRay = new Ray();

        private readonly HashSet<INoiseListener> m_hearedByListeners = new HashSet<INoiseListener>();

        private readonly HashSet<INoiseListener> m_hearedByThisFrame = new HashSet<INoiseListener>();

        private bool m_internalCurrentlyHeared;

        public float NoiseMultiplier { get; set; } = 1f;

        /// <summary>
        /// This must be set with the object velocity every frame in order to update correctly
        /// </summary>
        public Vector3 Velocity { get; set; }

        public bool CurrentlyHeared
        {
            get
            {
                return m_internalCurrentlyHeared;
            }
            set
            {
                if (value && !m_internalCurrentlyHeared)
                {
                    // heared and wast heared previously
                    StartBeingHeared?.Invoke();
                }
                else if (!value && m_internalCurrentlyHeared)
                {
                    StopBeingHeared?.Invoke();
                }

                m_internalCurrentlyHeared = value;
            }
        }

        private void Update()
        {
            m_hearedByThisFrame.Clear();

            var velocity = m_debugData.UseVelocityDebug ? m_debugData.DebugVelocity : Velocity;
            float noiseDistance = m_noiseData.NoiseCurve.Evaluate(velocity.magnitude) * NoiseMultiplier;

            // search for noise listeners in range
            int count = PhysicsUtility.OverlapSphereNonAlloc(
                m_noiseData.NoiseEmissionPoint.position,
                noiseDistance,
                ref m_inRangeColliders,
                m_noiseData.CanBeHeardByLayers);

            if (count < 0)
            {
                // nothing in range
                CurrentlyHeared = false;
                ClearAllCurrentlyHearingListeners(true);
                return;
            }

            bool hearedThisFrame = false;
            for (int i = 0; i < count; i++)
            {
                var c = m_inRangeColliders[i];
                var listener = c.GetComponent<INoiseListener>();
                if (listener == null || !listener.Enabled)
                {
                    continue;
                }

                // check if the listener is occluded by something
                foreach (var t in m_noiseData.RayRoots)
                {
                    Vector3 dir = (listener.Transform.position - t.position);
                    m_cachedRay.origin = t.position;
                    m_cachedRay.direction = dir.normalized;

                    if (m_debugData.Active)
                    {
                        Debug.DrawRay(t.position, dir, m_debugData.OcclusionRaysColor);
                    }

                    bool occluded = false;
                    if (Physics.Raycast(
                        m_cachedRay,
                        out m_cachedHit,
                        dir.magnitude,
                        m_noiseData.OccludedByPhysicalLayers,
                        QueryTriggerInteraction.UseGlobal))
                    {
                        if (m_cachedHit.collider != c)   // hit the listener
                        {
                            occluded = true;
                        }
                    }

                    if (occluded)
                    {
                        continue;
                    }

                    hearedThisFrame = true;

                    m_hearedByThisFrame.Add(listener);
                    if (!m_hearedByListeners.Contains(listener))
                    {
                        m_hearedByListeners.Add(listener);
                        listener.OnStartHearNoise(new NoiseHearedData
                        {
                            Source = this,
                            Direction = -dir.normalized,
                            Distance = dir.magnitude
                        });
                    }
                }
            }

            m_hearedByListeners.RemoveElementIfContained(m_hearedByThisFrame, l => l.OnStopHearNoise(this));

            CurrentlyHeared = hearedThisFrame;
        }

        private void ClearAllCurrentlyHearingListeners(bool invokeEvent)
        {
            if (invokeEvent)
            {
                foreach (var l in m_hearedByListeners)
                {
                    l.OnStopHearNoise(this);
                }
            }

            m_hearedByListeners.Clear();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (!m_debugData.Active)
            {
                return;
            }

            Gizmos.color = CurrentlyHeared ? m_debugData.NoiseSphereColorIfHeared : m_debugData.NoiseSphereColor;

            var velocity = m_debugData.UseVelocityDebug ? m_debugData.DebugVelocity : Velocity;
            float noiseDistance = m_noiseData.NoiseCurve.Evaluate(velocity.magnitude);
            Gizmos.DrawWireSphere(m_noiseData.NoiseEmissionPoint.position, noiseDistance);
        }

        [CustomEditor(typeof(NoiseMaker))]
        private class NoiseMakedEditor : Editor
        {
            private bool m_foldout;

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!Application.isPlaying)
                {
                    return;
                }

                var me = target as NoiseMaker;

                m_foldout = EditorGUILayout.Foldout(m_foldout, "DEBUG", true);
                if (!m_foldout)
                {
                    return;
                }

                EditorGUILayout.LabelField("Heared By:");

                if (me.m_hearedByListeners.Count == 0)
                {
                    EditorGUILayout.LabelField("-> You're hidden <-");
                    return;
                }

                foreach (var l in me.m_hearedByListeners)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"{l.Transform.name} -> ID: {l.Transform.gameObject.GetInstanceID()}");
                        if (GUILayout.Button("Ping"))
                        {
                            EditorGUIUtility.PingObject(l.Transform.gameObject);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
#endif
    }
}
