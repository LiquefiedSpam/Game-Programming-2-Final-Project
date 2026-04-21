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
        DownInteractionInfo = tileData.ChoiceTile ? new() : null;
    }
    public bool AddRandomInteractionToList(ref List<InteractionInfo> list)
    {
        if (UpInteractionInfo != null) list.Add(UpInteractionInfo);
        if (DownInteractionInfo != null) list.Add(DownInteractionInfo);
        return list.Count > 0;
    }
}