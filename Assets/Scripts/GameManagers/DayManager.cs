using System;
using UnityEngine;
using UnityEngine.UIElements;

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

    public event Action OnTimeSet; //for things that just need to know time has changed
    public event Action<int> OnUnitsConsumed; //for things that need to know amount of units that elapsed
    public Action OnDayChanged;
    public Action<bool, int> OnTimeUnitPreview; // bool is if we are entering or exiting preview, and int is number of units to preview
    public Action<int> OnTimeUnitConfirmed; //confirmed that these units will eventually be consumed
    //public Action<int> OnTimeUnitTallyAdded; //the time unit tally is the amount of units that will be consumed upon exiting the current interaction.

    public DayInterval DayInterval => dayInterval;
    public int UnitsPerInterval => unitsPerInterval;
    public static DayManager Ins => _instance;
    private static DayManager _instance;


    private int day = 1;
    private int units = 1;
    private int timeUnitTally = 0;
    public int TimeUnitTally => timeUnitTally;

    private DayInterval dayInterval = DayInterval.Evening;

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

    void Start()
    {
        OnTimeSet?.Invoke();
    }

    //force a next day, starting in the morning
    public void NextDay()
    {
        day++;
        OnUnitsConsumed?.Invoke(units);
        units = unitsPerInterval;
        dayInterval = DayInterval.Morning;
        OnTimeSet?.Invoke();
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
            OnUnitsConsumed?.Invoke(unitsToConsume);
            OnTimeSet?.Invoke();
        }
    }

    public void PreviewUnit(bool toPreview, int unitsToPreview = 0)
    {
        OnTimeUnitPreview?.Invoke(toPreview, unitsToPreview);
    }

    public void ConfirmUnit(int units)
    {
        Debug.Log($"ConfirmUnit called, units={units}", this);
        OnTimeUnitConfirmed?.Invoke(units);
    }

    //for debugging
    public void FireDayChanged()
    {
        OnDayChanged?.Invoke();
    }

    public void AddToTimeUnitTally(int amt)
    {
        ConfirmUnit(amt);
        timeUnitTally += amt;
        //OnTimeUnitTallyAdded?.Invoke(amt);
    }

    public void ConsumeTimeUnitTally()
    {
        ConsumeUnit(timeUnitTally);
        timeUnitTally = 0;
    }
}
