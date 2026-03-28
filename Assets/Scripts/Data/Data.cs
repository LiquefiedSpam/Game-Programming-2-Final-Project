using UnityEngine;

public static class Data
{
    public static CustomerReactions CustomerReactions;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        CustomerReactions = Resources.Load<CustomerReactions>("CustomerReactions");
    }
}