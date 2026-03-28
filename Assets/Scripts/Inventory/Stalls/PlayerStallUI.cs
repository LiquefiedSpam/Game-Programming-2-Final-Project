using System.Collections.Generic;
using UnityEngine;

public class PlayerStallUI : MonoBehaviour
{
    [SerializeField] StallSlot[] listingSlots;
    [SerializeField] ListingUI listingUI;
    [SerializeField] GameObject inventoryDisplayParent;
    [SerializeField] StallSlot[] inventorySlots;

    void OnEnable()
    {
        ShowInventorySlots();
        foreach (var s in listingSlots) s.OnSlotClicked += OnListingSlotClicked;
        foreach (var s in inventorySlots) s.OnSlotClicked += OnInventorySlotClicked;
    }

    void OnDisable()
    {
        listingUI.Hide();
        foreach (var s in listingSlots) s.OnSlotClicked -= OnListingSlotClicked;
        foreach (var s in inventorySlots) s.OnSlotClicked -= OnInventorySlotClicked;
        listingUI.OnListingRemoved -= () => RefreshInventorySlots();
        listingUI.OnUIClosed -= () => ShowInventorySlots();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnInventorySlotClicked(StallSlot s)
    {
        if (!CanPlaceItemForSale())
        {
            inventoryDisplayParent.SetActive(false);
            Hide();
        }

        PlayerInventory.Instance.RemoveItem(s.GetItem(), s.GetAmount());
        CreateListing(s.GetItem(), s.GetAmount());
        s.ClearSlot();
    }

    void ShowInventorySlots()
    {
        listingUI.Hide();
        RefreshInventorySlots();
        inventoryDisplayParent.SetActive(true);
    }

    void RefreshInventorySlots()
    {
        List<Slot> actualInventory = PlayerInventory.Instance.GetInventorySlots();
        for (int i = 0; i < actualInventory.Count; i++)
        {
            if (i >= inventorySlots.Length)
            {
                Debug.LogWarning("Cannot display entire inventory");
                return;
            }
            inventorySlots[i].SetItem(actualInventory[i].GetItem(), actualInventory[i].GetAmount());
        }
    }


    void OnListingSlotClicked(StallSlot s)
    {
        if (s.IsPurchased())
        {
            ShowListingUI(s);
            return;
        }
        if (!s.HasItem()) return;
        ShowListingUI(s);
    }

    void CreateListing(ItemSO item, int amount)
    {
        StallSlot s = FindEmptyStallSlot();
        if (s == null)
        {
            Debug.LogError("No room to add listing, method should not have been called");
            return;
        }

        s.SetItem(item, amount);
        ShowListingUI(s);
    }

    void ShowListingUI(StallSlot s)
    {
        inventoryDisplayParent.SetActive(false);
        listingUI.Show(s);
        listingUI.OnListingRemoved += () => RefreshInventorySlots();
        listingUI.OnUIClosed += () => ShowInventorySlots();
    }

    StallSlot FindEmptyStallSlot()
    {
        foreach (var s in listingSlots)
        {
            if (!s.HasItem()) return s;
        }

        return null;
    }

    bool CanPlaceItemForSale()
    {
        return FindEmptyStallSlot() != null;
    }
}