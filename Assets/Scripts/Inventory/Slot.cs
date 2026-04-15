using System;
using UnityEngine;

[Serializable]
public class Slot
{
    public const int STACK_SIZE = 16;

    public ItemData item;
    public int amount;

    public Action OnSlotChanged;

    public Slot(ItemData itemData, int itemAmount)
    {
        item = itemData;
        amount = itemAmount;
    }

    public Slot(Slot slot)
    {
        item = slot.item;
        amount = slot.amount;
    }

    public float GetMerchantPrice()
    {
        return item.merchantSalePrice * amount;
    }

    /// <summary>
    /// Returns amount that couldn't be added without exceeding stack size.
    /// </summary>
    /// <param name="addAmount"></param>
    /// <returns></returns>
    public int AddAmount(int addAmount)
    {
        amount = Mathf.Min(STACK_SIZE, amount + addAmount);
        OnSlotChanged?.Invoke();
        return Mathf.Max(0, amount + addAmount - STACK_SIZE);
    }

    /// <summary>
    /// Returns new amount.
    /// </summary>
    /// <param name="subAmount"></param>
    /// <returns></returns>
    public int SubtractAmount(int subAmount)
    {
        amount = Mathf.Max(0, amount - subAmount);
        OnSlotChanged?.Invoke();
        return amount;
    }
}