namespace MP.Extensions
{
    using UnityEngine;

    public static class MonoBehaviourExtensions
    {
        public static void SafeStopCoroutine(this MonoBehaviour mb, ref Coroutine coroutine)
        {
            if(coroutine != null)
            {
                mb.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
