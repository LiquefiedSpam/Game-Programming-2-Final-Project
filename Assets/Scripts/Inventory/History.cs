using System.Collections.Generic;
using UnityEngine;

public static class History
{
    public static List<PlayerListingSlot> ListingsByTime;
    public static Dictionary<ItemData, List<PlayerListingSlot>> ListingsByItem;
    public static List<string> RecordedListingIDs;

    public static void AddListing(PlayerListingSlot listing)
    {
        if (RecordedListingIDs.Contains(listing.ID)) return;
        RecordedListingIDs.Add(listing.ID);
        ListingsByTime.Add(listing);
        if (ListingsByItem.ContainsKey(listing.item))
        {
            ListingsByItem[listing.item].Add(listing);
        }
        else
        {
            ListingsByItem[listing.item] = new() { listing };
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        ListingsByTime = new();
        ListingsByItem = new();
        RecordedListingIDs = new();
    }
}