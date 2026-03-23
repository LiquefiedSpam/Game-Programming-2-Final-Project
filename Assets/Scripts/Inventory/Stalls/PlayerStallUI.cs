using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStallUI : MonoBehaviour, IPointerClickHandler
{
    bool selectingItemToSell = false;
    [SerializeField] StallSlot[] slots;

    [SerializeField] GameObject inventoryDisplayParent;
    [SerializeField] Slot[] inventoryDisplay;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectingItemToSell)
        {
            Slot s = GetSelectedInventorySlot();
            if (s == null) return;
            FoundItemToSell(s.GetItem(), s.GetAmount());
        }
        else
        {
            // foreach (var s in slots)
            // {
            //     if ()
            // }
        }
    }

    Slot GetSelectedInventorySlot()
    {
        if (!inventoryDisplayParent.activeInHierarchy) return null;
        foreach (var s in inventoryDisplay)
        {
            if (s.hovering)
            {
                if (!s.HasItem()) continue;
                return s;
            }
        }
        return null;
    }

    // StallSlot GetSelectedStallSlot()
    // {
    //     if (!gameObject.activeInHierarchy) return null;
    //     foreach (var s in slots)
    //     {
    //         if (s.hovering)
    //         {
    //             if (!s.HasItem())
    //             {
    //                 SelectItemToSell();
    //             }
    //         }
    //     }
    // }

    void FoundItemToSell(ItemSO item, int amount)
    {

    }

    void SelectItemToSell()
    {

    }

    int FindEmptySlotIndex()
    {
        for (int i = 0; i < slots.Length; i++)
        {

        }

        return 0;
    }
}