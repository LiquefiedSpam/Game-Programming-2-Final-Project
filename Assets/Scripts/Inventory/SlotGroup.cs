using System;
using UnityEngine;

public class SlotGroup : MonoBehaviour
{
    public virtual int Count => 0;
    public Action OnSlotGroupChanged;
    public virtual Slot GetAt(int idx) { return null; }
    public virtual bool Contains(Slot s) { return false; }
}