using UnityEngine;
using UnityEngine.InputSystem;

public class DialogBehavior : InteractableBehavior
{
    [SerializeField] string _name;
    [TextArea(2, 10)]
    [SerializeField] string _dialog;
    [SerializeField] Sprite _portrait;

    Animator animator;

    public override InteractableType Type => InteractableType.DIALOG;

    public static DialogBehavior InteractingWith;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Interact()
    {
        if (InteractingWith != null)
        {
            Debug.LogError($"Already interacting with {InteractingWith.gameObject.name}");
            return;
        }
        base.Interact();
        InteractingWith = this;
        UIManager.Ins.ShowDialog(true, _name, _dialog, _portrait);
        DayManager.Ins.ConsumeUnit(1);
    }

    public override void Quit()
    {
        base.Quit();
        UIManager.Ins.ShowDialog(false);
        InteractingWith = null;
    }
}
