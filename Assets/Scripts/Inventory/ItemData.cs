using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string title;
    public Sprite sprite;
    [Space]
    public Town town;
    public string merchant;
    public float merchantSalePrice;
    [Space]
    public float town1TargetPrice;
    public float town2TargetPrice;
    public float town3TargetPrice;
    [Space]
    public bool consumable;
    public float healthIncrease;

    public float GetTownTargetPrice(Town town)
    {
        return town switch
        {
            Town.TOWN_1 => town1TargetPrice,
            Town.TOWN_2 => town2TargetPrice,
            _ => town3TargetPrice
        };
    }

    public void SetSaleResult(float pricePerItem, Town town, out CustomerReaction reaction, out bool sold)
    {
        float targetPrice = GetTownTargetPrice(town);
        float cheapThreshold = targetPrice / 2f;

        if (pricePerItem < cheapThreshold)
        {
            reaction = CustomerReaction.CHEAP;
            sold = true;
        }
        else if (pricePerItem <= targetPrice)
        {
            reaction = CustomerReaction.TARGET;
            sold = true;
        }
        else
        {
            reaction = CustomerReaction.EXPENSIVE;
            float saleChance = GetSaleChance(targetPrice, pricePerItem);
            sold = Random.value < saleChance;
        }
    }

    float GetSaleChance(float targetPrice, float pricePerItem)
    {
        if (pricePerItem <= targetPrice) return 1f;

        float noSaleThreshold = pricePerItem * 2f;
        if (pricePerItem >= noSaleThreshold) return 0f;

        float priceAboveTarget = targetPrice - pricePerItem;
        float tooExpensiveIntervalLength = noSaleThreshold - targetPrice;

        return 1 - (priceAboveTarget / tooExpensiveIntervalLength);
    }
}
