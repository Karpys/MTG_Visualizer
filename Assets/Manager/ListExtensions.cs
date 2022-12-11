using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Manager
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}