using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _mapAction;
    [SerializeField] private InputActionReference _inventoryAction;

    public Vector2 MovementInputVector { get; private set; }
    public Action OnPlayerMove;

    private PlayerState _currentState;

    private void Start()
    {
        InteractableMonitor.Ins.OnInteractableEntered += HandleInteractableEntered;
        InteractableMonitor.Ins.OnInteractableExited += HandleInteractableExited;
        TavernManager.Ins.OnEnterExitCutscene += HandleCutsceneStateChange;
        TransitionTo(new IdleState(this));
    }

    private void OnDestroy()
    {
        InteractableMonitor.Ins.OnInteractableEntered -= HandleInteractableEntered;
        InteractableMonitor.Ins.OnInteractableExited -= HandleInteractableExited;
        TavernManager.Ins.OnEnterExitCutscene -= HandleCutsceneStateChange;
    }

    public void ExitToAppropriateState()
    {
        if (_currentState is InCutsceneState) return;

        TransitionTo(InteractableMonitor.Ins.Interactable != null
            ? new InRangeState(this, InteractableMonitor.Ins.Interactable)
            : new IdleState(this));
        InteractableMonitor.Ins.RecheckCurrent();
    }

    private void HandleCutsceneStateChange(bool entering)
    {
        if (entering)
            TransitionTo(new InCutsceneState(this));
        else
            ExitToAppropriateState();
    }

    public void TransitionTo(PlayerState newState)
    {
        Debug.Log("transitioning to " + newState);
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void OnInteract() => _currentState?.OnInteract();

    private void HandleInteractableEntered(InteractableBehavior i) => _currentState?.OnInteractableEntered(i);
    private void HandleInteractableExited(InteractableBehavior i) => _currentState?.OnInteractableExited(i);

    private void OnMove(InputValue v)
    {
        var input = (_currentState != null && _currentState.CanMove) ? v.Get<Vector2>() : Vector2.zero;
        _currentState?.OnMove(input);
        MovementInputVector = input;
        OnPlayerMove?.Invoke();
    }

    public void ResetMovement()
    {
        MovementInputVector = Vector2.zero;
    }
}