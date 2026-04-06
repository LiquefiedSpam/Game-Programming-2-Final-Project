using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance => instance;
    public static TileManager instance;

    public int travelPathLength = 3;
    public Transform travelPathObject;

    [SerializeField] private Vector3 nextSpawnPoint;

    public void GenerateTravelPath(GameObject[] startingTown, GameObject[] endingTown)
    {
        List<GameObject> startTiles = new List<GameObject>(startingTown);
        List<GameObject> endTiles = new List<GameObject>(endingTown);

        GameObject tilePrefab;

        for (int i = 0; i < travelPathLength; i++)
        {
            if (i == 0)
            {
                int index = Random.Range(0, startTiles.Count);
                tilePrefab = startTiles[index];
                startTiles.RemoveAt(index);
            }
            else if (i != travelPathLength - 1)
            {
                List<GameObject> combinedTiles = new List<GameObject>();
                combinedTiles.AddRange(startTiles);
                combinedTiles.AddRange(endTiles);

                int index = Random.Range(0, combinedTiles.Count);
                tilePrefab = combinedTiles[index];

                if (startTiles.Contains(tilePrefab))
                {
                    startTiles.Remove(tilePrefab);
                }
                else
                {
                    endTiles.Remove(tilePrefab);
                }
            }
            else
            {
                int index = Random.Range(0, endTiles.Count);
                tilePrefab = endTiles[index];
                endTiles.RemoveAt(index);
            }

            GameObject newTile = Instantiate(tilePrefab, nextSpawnPoint, Quaternion.identity);
            newTile.transform.SetParent(travelPathObject);

            Tile tileScript = newTile.GetComponent<Tile>();
            nextSpawnPoint += new Vector3(tileScript.GetLength(), 0, 0);
        }
    }
}
