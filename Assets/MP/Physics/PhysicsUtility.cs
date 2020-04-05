namespace MP.Unity.Physics
{
    using UnityEngine;
    using System;

    public static class PhysicsUtility
    {
        public static int OverlapSphereNonAlloc(
            Vector3 position, 
            float radius, 
            ref Collider[] colliders, 
            LayerMask layerMask = default, 
            QueryTriggerInteraction qti = QueryTriggerInteraction.UseGlobal)
        {
            if(colliders.Length == 0)
            {
                // length can't be zero
                Array.Resize(ref colliders, 2);
            }

            while(true)
            {
                if(layerMask == default)
                {
                    layerMask = ~0;
                }

                int result = Physics.OverlapSphereNonAlloc(position, radius, colliders, layerMask, qti);
                
                if(result < colliders.Length)
                {
                    // bounds not exceeded, all is good
                    return result;
                }

                // maximum size reached, increase array bounds
                Array.Resize(ref colliders, colliders.Length * 2);
            }
        }

        public static int RaycastNonAlloc(
            Ray ray,
            ref RaycastHit[] hits,
            float distance,
            LayerMask mask = default,
            QueryTriggerInteraction qti = QueryTriggerInteraction.UseGlobal)
        {
            if (hits.Length == 0)
            {
                // length can't be zero
                Array.Resize(ref hits, 2);
            }

            while (true)
            {
                if (mask == default)
                {
                    mask = ~0;
                }

                int result = Physics.RaycastNonAlloc(ray, hits, distance, mask, qti);

                if (result < hits.Length)
                {
                    // bounds not exceeded, all is good
                    return result;
                }

                // maximum size reached, increase array bounds
                Array.Resize(ref hits, hits.Length * 2);
            }
        }
    }
}
