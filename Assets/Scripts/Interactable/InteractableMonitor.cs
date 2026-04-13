using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class InteractableMonitor : MonoBehaviour
{
    public InteractableBehavior Interactable { get; private set; }
    public bool Interacting { get; private set; }
    public void SetInteracting(bool interacting)
    {
        if (Interactable == null || Interactable.Instant)
        {
            Interacting = false;
            return;
        }

        if (Interactable.interactableIcon != null && interacting == true)
            Interactable.TriggerIconPopAndShrink();

        Interacting = interacting;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Interactable")) return;
        if (other.TryGetComponent<InteractableBehavior>(out var interactable))
        {
            if (Interactable != interactable)
            {
                if (Interactable != null && Interactable.interactableIcon != null)
                {
                    Interactable.TriggerIconExpand(false);
                }
                Interactable = interactable;

                if (Interactable.interactableIcon != null)
                {
                    Interactable.TriggerIconExpand(true);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Interactable")) return;
        if (other.TryGetComponent<InteractableBehavior>(out var interactable))
        {
            if (!interactable == Interactable) return;
            if (Interacting)
            {
                Interacting = false;
                Interactable.Quit();
            }
            else
            {
                if (Interactable.interactableIcon != null)
                {
                    Interactable.TriggerIconExpand(false);
                }
            }
            Interactable = null;
        }
    }
}