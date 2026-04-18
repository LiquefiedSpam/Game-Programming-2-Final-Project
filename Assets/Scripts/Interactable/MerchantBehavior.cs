using System.Collections.Generic;
using UnityEngine;

public class MerchantBehavior : InteractableBehavior
{
    [SerializeField] MerchantStall listings;
    [SerializeField] AudioSource audioSource;

    public override void Interact()
    {
        audioSource.Play();
        // InventoryDisplayManager.Ins.ShowMerchantStall(listings);
        base.Interact();
    }

    public override void Quit()
    {
        audioSource.Stop();
        InventoryDisplayManager.Ins.HideMerchantStall();
        base.Quit();
    }
}