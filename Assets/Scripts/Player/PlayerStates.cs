using UnityEngine;

public abstract class PlayerState
{
    protected PlayerInteractionManager ctx;
    public PlayerState(PlayerInteractionManager ctx) { this.ctx = ctx; }
    public virtual bool CanMove => true;
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void OnInteract() { }
    public virtual void OnMove(Vector2 input) { }
    public virtual void OnInteractableEntered(InteractableBehavior interactable) { }
    public virtual void OnInteractableExited(InteractableBehavior interactable) { }
    public virtual void OnMapPressed() { }
    public virtual void OnInventoryPressed() { }

    protected void ExitToAppropriateState() => ctx.ExitToAppropriateState();
}


//OVERWORLD GROUP//////////////////////////////////////////////////////////////////////////
public abstract class OverworldState : PlayerState
{
    private bool _mapOpen = false;
    private bool _inventoryOpen = false;

    public OverworldState(PlayerInteractionManager ctx) : base(ctx) { }

    public override void OnMapPressed()
    {
        if (_mapOpen) { MapDisplayManager.Ins.Hide(); _mapOpen = false; }
        else { MapDisplayManager.Ins.Show(); _mapOpen = true; }
    }

    public override void OnInventoryPressed()
    {
        if (_inventoryOpen) { InventoryDisplayManager.Ins.SetInventoryVisibility(false); _inventoryOpen = false; }
        else { InventoryDisplayManager.Ins.SetInventoryVisibility(true); _inventoryOpen = true; }
    }

    public override void OnMove(Vector2 input)
    {
        if (_mapOpen) { MapDisplayManager.Ins.Hide(); _mapOpen = false; }
        if (_inventoryOpen) { InventoryDisplayManager.Ins.SetInventoryVisibility(false); _inventoryOpen = false; }
    }
}

public class IdleState : OverworldState
{
    public IdleState(PlayerInteractionManager ctx) : base(ctx) { }

    public override void OnInteractableEntered(InteractableBehavior interactable)
    {
        ctx.TransitionTo(new InRangeState(ctx, interactable));
    }
}

public class InRangeState : OverworldState
{
    private InteractableBehavior _interactable;

    public InRangeState(PlayerInteractionManager ctx, InteractableBehavior interactable) : base(ctx)
    {
        _interactable = interactable;
    }

    public override void Enter()
    {
        DayManager.Ins.PreviewUnit(true, 1);
    }

    public override void Exit()
    {
        DayManager.Ins.PreviewUnit(false);
    }

    public override void OnInteract()
    {
        if (TimeUIManager.Ins.IsConsuming) return;
        if (TimeIconBehavior.Ins.IsTransitioning) return;
        _interactable.Interact(ctx.transform.position);
        ctx.TransitionTo(new InDialogueState(ctx, _interactable));
    }

    public override void OnInteractableExited(InteractableBehavior interactable)
    {
        ctx.TransitionTo(new IdleState(ctx));
    }

    public override void OnMove(Vector2 input)
    {
        base.OnMove(input); // still close map/inventory on move
    }
}



//INTERACTION GROUP//////////////////////////////////////////////////////////////////////////
public abstract class InteractionState : PlayerState
{
    protected InteractableBehavior _interactable;

    public InteractionState(PlayerInteractionManager ctx, InteractableBehavior interactable) : base(ctx)
    {
        _interactable = interactable;
    }

    public override void OnInteractableExited(InteractableBehavior interactable)
    {
        _interactable.Quit();
        ctx.TransitionTo(new IdleState(ctx));
    }
}

public class InDialogueState : InteractionState
{
    public InDialogueState(PlayerInteractionManager ctx, InteractableBehavior interactable) : base(ctx, interactable) { }

    public override void OnInteract()
    {
        if (DialogueDriver.Ins.HasPendingConfirm)
        {
            DialogueDriver.Ins.Confirm();
        }
        else
        {
            _interactable.Quit();
            ExitToAppropriateState();
        }
    }
}

public class InMenuState : InteractionState
{
    public InMenuState(PlayerInteractionManager ctx, InteractableBehavior interactable) : base(ctx, interactable) { }

    public override void OnInteract() { }
}

//CUTSCENE GROUP//////////////////////////////////////////////////////////////////////////
public class InCutsceneState : PlayerState
{
    public override bool CanMove => false;

    public InCutsceneState(PlayerInteractionManager ctx) : base(ctx) { }

    public override void OnInteract()
    {
        if (DialogueDriver.Ins.HasPendingConfirm)
            DialogueDriver.Ins.Confirm();
    }
}