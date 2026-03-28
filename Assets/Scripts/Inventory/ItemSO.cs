using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Item", menuName = "NewItem")]
public class ItemSO : ScriptableObject
{
    public const int MAX_PURCHASE_DAYS = 3;
    public const int INTERVAL_COUNT = 3;

    public string itemName;
    public Sprite icon;
    public int maxStackSize;
    public GameObject itemPrefab;
    public GameObject handItemPrefab;

    [Header("If the player prices an item above the max price it will not be purchased")]
    [SerializeField] float maxPriceTown1;
    [SerializeField] float maxPriceTown2;
    [SerializeField] float maxPriceTown3;

    public float MaxPriceForTown(int town)
    {
        return town switch
        {
            1 => maxPriceTown1,
            2 => maxPriceTown2,
            3 => maxPriceTown3,
            _ => throw new Exception($"Unrecognized town index {town}")
        };
    }

    public bool ValidPrice(float price, int town)
    {
        return town switch
        {
            1 => price <= maxPriceTown1,
            2 => price <= maxPriceTown2,
            3 => price <= maxPriceTown3,
            _ => throw new Exception($"Unrecognized town index {town}")
        };
    }

    /// <summary>
    /// Gets time it will take to purchase item at price in (Day, Unit). Returns false if it will
    /// take longer than max purchase days.
    /// </summary>
    /// <returns></returns>
    public bool TryGetPurchaseTime(float price, int town, out Vector2Int purchaseTime)
    {
        if (!ValidPrice(price, town))
        {
            purchaseTime = Vector2Int.zero;
            return false;
        }

        float maxPricePerDay = MaxPriceForTown(town) / MAX_PURCHASE_DAYS;

        int days = 0;
        int intervals = 0;

        float priceThreshold = maxPricePerDay;
        for (int i = 0; i < MAX_PURCHASE_DAYS; i++)
        {
            if (price < priceThreshold)
            {
                days = i;
                intervals = Mathf.RoundToInt(INTERVAL_COUNT * Mathf.Max(0f, price - (priceThreshold * i)));
                break;
            }
            priceThreshold += maxPricePerDay;
        }

        if (intervals == INTERVAL_COUNT)
        {
            days++;
            intervals = 0;
        }

        purchaseTime = new(days, intervals);
        return true;
    }

    public CustomerReaction GetCustomerReaction(Vector2Int buyWaitTime)
    {
        if (buyWaitTime.x < 1) return CustomerReaction.HAPPY;
        if (buyWaitTime.x < 2) return CustomerReaction.OKAY;
        if (buyWaitTime.x < 3) return CustomerReaction.ANNOYED;
        return CustomerReaction.ANGRY;
    }
}
