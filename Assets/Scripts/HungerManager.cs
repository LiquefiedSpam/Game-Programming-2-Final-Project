using System;
using UnityEngine;

public class HungerManager : MonoBehaviour
{

    [SerializeField] private float maxHunger = 100;
    private float minHunger = 0;


    public float CurrentHunger => currentHunger;
    public float BaseHungerRemoveRate => baseHungerRemoveRate;
    public float BaseHungerRemoveAmtModifier => baseHungerRemoveRateModifier;
    private float currentHunger = 100;
    private float baseHungerRemoveRate = 2;
    private float baseHungerRemoveRateModifier = 1;

    public event Action OnHungerChanged;
    public event Action OnHungerKnockedOut;

    public static HungerManager Ins => _instance;
    private static HungerManager _instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DayManager.Ins.OnUnitsConsumed += ConsumedUnitsToHunger;
    }

    void OnDestroy()
    {
        if (DayManager.Ins != null)
            DayManager.Ins.OnUnitsConsumed -= ConsumedUnitsToHunger;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ConsumedUnitsToHunger(int units)
    {
        float hungerToRemove = units * (baseHungerRemoveRate * baseHungerRemoveRateModifier);
        ModifyHungerByPercent(hungerToRemove);
    }

    void ModifyHungerByAmt(float amt)
    {
        if (currentHunger + amt > maxHunger)
        {
            Debug.Log("Hit max hunger!");
            amt = maxHunger;
            OnHungerChanged?.Invoke();
            return;
        }
        else if (currentHunger + amt < minHunger)
        {
            Debug.Log("Knocked out!");
            amt = minHunger;
            OnHungerKnockedOut?.Invoke();
            OnHungerChanged?.Invoke();
            return;
        }

        currentHunger += amt;
        OnHungerChanged?.Invoke();
    }

    //modifies hunger by a percent based on max hunger.
    public void ModifyHungerByPercent(float pct)
    {
        float amtToModify = (pct / 100) * maxHunger;
        ModifyHungerByAmt(amtToModify);
    }
}
