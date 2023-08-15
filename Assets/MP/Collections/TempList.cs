#define ENABLE_EXTRA_SAFETY_CHECKS

using System.Collections.Generic;
using UnityEngine;

namespace MP.Collections
{
    public class TempList
    {
        /// <summary>
        /// Get a new temp list or a reused one holding objects of type T.
        /// </summary>
        public static TempList<T> Claim<T>()
        {
            return TempList<T>.Claim();
        }
    }

    public class TempList<T> : List<T>, System.IDisposable
    {
        // Set the default capacity to 4 when we create a temp list as most likely we're gonna use it and the default capacity is 0
        private const int c_defaultCapacity = 4;
        private static readonly List<TempList<T>> s_pool = new List<TempList<T>>();
        public static TempList<T> Claim()
        {
            foreach (var item in s_pool)
            {
                if(!item.m_inUse)
                {
                    item.m_inUse = true;
                    return item;
                }
            }

            return new TempList<T>(c_defaultCapacity) { m_inUse = true };
        }

        private TempList(int capacity) : base(capacity) { }

        private bool m_inUse;

        public void Dispose()
        {
#if ENABLE_EXTRA_SAFETY_CHECKS
            if(!m_inUse)
            {
                Debug.LogWarning($"TempList<{typeof(T).Name}>.Dispose() called when collection was not in use.");
            }
#endif
            m_inUse = false;
            Clear();
        }
    }
}