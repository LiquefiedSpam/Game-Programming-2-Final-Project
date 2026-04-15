
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : SlotGroup
{
    [SerializeField] int slotAmount = 5;
    [SerializeField] Slot[] startingInventory;
    Slot[] slots;

    static int[] randomIndices;

    public override int Count => slotAmount;

    public static Inventory Ins;

    void Awake()
    {
        if (Ins != null && Ins != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Ins = this;
        }

        slots = new Slot[slotAmount];
        for (int i = 0; i < startingInventory.Length; i++)
        {
            slots[i] = startingInventory[i];
            slots[i].OnSlotChanged += InventoryChanged;
        }

        randomIndices = new int[slotAmount];
        for (int i = 0; i < slotAmount; i++) randomIndices[i] = i;
    }

    public void Add(Slot purchase)
    {
        int remaining = TryAddToExisting(purchase);

        if (remaining > 0)
        {
            if (TryGetEmptySlotIndex(out var idx))
            {
                slots[idx] = new(purchase.item, remaining);
                purchase.OnSlotChanged += InventoryChanged;
            }
            else Debug.LogWarning("Could not add item to inventory, no room");
        }

        OnSlotGroupChanged?.Invoke();
    }

    public bool Remove(Slot slot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == slot)
            {
                slots[i].OnSlotChanged -= InventoryChanged;
                slots[i] = null;
                OnSlotGroupChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public Slot RemoveAt(int idx)
    {
        if (idx >= slotAmount || idx < 0) return null;
        Slot s = slots[idx];
        slots[idx] = null;
        OnSlotGroupChanged?.Invoke();
        return s;
    }

    public override Slot GetAt(int idx)
    {
        if (idx >= slotAmount || idx < 0) return null;
        return slots[idx];
    }

    public override bool Contains(Slot s)
    {
        return slots.Contains(s);
    }

    public bool HasRoomFor(Slot s)
    {
        if (TryGetEmptySlotIndex(out var idx))
        {
            return true;
        }
        else if (CanFitInExisting(s))
        {
            return true;
        }

        return false;
    }

    // returns amount that couldn't be added to existing slot
    int TryAddToExisting(Slot purchase)
    {
        int remaining = purchase.amount;
        foreach (var s in slots)
        {
            if (s == null || s.item == null) continue;
            if (s.item == purchase.item)
            {
                remaining = s.AddAmount(remaining);
                if (remaining == 0) break;
            }
        }

        return remaining;
    }

    bool CanFitInExisting(Slot slot)
    {
        int remaining = slot.amount;
        foreach (var s in slots)
        {
            if (s == null || s.item == null) continue;
            if (s.item == slot.item)
            {
                remaining -= (Slot.STACK_SIZE - s.amount);
                if (remaining <= 0) return true;
            }
        }
        return false;
    }

    // returns true if there is an empty slot
    bool TryGetEmptySlotIndex(out int idx)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null || slots[i].amount == 0)
            {
                idx = i;
                return true;
            }
        }
        idx = 0;
        return false;
    }

    void InventoryChanged()
    {
        OnSlotGroupChanged?.Invoke();
    }

    public void RemoveRandomItems(int amount)
    {
        randomIndices.Shuffle();

        int remainingToRemove = amount;

        for (int i = 0; i < slotAmount; i++)
        {
            int randomIdx = randomIndices[i];
            if (slots[randomIdx] != null && slots[randomIdx].item != null)
            {
                slots[randomIdx] = null;
                remainingToRemove--;
                if (remainingToRemove <= 0) break;
            }
        }
    }
}