using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] float startingMoney;

    float money;

    public static MoneyManager Ins;
    public float Money => money;

    void Awake()
    {
        if (Ins != null && Ins != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Ins = this;
        }
    }

    void Start()
    {
        AddMoney(startingMoney);
    }

    public bool CanAfford(float cost)
    {
        return Money >= cost;
    }

    public void AddMoney(float amt)
    {
        money = Mathf.Max(0, money + amt);
        UIManager.Ins.UpdateMoneyUI(money);
    }

    public void HalfMoney()
    {
        money /= 2f;
        UIManager.Ins.UpdateMoneyUI(money);
    }
}