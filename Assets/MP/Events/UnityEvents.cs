namespace MP.Unity.Events
{
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public class UnityEventCollision : UnityEvent<Collision> { }
    
    [System.Serializable]
    public class UnityEventCollider : UnityEvent<Collider> { }

    [System.Serializable]
    public class UnityEventCollision2D : UnityEvent<Collision2D> { }

    [System.Serializable]
    public class UnityEventCollider2D : UnityEvent<Collider2D> { }
}
