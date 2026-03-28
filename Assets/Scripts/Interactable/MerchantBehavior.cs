using System.Collections.Generic;
using UnityEngine;

public class MerchantBehavior : InteractableBehavior
{
    [SerializeField] MerchantStallUI stallUI;
    [SerializeField] AudioSource audioSource;

    public override void Interact()
    {
        audioSource.Play();
        stallUI.Show();
        base.Interact();
    }

    public override void Quit()
    {
        audioSource.Stop();
        stallUI.Hide();
        base.Quit();
    }
}