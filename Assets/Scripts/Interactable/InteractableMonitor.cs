using UnityEngine;
using System;

//Monitor collider is a proximity controller used to check if the player is in reach of any 
//InteractableBehavior, and fires appropriate Actions.
[RequireComponent(typeof(Collider))]
public class InteractableMonitor : MonoBehaviour
{
    public InteractableBehavior Interactable { get; private set; }
    public Action<InteractableBehavior> OnInteractableEntered;
    public Action<InteractableBehavior> OnInteractableExited;

    // public void SetInteracting(bool interacting)
    // {
    //     if (Interactable == null || Interactable.Instant)
    //     {
    //         interacting = false;
    //         return;
    //     }

    //     if (Interactable.interactableIcon != null && Interactable.interactableIcon.activeInHierarchy && interacting == true)
    //         Interactable.TriggerIconPopAndShrink();

    //     interacting = interacting;
    // }

    //Fire OnInteractableEntered if we don't already have an interactable present
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Interactable")) return;
        if (other.TryGetComponent<InteractableBehavior>(out var interactable))
        {
            if (Interactable != interactable)
            {
                Interactable = interactable;
                OnInteractableEntered?.Invoke(interactable);
                //DayManager.Ins.PreviewUnit(true, 1);

                // if (Interactable.interactableIcon != null)
                // {
                //     Interactable.TriggerIconExpand(true);
                // }
            }
        }
    }

    //If exiting InteractableBehavior is the one we are interacting with, fire 
    //OnInteractableExited
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Interactable")) return;
        if (other.TryGetComponent<InteractableBehavior>(out var leavingInteractable))
        {
            // if (interacting)
            // {
            //     interacting = false;
            //     Interactable.Quit();
            // }

            if (leavingInteractable != Interactable) return;
            OnInteractableExited?.Invoke(leavingInteractable);
            // if (Interactable.interactableIcon != null)
            //     Interactable.TriggerIconExpand(false);
            // DayManager.Ins.PreviewUnit(false, 1);
            Interactable = null;
        }
    }
}