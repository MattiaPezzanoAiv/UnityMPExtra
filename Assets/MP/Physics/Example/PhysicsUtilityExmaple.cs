namespace MP.Examples
{
    using UnityEngine;
    using MP.Unity.Physics;

    public class PhysicsUtilityExmaple : MonoBehaviour
    {
        public Transform m_point;

        private Collider[] m_cols = new Collider[0];

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M)) 
            {
                int result = PhysicsUtility.OverlapSphereNonAlloc(m_point.position, 10f, ref m_cols, 1 << LayerMask.NameToLayer("Default"));
                Debug.Log(result);
            }
        }
    }
}
