using UnityEngine;

public class InteractionInfo
{
    const float CHANCE_PARTITION = 0.1f;
    public float MarauderChance;

    public InteractionInfo()
    {
        DayManager.Ins.OnDayChanged += HandleDayChanged;
    }

    public void SetMarauderChance(float chance)
    {
        MarauderChance = chance;
    }

    public InteractionResult PassInteraction()
    {
        if (Random.value < MarauderChance)
        {
            MarauderChance = 1f;
            return InteractionResult.MARAUDERS;
        }
        else
        {
            MarauderChance = 0f;
            return InteractionResult.SAFE;
        }
    }

    void HandleDayChanged()
    {
        if (MarauderChance < 0.5f)
        {
            MarauderChance += CHANCE_PARTITION;
        }
        else if (MarauderChance > 0.5f)
        {
            MarauderChance -= CHANCE_PARTITION;
        }
    }
}

public enum InteractionResult
{
    SAFE, MARAUDERS
}