using TMPro;
using UnityEngine;

public class PlayerStallUI : SlotGroupUI
{
    [SerializeField] TMP_Text stallTitle;

    public override void ShowSlots(SlotGroup slots)
    {
        stallTitle.text = $"Your Stall ({Data.CurrentTown.TownToString()})";
        base.ShowSlots(slots);
    }
}