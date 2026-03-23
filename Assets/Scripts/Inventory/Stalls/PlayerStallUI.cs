using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStallUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] StallSlot[] slots;
    [SerializeField] ListingUI listingUI;

    [SerializeField] GameObject inventoryDisplayParent;
    [SerializeField] Slot[] mockInventory;

    bool selectingItemToSell = false;

    void OnEnable()
    {
        listingUI.Hide();
        // inventoryDisplayParent.SetActive(false);
    }

    void OnDisable()
    {
        listingUI.Hide();
        // inventoryDisplayParent.SetActive(true);
    }

    public void Show()
    {
        RefreshInventory();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("pointer click");
        if (selectingItemToSell)
        {
            Slot s = GetSelectedInventorySlot();
            if (s == null || !s.HasItem()) return;
            FoundItemToSell(s.GetItem(), s.GetAmount());
        }
        else
        {
            StallSlot s = GetSelectedStallSlot();
            if (s == null) return;
            if (!s.HasItem()) ShowInventoryToChoose();
            else EditListing(s);
        }
    }

    Slot GetSelectedInventorySlot()
    {
        if (!inventoryDisplayParent.activeInHierarchy) return null;
        foreach (var s in mockInventory)
        {
            if (s != null && s.hovering)
            {
                return s;
            }
        }
        return null;
    }

    StallSlot GetSelectedStallSlot()
    {
        if (!gameObject.activeInHierarchy) return null;
        foreach (var s in slots)
        {
            if (s != null && s.hovering)
            {
                return s;
            }
        }
        return null;
    }

    void ShowInventoryToChoose()
    {
        selectingItemToSell = true;

        listingUI.Hide();
        RefreshInventory();
        inventoryDisplayParent.SetActive(true);
    }

    void RefreshInventory()
    {
        List<Slot> actualInventory = PlayerInventory.Instance.GetInventorySlots();
        for (int i = 0; i < actualInventory.Count; i++)
        {
            if (i >= mockInventory.Length)
            {
                Debug.LogWarning("Cannot display entire inventory");
                return;
            }
            mockInventory[i].SetItem(actualInventory[i].GetItem(), actualInventory[i].GetAmount());
        }
    }

    void FoundItemToSell(ItemSO item, int amount)
    {
        selectingItemToSell = false;

        inventoryDisplayParent.SetActive(false);
        StallSlot s = FindEmptySlot();
        if (s == null) return;

        s.SetItem(item, amount);
        PlayerInventory.Instance.RemoveItem(item, amount);
        EditListing(s);
    }

    StallSlot FindEmptySlot()
    {
        foreach (var s in slots)
        {
            if (!s.HasItem()) return s;
        }

        return null;
    }

    void EditListing(StallSlot s)
    {
        listingUI.Show(s);
    }
}