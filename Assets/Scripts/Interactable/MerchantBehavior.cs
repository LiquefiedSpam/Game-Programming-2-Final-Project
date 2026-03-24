using System.Collections.Generic;
using UnityEngine;

public class MerchantBehavior : InteractableBehavior
{
    [SerializeField] MerchantStallUI stallUI;

    public override void Interact()
    {
        stallUI.Show();
        base.Interact();
    }

    public override void Quit()
    {
        stallUI.Hide();
        base.Quit();
    }
}