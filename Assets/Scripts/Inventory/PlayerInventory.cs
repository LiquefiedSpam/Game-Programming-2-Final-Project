using System;
using UnityEngine;

public class PlayerInventory : Inventory
{
    public static PlayerInventory Instance => instance;
    public static PlayerInventory instance;

    [SerializeField] float startMoney = 10f;
    public Action<float> OnMoneyChanged;

    float money;
    public float Money => money;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        money = startMoney;
        UIManager.Ins.UpdateMoneyUI(money);
    }

    public bool TryPurchaseItem(float price, ItemSO item, int amount)
    {
        if (price > money) return false;
        AddItem(item, amount);
        AddMoney(-price);
        return true;
    }

    public void AddMoney(float amt)
    {
        money += amt;
        UIManager.Ins.UpdateMoneyUI(money);
        OnMoneyChanged?.Invoke(money);
    }
}