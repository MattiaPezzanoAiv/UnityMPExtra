namespace MP.Extensions
{
    using UnityEngine;

    public static class VectorExtensions
    {
        public static bool AlmostZero(this Vector3 v, float floatTolerance = 0.001f)
        {
            return v.x.AlmostZero() && v.y.AlmostZero() && v.z.AlmostZero();
        }

        public static bool AlmostZero(this Vector2 v, float floatTolerance = 0.001f)
        {
            return v.x.AlmostZero() && v.y.AlmostZero();
        }
    }
}
