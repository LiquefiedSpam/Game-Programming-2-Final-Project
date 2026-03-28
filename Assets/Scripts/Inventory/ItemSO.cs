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
    public int GetDaysBeforePurchase(float price, int town)
    {
        if (!ValidPrice(price, town))
        {
            return -1;
        }

        int days = 0;

        float maxPricePerDay = MaxPriceForTown(town) / MAX_PURCHASE_DAYS;
        float priceThreshold = maxPricePerDay;
        for (int i = 0; i < MAX_PURCHASE_DAYS; i++)
        {
            if (price < priceThreshold)
            {
                days = i;
                break;
            }
            priceThreshold += maxPricePerDay;
        }

        return days;
    }

    public CustomerReaction GetCustomerReaction(int buyWaitTime)
    {
        if (buyWaitTime <= 1) return CustomerReaction.HAPPY;
        if (buyWaitTime <= 2) return CustomerReaction.OKAY;
        if (buyWaitTime <= 3) return CustomerReaction.ANNOYED;
        return CustomerReaction.ANGRY;
    }
}
