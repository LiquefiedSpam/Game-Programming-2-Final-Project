using UnityEngine;

public class PlayerStallBehavior : InteractableBehavior
{
    [SerializeField] PlayerStall playerStall;
    public override void Interact()
    {
        InventoryDisplayManager.Ins.ShowPlayerStall(playerStall);
        base.Interact();
    }

    public override void Quit()
    {
        InventoryDisplayManager.Ins.HidePlayerStall();
        base.Quit();
    }
}