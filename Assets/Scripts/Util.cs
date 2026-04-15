using System.Collections;
using UnityEngine;

public static class Util
{
    public static void Shuffle<T>(this T[] items)
    {
        int n = items.Length - 1;
        while (n > 1)
        {
            int rand = Random.Range(0, n);
            (items[rand], items[n]) = (items[n], items[rand]);
            n--;
        }
    }
}