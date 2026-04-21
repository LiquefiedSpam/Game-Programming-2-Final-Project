using System;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo
{
    public InteractionInfo UpInteractionInfo;
    public InteractionInfo DownInteractionInfo; // will be null if only 1 interaction point
    public ScriptableTileData Data { get; private set; }

    public TileInfo(ScriptableTileData tileData)
    {
        Data = tileData;
        UpInteractionInfo = new();
        DownInteractionInfo = new();
    }

    //try to get a random interaction below 60% chance.
    public bool TryAddRandomInteractionToList(ref List<InteractionInfo> list)
    {
        bool success = false;

        List<InteractionInfo> eligibleInteractions = new List<InteractionInfo>();
        if (UpInteractionInfo.MarauderChance < 6)
            eligibleInteractions.Add(UpInteractionInfo);

        if (DownInteractionInfo != null)
        {
            if (DownInteractionInfo.MarauderChance < 6)
                eligibleInteractions.Add(DownInteractionInfo);
        }

        if (eligibleInteractions.Count == 0)
            return success;

        success = true;
        if (eligibleInteractions.Count == 1)
        {
            list.Add(eligibleInteractions[0]);
        }
        else
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                list.Add(eligibleInteractions[0]);
            }
            else
            {
                list.Add(eligibleInteractions[1]);
            }
        }
        return success;
    }
}