using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableBehavior : MonoBehaviour
{
    [SerializeField] private float _hungerCost;
    [SerializeField] public BubbleScript interactableIcon;

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

    public void TriggerIconExpand(bool inRange)
    {
        if (interactableIcon != null)
            interactableIcon.Expand(inRange);
    }

    public virtual void TriggerIconPopAndShrink()
    {
        if (interactableIcon != null)
            interactableIcon.StartCoroutine(interactableIcon.PopAndShrink());
    }
}

public enum InteractableType
{
    INN,
    DIALOG,
    SIGN,
    NPC
}