using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static Dictionary<Town, Transform> TownTeleports;
    public static Dictionary<Town, PlayerStall> TownPlayerStalls;
    public static Dictionary<Town, InnBehavior> TownInns;
    public static CustomerReactions CustomerReactions;

    public static PlayerController Player;
    public static Town CurrentTown { get; private set; } = Town.WOODED_KEEP;
    public static PlayerStall ClosestPlayerStall => TownPlayerStalls[CurrentTown];
    public static InnBehavior ClosestInn => TownInns[CurrentTown];


    static void SetCurrentTown(Town currentTown)
    {
        CurrentTown = currentTown;
    }

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
            if (t.name.ToLower().Contains("wood"))
            {
                TownTeleports[Town.WOODED_KEEP] = t.transform;
            }
            else if (t.name.ToLower().Contains("sand"))
            {
                TownTeleports[Town.SANDY_STALLS] = t.transform;
            }
            else if (t.name.ToLower().Contains("stone"))
            {
                TownTeleports[Town.STONE_SANCTUARY] = t.transform;
            }
        }

        InnBehavior[] inns = Object.FindObjectsByType<InnBehavior>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        TownInns = new();
        foreach (var inn in inns)
        {
            TownInns[inn.InnTown] = inn;
        }

        Player = GameObject.FindFirstObjectByType<PlayerController>();

        TeleportToTown.OnTownChanged += SetCurrentTown;
        PlayerController.OnForceNextDay += SetCurrentTown;
    }
}