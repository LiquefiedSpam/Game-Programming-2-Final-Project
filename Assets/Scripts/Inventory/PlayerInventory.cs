using System;
using UnityEngine;

public class PlayerInventory : Inventory
{
    public static PlayerInventory Instance => instance;
    public static PlayerInventory instance;

    [SerializeField] float startMoney = 100f;
    public Action<float> OnMoneyChanged;
    public int CurrentTown { get; private set; } = 1;

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
        Teleport.OnTownChanged += TownChanged;
    }

    void OnDestroy()
    {
        Teleport.OnTownChanged -= TownChanged;
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

    public void HalfMoney()
    {
        AddMoney(-(money / 2));
    }

    public void TownChanged(int town)
    {
        Debug.Log($"Current town: {town}");
        CurrentTown = town;
    }
}