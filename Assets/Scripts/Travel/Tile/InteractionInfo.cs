using UnityEngine;

public class InteractionInfo
{
    public int MarauderChance; // 10 = 100% chance, 0 = 0% chance;

    public InteractionInfo()
    {
        MarauderChance = 5;
        DayManager.Ins.OnDayChanged += HandleDayChanged;
    }

    /// <summary>
    /// 10 = 100% chance, 0 = 0% chance.
    /// </summary>
    /// <param name="chance"></param>
    public void SetMarauderChance(int chance)
    {
        MarauderChance = Mathf.Clamp(chance, 0, 10);
    }

    public void ModifyMarauderChance(int amt)
    {
        MarauderChance = MarauderChance = Mathf.Clamp(MarauderChance + amt, 0, 10);
    }

    public InteractionResult PassInteraction()
    {
        if ((Random.value * 10f) < MarauderChance)
        {
            MarauderChance = 10;
            return InteractionResult.MARAUDERS;
        }
        else
        {
            MarauderChance = 0;
            return InteractionResult.SAFE;
        }
    }

    void HandleDayChanged()
    {
        if (MarauderChance < 5)
        {
            MarauderChance++;
        }
        else if (MarauderChance > 5)
        {
            MarauderChance--;
        }
    }
}

public enum InteractionResult
{
    SAFE, MARAUDERS
}