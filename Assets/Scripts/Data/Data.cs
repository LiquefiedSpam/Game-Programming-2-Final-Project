using UnityEngine;

public static class Data
{
    public static CustomerReactions ReactionSprites;
    static EconomyData economy;

    static int Town = 1;

    public static bool TryGetPurchaseTime(ItemSO item, float normalizedPrice, out Vector2Int purchaseTime)
    {
        if (!economy.ContainsItem(item))
        {
            Debug.LogWarning("Economy does not contain item");
            purchaseTime = Vector2Int.zero;
            return false;
        }

        return economy[item].TryGetPurchaseTime(normalizedPrice, Town, out purchaseTime);
    }

    public static bool ValidPrice(ItemSO item, float normalizedPrice)
    {
        if (!economy.ContainsItem(item))
        {
            Debug.LogWarning("Economy does not contain item");
            return false;
        }

        return economy[item].ValidPrice(normalizedPrice, Town);
    }

    public static CustomerReaction GetReaction(ItemSO item, Vector2Int waitToBuyTime)
    {
        if (!economy.ContainsItem(item))
        {
            Debug.LogWarning("Economy does not contain item");
            return CustomerReaction.ANGRY;
        }

        return economy[item].GetCustomerReaction(waitToBuyTime);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        economy = Resources.Load<EconomyData>("EconomyData");
        if (economy == null)
        {
            Debug.LogError("Failed to load economy data (expected at path Resources/EconomyData)");
            return;
        }
        ReactionSprites = Resources.Load<CustomerReactions>("CustomerReactionSprites");
        Teleport.OnTownChanged += TownChanged;
    }

    static void TownChanged(int newTown)
    {
        Town = newTown;
    }
}