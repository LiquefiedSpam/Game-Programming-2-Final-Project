using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] InteractableMonitor _interactableMonitor;
    public Vector2 MovementInputVector { get; private set; }

    private void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>();
    }

    private void OnInteract()
    {
        Debug.Log("OnInteract");
        Debug.Log($"Interactable: {_interactableMonitor.Interactable}");
        InteractableBehavior interactable = _interactableMonitor.Interactable;

        if (_interactableMonitor.Interacting)
        {
            _interactableMonitor.SetInteracting(false);
            interactable.Quit();
        }
        else if (interactable != null)
        {
            _interactableMonitor.SetInteracting(true);
            if (interactable.Type == InteractableType.DIALOG)
            {
                interactable.Interact(this.transform.position);
            }
            else
            {
                interactable.Interact();
            }
        }
    }

    public void ResetMovement()
    {
        MovementInputVector = Vector2.zero;
    }
}
