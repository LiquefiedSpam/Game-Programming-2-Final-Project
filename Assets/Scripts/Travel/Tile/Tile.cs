using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public const float LAST_TILE_X_OFFSET = 20f;

    [SerializeField] protected Town town;
    [SerializeField] protected Transform start;
    [SerializeField] protected Transform end;
    [SerializeField] protected InteractionPoint upInteraction;

    public Action OnTileComplete;

    public Town Town => town;
    protected TileInfo tileInfo;

    public virtual async void TraverseTile(bool lastTile = false)
    {
        InteractionResult result = tileInfo.UpInteractionInfo.PassInteraction();
        upInteraction.SpawnPrefab(result);

        await Data.MockPlayer.MoveTo(upInteraction.PlayerDestination);
        HandleInteractionResult(result);

        Vector3 endPos = end.position;
        if (lastTile) endPos -= new Vector3(LAST_TILE_X_OFFSET, 0f, 0f);
        await Data.MockPlayer.MoveTo(endPos);

        DayManager.Ins.ConsumeUnit(1);
        OnTileComplete?.Invoke();
    }

    public void LinkTileInfo(TileInfo info)
    {
        tileInfo = info;
    }

    protected void HandleInteractionResult(InteractionResult result)
    {
        if (result == InteractionResult.SAFE)
        {
            TravelUIManager.Ins.ShowSafeResult();
        }
        else
        {
            int stolenAmount = UnityEngine.Random.Range(Data.MarauderStealRange.x, Data.MarauderStealRange.y);
            stolenAmount = Mathf.Min(stolenAmount, Inventory.Ins.GetTotalItemCount());
            if (stolenAmount > 0)
            {
                Inventory.Ins.RemoveRandomItems(stolenAmount);
            }
            TravelUIManager.Ins.ShowMarauderResult(stolenAmount);
        }
    }

    public float GetLength()
    {
        return end.localPosition.x;
    }
}
