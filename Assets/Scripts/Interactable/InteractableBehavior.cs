using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableBehavior : MonoBehaviour
{
    [SerializeField] private float _hungerCost;
    [SerializeField] public GameObject interactableIcon;
    protected BubbleScript bubbleScript;

    public virtual InteractableType Type { get; protected set; }
    public virtual bool Instant { get; protected set; } = false;

    public static Action<float> OnInteract;
    public static Action OnEndInteract;
    protected bool inCutscene = false;
    public bool InCutscene => inCutscene;

    protected virtual void Awake()
    {
        if (interactableIcon) interactableIcon.GetComponent<BubbleScript>();
    }
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
            bubbleScript.Expand(inRange);
    }

    public virtual void TriggerIconPopAndShrink()
    {
        if (interactableIcon != null)
            bubbleScript.StartCoroutine(bubbleScript.PopAndShrink());
    }
}

public enum InteractableType
{
    INN,
    DIALOG,
    SIGN,
    NPC
}