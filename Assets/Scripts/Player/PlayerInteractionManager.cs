using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private InteractableMonitor _interactableMonitor;

    public Vector2 MovementInputVector { get; private set; }
    public Action OnPlayerMove;

    private bool _interacting;

    private void Start()
    {
        _interactableMonitor.OnInteractableEntered += HandleInteractableEntered;
        _interactableMonitor.OnInteractableExited += HandleInteractableExited;
    }

    private void OnDestroy()
    {
        _interactableMonitor.OnInteractableEntered -= HandleInteractableEntered;
        _interactableMonitor.OnInteractableExited -= HandleInteractableExited;
    }

    private void HandleInteractableEntered(InteractableBehavior interactable)
    {
        DayManager.Ins.PreviewUnit(true, 1);
    }

    private void HandleInteractableExited(InteractableBehavior interactable)
    {
        DayManager.Ins.PreviewUnit(false, 0);
        if (_interacting)
        {
            _interacting = false;
            interactable.Quit();
        }
    }

    private void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>();
        OnPlayerMove?.Invoke();
    }

    private void OnInteract()
    {
        if (UIManager.Ins.HasPendingConfirm)
        {
            UIManager.Ins.Confirm();
            return;
        }

        InteractableBehavior interactable = _interactableMonitor.Interactable;

        if (_interacting)
        {
            _interacting = false;
            interactable.Quit();
            if (_interactableMonitor.Interactable != null)
                DayManager.Ins.PreviewUnit(true, 1);
            return;
        }

        if (interactable == null) return;

        if (interactable.Instant)
        {
            interactable.Interact();
            return;
        }

        _interacting = true;
        if (interactable.Type == InteractableType.DIALOG || interactable.Type == InteractableType.NPC)
            interactable.Interact(this.transform.position);
        else
            interactable.Interact();
    }

    public void ResetMovement()
    {
        MovementInputVector = Vector2.zero;
    }
}
