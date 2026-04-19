using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects/AllTileData", menuName = "AllTileData")]
public class AllTileData : ScriptableObject
{
    [SerializeField] ScriptableTileData[] grassTiles = Array.Empty<ScriptableTileData>();
    [SerializeField] ScriptableTileData[] sandTiles = Array.Empty<ScriptableTileData>();
    [SerializeField] ScriptableTileData[] stoneTiles = Array.Empty<ScriptableTileData>();

    public ScriptableTileData[] GetTilesFor(Town town)
    {
        return town switch
        {
            Town.WOODED_KEEP => grassTiles,
            Town.SANDY_STALLS => sandTiles,
            Town.STONE_SANCTUARY => stoneTiles,
            _ => null
        };
    }
}

[Serializable]
public class ScriptableTileData
{
    [SerializeField] public GameObject Prefab;
    [SerializeField] public Sprite Sprite;
    [SerializeField] public bool ChoiceTile;
    [SerializeField] public Vector2 NormalizedUpPoint;

    [Tooltip("This value is ignored if not a choice tile")]
    [SerializeField] public Vector2 NormalizedDownPoint;
}