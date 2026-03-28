using UnityEngine;

public class SignBehavior : InteractableBehavior
{
    [TextArea(2, 10)]
    [SerializeField] string _message;
    public override InteractableType Type => InteractableType.SIGN;

    public static SignBehavior InteractingWith;

    public override void Interact()
    {
        if (InteractingWith != null)
        {
            Debug.LogError($"Already interacting with {InteractingWith.gameObject.name}");
            return;
        }
        base.Interact();
        InteractingWith = this;
        UIManager.Ins.ShowSign(true, _message);
    }

    public override void Quit()
    {
        base.Quit();
        UIManager.Ins.ShowSign(false);
        InteractingWith = null;
    }
}