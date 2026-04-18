using TMPro;
using UnityEngine;

public class PlayerStallUI : SlotGroupUI
{
    [SerializeField] TMP_Text stallTitle;

    public override void ShowSlots(SlotGroup slots)
    {
        stallTitle.text = $"Your Stall ({Util.TownToString(Data.CurrentTown)})";
        base.ShowSlots(slots);
    }
}