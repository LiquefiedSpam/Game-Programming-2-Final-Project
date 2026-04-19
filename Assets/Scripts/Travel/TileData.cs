using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pathing/TileNode", menuName = "TileData")]
public class TileData : ScriptableObject
{
    [SerializeField] private Sprite mysteryIcon;
    [SerializeField] private Sprite tileIcon;

    [System.Serializable]
    public class InteractionPoint
    {
        public Vector2 normalizePosition;
        public float size;
    }

    [SerializeField] private List<InteractionPoint> interactionPoints;

    public Sprite MysteryIcon => mysteryIcon;
    public Sprite TileIcon => tileIcon;
    public List<InteractionPoint> InteractionPoints => interactionPoints;
}
