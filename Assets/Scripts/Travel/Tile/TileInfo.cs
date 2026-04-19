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

    public bool TryAddRandomInteractionToList(ref List<InteractionInfo> list)
    {
        if (Random.value < 0.5f)
        {
            if (UpInteractionInfo == null) return false;
            list.Add(UpInteractionInfo);
            return true;
        }
        else
        {
            if (DownInteractionInfo == null) return false;
            list.Add(DownInteractionInfo);
            return true;
        }
    }
}