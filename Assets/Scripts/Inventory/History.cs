using System.Collections.Generic;
using UnityEngine;

public static class History
{
    public static Dictionary<Town, Dictionary<ItemData, ItemHistory>> ItemTownHistory;
    // public static List<PlayerListingSlot> ListingsByTime;
    // public static Dictionary<ItemData, List<PlayerListingSlot>> ListingsByItem;
    public static List<string> RecordedListingIDs;

    public static void AddListing(PlayerListingSlot listing)
    {
        if (!listing.HasSaleResult)
        {
            Debug.LogWarning("Listing does not have sale result");
            return;
        }

        if (RecordedListingIDs.Contains(listing.ID)) return;
        RecordedListingIDs.Add(listing.ID);

        var dict = ItemTownHistory[listing.SoldInTown];
        if (!dict.ContainsKey(listing.item))
        {
            dict[listing.item] = new ItemHistory(listing.item);
        }
        dict[listing.item].AddPrice(listing.ListedPricePerItem, listing.CustomerReaction);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        ItemTownHistory = new()
        {
            [Town.TOWN_1] = new(),
            [Town.TOWN_2] = new(),
            [Town.TOWN_3] = new()
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