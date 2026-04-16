using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileUI : MonoBehaviour
{
    [SerializeField] private Image tileDetailImage;
    [SerializeField] private RectTransform container;

    [SerializeField] private List<RectTransform> interactionLocations;

    private TileData data;

    public void Initialize(TileData tileData)
    {
        data = tileData;
        tileDetailImage.sprite = data.MysteryIcon;
    }

    public void DiscoverTile()
    {
        tileDetailImage.sprite = data.TileIcon;
        //find interaction points for tile and mark them visible (defualt will be neutral with 50% chance)
    }
}
