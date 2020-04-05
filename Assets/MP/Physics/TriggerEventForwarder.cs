namespace MP.Unity.Physics
{
    using UnityEngine;
    using MP.Unity.Events;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TriggerEventForwarder))]
    public class TriggerEventForwarderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var me = target as TriggerEventForwarder;

            if(!me.GetComponent<Collider>())
            {
                EditorGUILayout.HelpBox("A collider has not been found. Be sure to add one in order to make this work.", MessageType.Warning);
            }

            base.OnInspectorGUI();
        }
    }
#endif

    public class TriggerEventForwarder : MonoBehaviour
    {
        public UnityEventCollider TriggerEnter;

        public UnityEventCollider TriggerExit;

        public UnityEventCollider TriggerStay;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TriggerStay?.Invoke(other);
        }
    }
}
