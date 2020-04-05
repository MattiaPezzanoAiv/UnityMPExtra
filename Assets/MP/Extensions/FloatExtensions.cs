namespace MP.Extensions
{
    using UnityEngine;

    public static class FloatExtensions
    {
        public static bool AlmostEqual(this float f, float other, float tolerance = 0.001f)
        {
            return Mathf.Abs(f - other) <= tolerance;
        }

        public static bool AlmostZero(this float f, float tolerance = 0.001f)
        {
            return f.AlmostEqual(0f, tolerance);
        }

        public static bool Between(this float f, float min, float max)
        {
            return f >= min && f <= max;
        }
    }
}
