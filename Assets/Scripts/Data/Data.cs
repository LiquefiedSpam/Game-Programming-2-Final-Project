using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static CustomerReactions CustomerReactions;
    public static Dictionary<Town, Transform> TownTeleports;
    public static Dictionary<Town, PlayerStall> TownPlayerStalls;
    public static Town CurrentTown = Town.TOWN_1;
    public static PlayerStall ClosestPlayerStall => TownPlayerStalls[CurrentTown];


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        CustomerReactions = Resources.Load<CustomerReactions>("CustomerReactions");

        PlayerStall[] playerStalls = Object.FindObjectsByType<PlayerStall>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        TownPlayerStalls = new();
        foreach (var ps in playerStalls)
        {
            TownPlayerStalls[ps.Town] = ps;
        }

        GameObject[] tps = GameObject.FindGameObjectsWithTag("Teleport");
        TownTeleports = new();
        foreach (var t in tps)
        {
            if (t.name.Contains("1"))
            {
                TownTeleports[Town.TOWN_1] = t.transform;
            }
            else if (t.name.Contains("2"))
            {
                TownTeleports[Town.TOWN_2] = t.transform;
            }
            else if (t.name.Contains("3"))
            {
                TownTeleports[Town.TOWN_3] = t.transform;
            }
        }
    }
}