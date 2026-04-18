using System.Collections.Generic;
using UnityEngine;

public static class History
{
    public static Dictionary<Town, Dictionary<ItemData, ItemHistory>> HistoryBySaleTown;
    public static HashSet<ItemData> ObtainedItems;
    public static List<string> RecordedListingIDs;

    public static void AddObtainedItem(ItemData item)
    {
        ObtainedItems.Add(item);
    }

    public static void AddListing(PlayerListingSlot listing)
    {
        if (!listing.HasSaleResult)
        {
            Debug.LogWarning("Listing does not have sale result");
            return;
        }

        if (RecordedListingIDs.Contains(listing.ID)) return;
        RecordedListingIDs.Add(listing.ID);

        var saleDict = HistoryBySaleTown[listing.SoldInTown];
        if (!saleDict.ContainsKey(listing.item))
        {
            saleDict[listing.item] = new ItemHistory(listing.item);
        }
        saleDict[listing.item].AddPrice(listing.ListedPricePerItem, listing.CustomerReaction);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        HistoryBySaleTown = new()
        {
            [Town.WOODED_KEEP] = new(),
            [Town.SANDY_STALLS] = new(),
            [Town.STONE_SANCTUARY] = new()
        };
        RecordedListingIDs = new();
    }
}

public class ItemHistory
{
    public ItemData itemData;
    public Town itemTown;
    public float BestCheapPrice { get; private set; } = -1;
    public float BestTargetPrice { get; private set; } = -1;
    public float BestExpensivePrice { get; private set; } = -1;

    public ItemHistory(ItemData item)
    {
        itemData = item;
    }

    public void AddPrice(float pricePerItem, CustomerReaction reaction)
    {
        switch (reaction)
        {
            case CustomerReaction.CHEAP:
                if (BestCheapPrice < pricePerItem)
                {
                    BestCheapPrice = pricePerItem;
                }
                break;
            case CustomerReaction.TARGET:
                if (BestTargetPrice < pricePerItem)
                {
                    BestTargetPrice = pricePerItem;
                }
                break;
            case CustomerReaction.EXPENSIVE:
                if (BestExpensivePrice == -1 || BestExpensivePrice > pricePerItem)
                {
                    BestExpensivePrice = pricePerItem;
                }
                break;
        }
    }

    public bool TryGetBestPrice(CustomerReaction customerReaction, out float price)
    {
        price = customerReaction switch
        {
            CustomerReaction.CHEAP => BestCheapPrice,
            CustomerReaction.TARGET => BestTargetPrice,
            CustomerReaction.EXPENSIVE => BestExpensivePrice,
            _ => -1
        };
        return price > 0;
    }
}