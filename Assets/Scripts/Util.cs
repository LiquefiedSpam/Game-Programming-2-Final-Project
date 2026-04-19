using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public static string TownToString(this Town town)
    {
        return town switch
        {
            Town.WOODED_KEEP => "Wooded Keep",
            Town.SANDY_STALLS => "Sandy Stalls",
            Town.STONE_SANCTUARY => "Stone Sanctuary",
            _ => ""
        };
    }

    public static void SetAlpha(this Image i, float alpha)
    {
        Color c = i.color;
        c.a = alpha;
        i.color = c;
    }
}