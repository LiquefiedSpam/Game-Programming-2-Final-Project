using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects/TilePrefabs", menuName = "Tile Prefabs")]
public class TilePrefabs : ScriptableObject
{
    [SerializeField] GameObject[] grassPaths;
    [SerializeField] GameObject[] sandPaths;
    [SerializeField] GameObject[] stonePaths;

    public GameObject[] GetPrefabsFor(Town town)
    {
        return town switch
        {
            Town.WOODED_KEEP => grassPaths,
            Town.SANDY_STALLS => sandPaths,
            Town.STONE_SANCTUARY => stonePaths,
            _ => null
        };
    }
}