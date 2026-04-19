using System;
using System.Threading.Tasks;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected Town town;
    [SerializeField] protected Transform start;
    [SerializeField] protected Transform end;
    [SerializeField] protected InteractionPoint upInteraction;

    public Action OnTileComplete;

    public Town Town => town;
    protected TileInfo tileInfo;

    public virtual async void TraverseTile()
    {
        InteractionResult result = tileInfo.UpInteractionInfo.PassInteraction();
        upInteraction.SpawnPrefab(result);

        await Data.MockPlayer.MoveTo(upInteraction.PlayerDestination);

        // TODO result UI

        await Data.MockPlayer.MoveTo(end.position);

        OnTileComplete?.Invoke();
    }

    public void LinkTileInfo(TileInfo info)
    {
        Debug.Log("Linked tile info");
        tileInfo = info;
    }

    public float GetLength()
    {
        return end.localPosition.x;
    }
}
