using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PathManager : MonoBehaviour
{

    public static PathManager Ins { get; private set; }
    const float FADE_TIME = 0.4f;

    [SerializeField] AllTileData tilePrefabs;
    [SerializeField] Transform playerStart;
    [SerializeField] Transform tile1Parent;
    [SerializeField] Transform tile2Parent;
    [SerializeField] Transform tile3Parent;
    [SerializeField] CameraFollow camFollow;

    Tile[] activeTiles;
    TileInfo[] activeTileInfo;
    Town activeDestination;

    public Vector3 PlayerStart => playerStart.position;
    public Dictionary<Town, Dictionary<Town, PathTileInfo>> Tiles;

    public static Action<Town> OnTownChanged;

    private void Awake()
    {
        if (Ins != null && Ins != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Ins = this;
        }
    }

    void Start()
    {
        InitTiles();
        activeTiles = new Tile[3];
        activeTileInfo = new TileInfo[3];
    }

    public async Task BuildPath(Town fromTown, Town toTown)
    {
        PathTileInfo pathInfo = Tiles[fromTown][toTown];
        activeTileInfo = pathInfo.GetRandomPathTiles(fromTown, toTown);

        activeTiles[0] = CreateTile(activeTileInfo[0], tile1Parent);
        activeTiles[1] = CreateTile(activeTileInfo[1], tile2Parent);
        activeTiles[2] = CreateTile(activeTileInfo[2], tile3Parent);

        activeDestination = toTown;
    }

    public void StartPlayerOnPath()
    {
        Data.MockPlayer.gameObject.SetActive(true);
        Data.MockPlayer.SetPos(PlayerStart);
        camFollow.FollowMockPlayer();

        activeTiles[0].OnTileComplete += () => activeTiles[1].TraverseTile();
        activeTiles[1].OnTileComplete += () => activeTiles[2].TraverseTile(true);
        activeTiles[2].OnTileComplete += EndPlayerOnPath;

        _ = UIManager.Ins.FadeAlpha(FADE_TIME, 0f);

        activeTiles[0].TraverseTile();
    }

    public async void EndPlayerOnPath()
    {
        if (activeTiles[2] != null)
        {
            activeTiles[2].OnTileComplete -= EndPlayerOnPath;
        }

        await UIManager.Ins.FadeAlpha(FADE_TIME, 1f);

        OnTownChanged?.Invoke(activeDestination);

        Data.MockPlayer.gameObject.SetActive(false);
        Data.Player.SetLocation(Data.TownTeleports[activeDestination].position);
        Data.Player.SetMovementDisable(false);
        camFollow.FollowPlayer();

        _ = UIManager.Ins.FadeAlpha(FADE_TIME, 0f);
        DestroyCurrentTilesAfter(1f);
    }

    //returns a random neutral or positive interaction from the passed in town's path units.
    public InteractionInfo GetRandomInteraction(Town townFrom, Town townTo)
    {
        List<TileInfo> allTiles = new();

        Tiles[townFrom][townTo].TryAddTileInfosForTownToList(townFrom, ref allTiles);

        List<InteractionInfo> allInteractions = new();
        foreach (var tileInfo in allTiles)
        {
            tileInfo.TryAddRandomInteractionToList(ref allInteractions);
        }

        return allInteractions[UnityEngine.Random.Range(0, allInteractions.Count)];
    }

    Tile CreateTile(TileInfo tileInfo, Transform parent)
    {
        GameObject tileObj = Instantiate(tileInfo.Data.Prefab, parent);
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

    void DestroyCurrentTilesAfter(float delay)
    {
        for (int i = tile1Parent.childCount - 1; i >= 0; i--) Destroy(tile1Parent.GetChild(i).gameObject, delay);
        for (int i = tile2Parent.childCount - 1; i >= 0; i--) Destroy(tile2Parent.GetChild(i).gameObject, delay);
        for (int i = tile3Parent.childCount - 1; i >= 0; i--) Destroy(tile3Parent.GetChild(i).gameObject, delay);
        for (int i = 0; i < activeTiles.Length; i++) activeTiles[i] = null;
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