using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] TilePrefabs tilePrefabs;
    [SerializeField] Transform playerStart;
    [SerializeField] Transform tile1Parent;
    [SerializeField] Transform tile2Parent;
    [SerializeField] Transform tile3Parent;
    [SerializeField] CameraFollow camFollow;

    Tile[] activeTiles;
    TileInfo[] activeTileInfo;
    Town activeDestination;

    public Vector3 PlayerStart => playerStart.position;
    public Action OnPathCompleted;

    public Dictionary<Town, Dictionary<Town, PathTileInfo>> Tiles;

    void Start()
    {
        InitTiles();
        activeTiles = new Tile[3];
        activeTileInfo = new TileInfo[3];
    }

    public async Task BuildPath(Town fromTown, Town toTown)
    {
        DestroyCurrentTiles();

        PathTileInfo pathInfo = Tiles[fromTown][toTown];
        activeTileInfo = pathInfo.GetRandomPathTiles(fromTown, toTown);

        activeTiles[0] = CreateTile(activeTileInfo[0], tile1Parent);
        activeTiles[1] = CreateTile(activeTileInfo[1], tile2Parent);
        activeTiles[2] = CreateTile(activeTileInfo[2], tile3Parent);

        activeDestination = toTown;
    }

    public void StartPlayerOnPath()
    {
        // TODO: TP player to start, fade UI
        Data.Player.SetMovementDisable(true);
        Data.MockPlayer.gameObject.SetActive(true);
        Data.MockPlayer.SetPos(PlayerStart);
        camFollow.FollowMockPlayer();

        activeTiles[0].OnTileComplete += () => activeTiles[1].TraverseTile();
        activeTiles[1].OnTileComplete += () => activeTiles[2].TraverseTile();
        activeTiles[2].OnTileComplete += EndPlayerOnPath;
        activeTiles[0].TraverseTile();
    }

    public void EndPlayerOnPath()
    {
        // TODO: TP player to town, fade UI
        OnPathCompleted?.Invoke();

        Data.MockPlayer.gameObject.SetActive(false);
        Data.Player.SetLocation(Data.TownTeleports[activeDestination].position);
        Data.Player.SetMovementDisable(false);
        camFollow.FollowPlayer();
    }

    public InteractionInfo GetRandomInteraction(Town town)
    {
        List<TileInfo> allTiles = new();

        (Town, Town) otherTowns = GetOtherTowns(town);
        Tiles[town][otherTowns.Item1].TryAddTileInfosForTownToList(town, ref allTiles);
        Tiles[town][otherTowns.Item2].TryAddTileInfosForTownToList(town, ref allTiles);

        List<InteractionInfo> allInteractions = new();
        foreach (var tileInfo in allTiles)
        {
            tileInfo.TryAddRandomInteractionToList(ref allInteractions);
        }

        return allInteractions[UnityEngine.Random.Range(0, allInteractions.Count)];
    }

    Tile CreateTile(TileInfo tileInfo, Transform parent)
    {
        GameObject tileObj = Instantiate(tileInfo.Prefab, parent);
        tileObj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (tileObj.TryGetComponent<Tile>(out var tile))
        {
            tile.LinkTileInfo(tileInfo);
            return tile;
        }
        else
        {
            Destroy(tileObj);
            Debug.LogWarning("Failed to instantiate tile, Tile script missing");
            return null;
        }
    }

    void DestroyCurrentTiles()
    {
        for (int i = 0; i < activeTiles.Length; i++)
        {
            if (activeTiles[i] != null)
            {
                var obj = activeTiles[i];
                activeTiles[i] = null;
                Destroy(obj);
            }
        }
    }

    void InitTiles()
    {
        Tiles = new()
        {
            [Town.WOODED_KEEP] = new(),
            [Town.SANDY_STALLS] = new(),
            [Town.STONE_SANCTUARY] = new()
        };

        PathTileInfo town_1_2_info = new(Town.WOODED_KEEP, Town.SANDY_STALLS, tilePrefabs);
        Tiles[Town.WOODED_KEEP][Town.SANDY_STALLS] = town_1_2_info;
        Tiles[Town.SANDY_STALLS][Town.WOODED_KEEP] = town_1_2_info;

        PathTileInfo town_2_3_info = new(Town.SANDY_STALLS, Town.STONE_SANCTUARY, tilePrefabs);
        Tiles[Town.SANDY_STALLS][Town.STONE_SANCTUARY] = town_2_3_info;
        Tiles[Town.STONE_SANCTUARY][Town.SANDY_STALLS] = town_2_3_info;

        PathTileInfo town_1_3_info = new(Town.WOODED_KEEP, Town.STONE_SANCTUARY, tilePrefabs);
        Tiles[Town.WOODED_KEEP][Town.STONE_SANCTUARY] = town_1_3_info;
        Tiles[Town.STONE_SANCTUARY][Town.WOODED_KEEP] = town_1_3_info;
    }

    (Town, Town) GetOtherTowns(Town town)
    {
        return town switch
        {
            Town.SANDY_STALLS => (Town.STONE_SANCTUARY, Town.WOODED_KEEP),
            Town.WOODED_KEEP => (Town.SANDY_STALLS, Town.STONE_SANCTUARY),
            _ => (Town.WOODED_KEEP, Town.SANDY_STALLS)
        };
    }
}