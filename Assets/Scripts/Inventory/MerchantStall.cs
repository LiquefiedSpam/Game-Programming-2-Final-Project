using System.Linq;
using UnityEngine;

public class MerchantStall : SlotGroup
{
    [SerializeField] Slot[] slots;

    public override int Count => slots.Length;

    public override Slot GetAt(int idx)
    {
        if (idx >= Count || idx < 0) return null;
        return slots[idx];
    }

    public override bool Contains(Slot s)
    {
        return slots.Contains(s);
    }
}