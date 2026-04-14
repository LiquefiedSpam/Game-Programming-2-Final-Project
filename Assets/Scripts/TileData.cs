using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Pathing/TileNode", menuName = "TileData")]
public class TileData : ScriptableObject
{
    [SerializeField] private Image mysteryIcon;
    [SerializeField] private Image tileIcon;
    //circle for potential interaction zone (zone1)
    //circle for potential interaction zone (zone2)

    private Image currentIcon;

    private void Awake()
    {
        currentIcon = mysteryIcon;
    }

    void DiscoveryTile()
    {
        currentIcon = tileIcon;
        //show spots for interaction
        //give details for the paths based on what is stored in the MarauderCampData
    }
}
