using UnityEngine;

public class ChoiceTile : Tile
{
    [SerializeField] InteractionPoint downInteraction;
    [SerializeField] Transform choicePoint;
    [SerializeField] Transform convergePoint;

    public override async void TraverseTile(bool lastTile = false)
    {
        await Data.MockPlayer.MoveTo(choicePoint.position);

        ChoiceResult choice = await TravelUIManager.Ins.AwaitChoiceResult(tileInfo.UpInteractionInfo, tileInfo.DownInteractionInfo);

        InteractionPoint chosenPoint = choice == ChoiceResult.UP ? upInteraction : downInteraction;
        InteractionInfo chosenInfo = choice == ChoiceResult.UP ? tileInfo.UpInteractionInfo : tileInfo.DownInteractionInfo;

        InteractionResult result = chosenInfo.PassInteraction();
        chosenPoint.SpawnPrefab(result);

        await Data.MockPlayer.MoveTo(chosenPoint.PlayerDestination);
        HandleInteractionResult(result);

        await Data.MockPlayer.MoveTo(convergePoint.position);

        Vector3 endPos = end.position;
        if (lastTile) endPos -= new Vector3(LAST_TILE_X_OFFSET, 0f, 0f);
        await Data.MockPlayer.MoveTo(endPos);

        DayManager.Ins.ConsumeUnit(1);
        OnTileComplete?.Invoke();
    }
}