namespace MP.Unity.Physics
{
    using MP.Unity.Events;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(CollisionEventForwarder))]
    public class CollisionEventForwarderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var me = target as CollisionEventForwarder;

            var collider = me.GetComponent<Collider>();
            if (!collider)
            {
                EditorGUILayout.HelpBox("A collider has not been found. Be sure to add one in order to make this work.", MessageType.Warning);
                return;
            }

            if(collider.isTrigger)
            {
                EditorGUILayout.HelpBox("A trigger collider will not generate a collision event.", MessageType.Warning);
                return;
            }

            base.OnInspectorGUI();
        }
    }
#endif

    public class CollisionEventForwarder : MonoBehaviour
    {
        public UnityEventCollision CollisionEnter;

        public UnityEventCollision CollisionExit;

        public UnityEventCollision CollisionStay;

        private void OnCollisionEnter(Collision collision)
        {
            CollisionEnter?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            CollisionExit?.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            CollisionStay?.Invoke(collision);
        }
    }
}
