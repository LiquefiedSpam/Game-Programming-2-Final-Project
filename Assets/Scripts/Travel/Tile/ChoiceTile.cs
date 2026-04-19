using System.Threading.Tasks;
using UnityEngine;

public class ChoiceTile : Tile
{
    [SerializeField] InteractionPoint downInteraction;
    [SerializeField] Transform choicePoint;
    [SerializeField] Transform convergePoint;

    public override async void TraverseTile()
    {
        await Data.MockPlayer.MoveTo(choicePoint.position);

        // TODO await choice UI
        // for now we pick randomly
        InteractionPoint chosenPoint;
        InteractionResult result;
        if (Random.value < 0.5f)
        {
            chosenPoint = upInteraction;
            result = tileInfo.UpInteractionInfo.PassInteraction();
            upInteraction.SpawnPrefab(result);
        }
        else
        {
            chosenPoint = downInteraction;
            result = tileInfo.DownInteractionInfo.PassInteraction();
            downInteraction.SpawnPrefab(result);
        }

        await Data.MockPlayer.MoveTo(chosenPoint.PlayerDestination);

        // TODO result UI

        await Data.MockPlayer.MoveTo(convergePoint.position);
        await Data.MockPlayer.MoveTo(end.position);

        OnTileComplete?.Invoke();
    }
}