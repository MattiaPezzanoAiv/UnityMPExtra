namespace MP.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public static class CollectionsExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            T temp;
            int nextIdx;
            for (int i = 0; i < list.Count; i++)
            {
                nextIdx = UnityEngine.Random.Range(i, list.Count);

                temp = list[i];
                list[i] = list[nextIdx];
                list[nextIdx] = temp;
            }
        }

        public static void Shuffle(this Array array)
        {
            object temp;
            int nextIdx;
            for (int i = 0; i < array.Length; i++)
            {
                nextIdx = UnityEngine.Random.Range(i, array.Length);

                temp = array.GetValue(i);
                array.SetValue(array.GetValue(nextIdx), i);
                array.SetValue(temp, nextIdx);
            }
        }

        public static void RemoveElementIfContained<T>(this HashSet<T> hashSet, HashSet<T> other, Action<T> onRemove = null)
        {
            HashSet<T> temp = new HashSet<T>();

            foreach (var i in hashSet)
            {
                if (!other.Contains(i))
                {
                    temp.Add(i);
                }
            }

            foreach (var i in temp)
            {
                onRemove?.Invoke(i);
                hashSet.Remove(i);
            }
        }
    }
}
