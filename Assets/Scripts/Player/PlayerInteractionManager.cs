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
        TransitionTo(new IdleState(this));
    }

    private void OnDestroy()
    {
        InteractableMonitor.Ins.OnInteractableEntered -= HandleInteractableEntered;
        InteractableMonitor.Ins.OnInteractableExited -= HandleInteractableExited;
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