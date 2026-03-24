using UnityEngine;

public class PlayerStallBehavior : InteractableBehavior
{
    [SerializeField] PlayerStallUI stallUI;
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