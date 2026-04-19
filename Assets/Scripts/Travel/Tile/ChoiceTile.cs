using UnityEngine;

public class ChoiceTile : Tile
{
    [SerializeField] InteractionPoint downInteraction;
    [SerializeField] Transform choicePoint;
    [SerializeField] Transform convergePoint;

    public override async void TraverseTile()
    {
        await Data.MockPlayer.MoveTo(choicePoint.position);

        ChoiceResult choice = await TravelUIManager.Ins.AwaitChoiceResult(tileInfo.UpInteractionInfo, tileInfo.DownInteractionInfo);

        InteractionPoint chosenPoint = choice == ChoiceResult.UP ? upInteraction : downInteraction;
        InteractionInfo chosenInfo = choice == ChoiceResult.UP ? tileInfo.UpInteractionInfo : tileInfo.DownInteractionInfo;

        InteractionResult result = chosenInfo.PassInteraction();
        chosenPoint.SpawnPrefab(result);

        await Data.MockPlayer.MoveTo(chosenPoint.PlayerDestination);
        TravelUIManager.Ins.ShowInteractionResult(result);

        await Data.MockPlayer.MoveTo(convergePoint.position);
        await Data.MockPlayer.MoveTo(end.position);

        OnTileComplete?.Invoke();
    }
}