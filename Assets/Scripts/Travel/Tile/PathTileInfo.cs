using System.Collections.Generic;
using UnityEngine;

public class PathTileInfo
{
    public const int TILES_PER_TOWN = 3;
    static readonly List<int> indices = new() { 0, 1, 2 };
    public readonly Town TownA;
    public readonly Town TownB;

    readonly TileInfo[] townATiles;
    readonly TileInfo[] townBTiles;

    public PathTileInfo(Town townA, Town townB, TilePrefabs prefabs)
    {
        TownA = townA;
        TownB = townB;

        GameObject[] townATilePrefabs = prefabs.GetPrefabsFor(townA);
        townATiles = new TileInfo[TILES_PER_TOWN];
        for (int i = 0; i < TILES_PER_TOWN; i++)
        {
            GameObject prefab = townATilePrefabs[i];
            townATiles[i] = new(prefab);
        }

        GameObject[] townBTilePrefabs = prefabs.GetPrefabsFor(townB);
        townBTiles = new TileInfo[TILES_PER_TOWN];
        for (int i = 0; i < TILES_PER_TOWN; i++)
        {
            GameObject prefab = townBTilePrefabs[i];
            townBTiles[i] = new(prefab);
        }
    }

    public TileInfo[] GetRandomPathTiles(Town fromTown, Town toTown)
    {
        TileInfo[] ret = new TileInfo[3];
        Town secondTileFrom = Random.value < 0.5f ? fromTown : toTown;

        if (secondTileFrom == fromTown)
        {
            var twoTiles = GetTwoRandomTilesFrom(fromTown);
            ret[0] = twoTiles.Item1;
            ret[1] = twoTiles.Item2;
            ret[2] = GetRandomTileFrom(toTown);
            return ret;
        }
        else
        {
            var twoTiles = GetTwoRandomTilesFrom(toTown);
            ret[0] = GetRandomTileFrom(fromTown);
            ret[1] = twoTiles.Item1;
            ret[2] = twoTiles.Item2;
            return ret;
        }
    }

    TileInfo GetRandomTileFrom(Town town)
    {
        if (town == TownA)
        {
            return townATiles[Random.Range(0, TILES_PER_TOWN)];
        }
        else return townBTiles[Random.Range(0, TILES_PER_TOWN)];
    }

    (TileInfo, TileInfo) GetTwoRandomTilesFrom(Town town)
    {
        List<int> includeIndices = new(indices);
        includeIndices.RemoveAt(Random.Range(0, TILES_PER_TOWN));
        if (Random.value < 0.5f)
        {
            (includeIndices[0], includeIndices[1]) = (includeIndices[1], includeIndices[0]);
        }

        if (town == TownA)
        {
            return (townATiles[includeIndices[0]], townATiles[includeIndices[1]]);
        }
        else return (townBTiles[includeIndices[0]], townBTiles[includeIndices[1]]);
    }

    public bool TryAddTileInfosForTownToList(Town t, ref List<TileInfo> infoList)
    {
        if (TownA == t)
        {
            infoList.AddRange(townATiles);
            return true;
        }
        else if (TownB == t)
        {
            infoList.AddRange(townBTiles);
            return true;
        }
        return false;
    }
}