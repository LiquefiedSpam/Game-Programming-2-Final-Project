using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableBehavior : MonoBehaviour
{
    [SerializeField] private float _hungerCost;

    public virtual InteractableType Type { get; protected set; }
    public virtual bool Instant { get; protected set; } = false;

    public static Action<float> OnInteract;
    public static Action OnEndInteract;

    public virtual void Interact()
    {
        if (_hungerCost > 0) OnInteract?.Invoke(_hungerCost);
    }

    public virtual void Interact(Vector3 playerPos)
    {
        if (_hungerCost > 0) OnInteract?.Invoke(_hungerCost);
    }

    public virtual void Quit()
    {
        OnEndInteract?.Invoke();
    }
}

public enum InteractableType
{
    INN,
    DIALOG,
    SIGN,
    NPC
}