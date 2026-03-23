using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class Inventory : MonoBehaviour
{
    // public ItemSO itemToCollect;
    // public int numberOfItem;
    // public GameObject inventorySlotParent;

    [SerializeField] private List<Slot> inventorySlots = new List<Slot>();

    // private void Awake()
    // {
    //     inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
    // }

    public void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;

        foreach (Slot slot in inventorySlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= amountToAdd;

                    if (remaining <= 0)
                    {
                        return;
                    }
                }
            }
        }

        foreach (Slot slot in inventorySlots)
        {
            if (!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;

                if (remaining <= 0)
                {
                    return;
                }
            }
        }

        if (remaining > 0)
        {
            Debug.Log($"{gameObject.name} Inventory is full, could not add " + remaining + " of " + itemToAdd.itemName);
        }
    }

    public void RemoveItem(ItemSO itemToRemove, int amountToRemove)
    {
        int remaining = amountToRemove;

        foreach (Slot slot in inventorySlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToRemove)
            {
                int currentAmount = slot.GetAmount();
                if (currentAmount > remaining)
                {
                    slot.SetItem(itemToRemove, currentAmount - amountToRemove);
                    remaining = 0;
                }
                else if (currentAmount <= amountToRemove)
                {
                    slot.ClearSlot();
                    remaining -= currentAmount;
                }
                if (remaining == 0) return;
            }
        }

        if (remaining >= 0)
        {
            Debug.LogError($"Tried to remove more {itemToRemove.name} than inventory contained");
        }
    }

    public List<Slot> OccupiedSlots()
    {
        List<Slot> occupied = new();
        foreach (var s in inventorySlots)
        {
            if (s.HasItem()) occupied.Add(s);
        }
        return occupied;
    }

    public void SetInventory(ItemListing[] items)
    {
        if (items.Length > inventorySlots.Count)
        {
            Debug.LogWarning("More items than slots");
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            Slot s = inventorySlots[i];
            if (i >= items.Length) s.ClearSlot();
            else
            {
                ItemSO item = items[i].item;
                int amount = items[i].amount;
                s.SetItem(item, amount);
            }
        }
    }

    public Slot GetHoveredItem()
    {
        // TODO this is so inefficient
        foreach (var slot in inventorySlots)
        {
            if (slot != null && slot.HasItem() && slot.hovering)
            {
                return slot;
            }
        }
        return null;
    }

    // I have not tested this (Marcella)
    public void RemoveRandomItems(int amount)
    {
        var occupiedSlots = OccupiedSlots();

        for (int i = 0; i < amount; i++)
        {
            Slot randomSlot = occupiedSlots[UnityEngine.Random.Range(0, occupiedSlots.Count)];
            if (randomSlot.RemoveAmount(1) <= 0)
            {
                occupiedSlots.Remove(randomSlot);
            }
        }
    }

    public List<Slot> GetInventorySlots()
    {
        return new(inventorySlots);
    }
}
