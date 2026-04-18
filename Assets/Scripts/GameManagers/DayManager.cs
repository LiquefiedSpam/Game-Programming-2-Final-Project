using System;
using UnityEngine;

public enum DayInterval
{
    Morning,
    Daytime,
    Evening,
    Night
}
public class DayManager : MonoBehaviour
{
    [SerializeField] private int unitsPerInterval;

    public int Day => day;
    public int Units => units;

    public event Action OnTimeChanged; //for things that just need to know time has changed
    public event Action<int> OnUnitsConsumed; //for things that need to know amount of units that elapsed
    public Action OnDayChanged;

    public DayInterval DayInterval => dayInterval;
    public int UnitsPerInterval => unitsPerInterval;
    public static DayManager Ins => _instance;
    private static DayManager _instance;


    private int day = 1;
    private int units = 1;

    private DayInterval dayInterval = DayInterval.Morning;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of DayManager in scene, destroying component on {gameObject.name}");
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }

        units = unitsPerInterval;
    }

    //force a next day, starting in the morning
    public void NextDay()
    {
        day++;
        OnUnitsConsumed?.Invoke(units);
        units = unitsPerInterval;
        dayInterval = DayInterval.Morning;
        OnTimeChanged?.Invoke();
        OnDayChanged?.Invoke();
    }

    // advances units by  the number inputted. Will change over to next interval 
    // or day if necessary and continue consumption.
    public void ConsumeUnit(int unitsToConsume)
    {
        int remainingUnits = unitsToConsume;
        bool changed = false;

        while (remainingUnits > 0)
        {
            units--;
            remainingUnits--;
            changed = true;
            if (units == 0)
            {
                if (dayInterval == DayInterval.Night)
                {
                    OnDayChanged?.Invoke();
                    dayInterval = DayInterval.Morning;
                }
                else
                {
                    dayInterval = (DayInterval)((int)dayInterval + 1);
                }

                units = unitsPerInterval;
            }
        }

        if (changed)
        {
            OnTimeChanged?.Invoke();
            OnUnitsConsumed?.Invoke(unitsToConsume);
        }
    }
}
