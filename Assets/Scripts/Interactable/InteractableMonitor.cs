using UnityEngine;
using System;

//Monitor collider is a proximity controller used to check if the player is in reach of any 
//InteractableBehavior, and fires appropriate Actions.
[RequireComponent(typeof(Collider))]
public class InteractableMonitor : MonoBehaviour
{
    public static InteractableMonitor Ins => _instance;
    private static InteractableMonitor _instance;

    public InteractableBehavior Interactable { get; private set; }
    public Action<InteractableBehavior> OnInteractableEntered;
    public Action<InteractableBehavior> OnInteractableExited;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of InteractableMonitor, destroying on {gameObject.name}");
            Destroy(this);
            return;
        }
        _instance = this;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Interactable")) return;
        if (other.TryGetComponent<InteractableBehavior>(out var interactable))
        {
            if (Interactable != interactable)
            {
                Interactable = interactable;
                OnInteractableEntered?.Invoke(interactable);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Interactable")) return;
        if (other.TryGetComponent<InteractableBehavior>(out var leavingInteractable))
        {
            if (leavingInteractable != Interactable) return;
            OnInteractableExited?.Invoke(leavingInteractable);
            Interactable = null;
        }
    }

    public void RecheckCurrent()
    {
        Debug.Log("getting here");
        if (Interactable != null)
            OnInteractableEntered?.Invoke(Interactable);
    }
}