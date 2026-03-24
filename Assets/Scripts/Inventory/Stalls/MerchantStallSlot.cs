using System.Collections;
using UnityEngine;

public class MerchantStallSlot : StallSlot
{
    [SerializeField] public int cooldownInHours;
    public bool CoolingDown => coolingDown;
    bool coolingDown;

    protected override void SlotClicked()
    {
        PlayerInventory.Instance.TryPurchaseItem(itemPrice, heldItem, itemAmount);
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield break; // implement later
    }
}