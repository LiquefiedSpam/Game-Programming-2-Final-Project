using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static Vector2Int MarauderStealRange = new(1, 5); // max exclusive
    public static Dictionary<Town, Transform> TownTeleports;
    public static Dictionary<Town, PlayerStall> TownPlayerStalls;
    public static Dictionary<Town, InnBehavior> TownInns;
    public static CustomerReactions CustomerReactions;
    public static TileDisplaySO TileDisplaySO;

    public static PlayerController Player;
    public static MockPlayerController MockPlayer;
    public static Town CurrentTown { get; private set; } = Town.WOODED_KEEP;
    public static PlayerStall ClosestPlayerStall => TownPlayerStalls[CurrentTown];
    public static InnBehavior ClosestInn => TownInns[CurrentTown];

    public static BeerData[] Beers;
    public static Dictionary<BeerSize, BeerData> BeersBySize;

    static void SetCurrentTown(Town currentTown)
    {
        CurrentTown = currentTown;
    }

    public static void Init()
    {
        CurrentTown = Town.WOODED_KEEP;

        CustomerReactions = Resources.Load<CustomerReactions>("CustomerReactions");
        TileDisplaySO = Resources.Load<TileDisplaySO>("TileDisplaySO");
        Beers = Resources.LoadAll<BeerData>("Beers");

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

        BeersBySize = new();
        foreach (var beer in Beers)
            BeersBySize[beer.size] = beer;

        Player = GameObject.FindFirstObjectByType<PlayerController>();

        PathManager.OnTownChanged += SetCurrentTown;
        PlayerController.OnForceNextDay += SetCurrentTown;
    }
}