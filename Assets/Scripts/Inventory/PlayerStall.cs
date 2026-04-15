using UnityEngine;
public class PlayerStall : SlotGroup
{
    [SerializeField] int slotAmount = 5;
    [SerializeField] Town town;

    PlayerListingSlot[] slots;

    public override int Count => slotAmount;
    public Town Town => town;

    void Awake()
    {
        slots = new PlayerListingSlot[slotAmount];
    }

    public void Add(PlayerListingSlot listing)
    {
        if (TryGetEmptySlotIndex(out var idx))
        {
            slots[idx] = listing;
            listing.OnSlotChanged += StallChanged;
            OnSlotGroupChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning("Could not add item to inventory, no room");
        }
    }

    public override Slot GetAt(int idx)
    {
        if (idx >= slotAmount || idx < 0) return null;
        return slots[idx];
    }

    public bool Remove(PlayerListingSlot listing)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == listing)
            {
                slots[i].OnSlotChanged -= StallChanged;
                slots[i] = null;
                OnSlotGroupChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public bool CanAddListing()
    {
        return TryGetEmptySlotIndex(out var _);
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

    void StallChanged()
    {
        OnSlotGroupChanged?.Invoke();
    }
}