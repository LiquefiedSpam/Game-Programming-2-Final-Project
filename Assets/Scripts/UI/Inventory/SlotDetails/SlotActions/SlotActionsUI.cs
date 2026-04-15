using System;
using UnityEngine;

public class SlotActionsUI : MonoBehaviour
{
    [SerializeField] SlotDetailsUI slotDetails;

    public Action OnSlotRemoved;

    protected Slot slot;

    void OnDisable()
    {
        UnsubscribeFromActions();
    }

    public virtual void Show(Slot s)
    {
        slot = s;
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        slot = null;
        gameObject.SetActive(false);
        UnsubscribeFromActions();
    }

    protected virtual void UnsubscribeFromActions()
    {
        // unsubscribe relevant details methods to actions
    }
}