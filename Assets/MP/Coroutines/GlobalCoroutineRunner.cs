namespace MP.Coroutines
{
    using UnityEngine;

    public class GlobalCoroutineRunner : MonoBehaviour
    {
        private static GlobalCoroutineRunner s_instance;

        public static GlobalCoroutineRunner Instance
        {
            get
            {
                if (!s_instance)
                {
                    s_instance = GameObject.FindObjectOfType<GlobalCoroutineRunner>();
                    
                    if(!s_instance)
                    {
                        s_instance = new GameObject("Global Coroutine Runner").AddComponent<GlobalCoroutineRunner>();
                    }
                }

                return s_instance;
            }
        }
    }
}
