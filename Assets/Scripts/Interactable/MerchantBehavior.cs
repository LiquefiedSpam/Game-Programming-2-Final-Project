using System.Collections.Generic;
using UnityEngine;

public class MerchantBehavior : InteractableBehavior
{
    [SerializeField] MerchantStallUI stallUI;

    public override void Interact()
    {
        // this is so messy must fix
        // UIManager.Ins.ShowMerchantInventory(true, itemsForSale);
        stallUI.Show();
        base.Interact();
    }

    public override void Quit()
    {
        // UIManager.Ins.ShowMerchantInventory(false);
        stallUI.Hide();
        base.Quit();
    }
}