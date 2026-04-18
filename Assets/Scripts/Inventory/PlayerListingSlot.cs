using UnityEngine;

public class PlayerListingSlot : Slot
{
    public const float MIN_PRICE_PER_ITEM = 0.5f;
    public const float SALE_CHANCE_PER_TIME_UNIT = 0.04f; // listing has a 4% chance of being sold when 1 time unit passes

    public float ListedPrice { get; private set; }
    public bool HasSaleResult { get; private set; }
    public bool Sold { get; private set; }
    public CustomerReaction CustomerReaction { get; private set; }
    public Town SoldInTown { get; private set; }
    public string ID { get; private set; }
    public float ListedPricePerItem => ListedPrice / amount;

    public PlayerListingSlot(ItemData itemData, int itemAmount, float price, Town inTown) : base(itemData, itemAmount)
    {
        item = itemData;
        amount = itemAmount;
        ListedPrice = price;
        SoldInTown = inTown;
        ID = System.Guid.NewGuid().ToString();
        DayManager.Ins.OnUnitsConsumed += OnUnitsConsumed;
    }

    public void SetPricePerItem(float pricePerItem)
    {
        if (HasSaleResult) return;

        ListedPrice = pricePerItem * amount;
        OnSlotChanged?.Invoke();
    }

    public void Cancel()
    {
        DayManager.Ins.OnUnitsConsumed -= OnUnitsConsumed;
    }

    void EndWaitForResult()
    {
        DayManager.Ins.OnUnitsConsumed -= OnUnitsConsumed;

        item.SetSaleResult(ListedPricePerItem, SoldInTown, out CustomerReaction reaction, out bool sold);
        CustomerReaction = reaction;
        Sold = sold;

        HasSaleResult = true;

        History.AddListing(this);
    }

    void OnUnitsConsumed(int units)
    {
        if (HasSaleResult)
        {
            DayManager.Ins.OnUnitsConsumed -= OnUnitsConsumed;
            return;
        }

        float saleChance = units * SALE_CHANCE_PER_TIME_UNIT;
        if (Random.value < saleChance)
        {
            EndWaitForResult();
        }
    }
}