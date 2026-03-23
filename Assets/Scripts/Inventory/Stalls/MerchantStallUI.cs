using UnityEngine;
using UnityEngine.EventSystems;

public class MerchantStallUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] StallSlot[] slots;

    public void Show()
    {
        foreach (var s in slots) s.UpdateSlot();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StallSlot slot = GetClickedSlot();
        if (slot == null) return;
        PlayerInventory.Instance.TryPurchase(slot.GetPrice(), slot.GetItem(), slot.GetAmount());
        // if we want to add cooldown / remove items from shop do it here
    }

    StallSlot GetClickedSlot()
    {
        foreach (var s in slots)
        {
            if (s.hovering) return s;
        }
        return null;
    }
}