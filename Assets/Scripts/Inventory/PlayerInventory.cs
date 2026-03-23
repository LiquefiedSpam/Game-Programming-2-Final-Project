using UnityEngine;

public class PlayerInventory : Inventory
{
    public static PlayerInventory Instance => instance;
    public static PlayerInventory instance;

    [SerializeField] float startMoney = 10f;

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
        money = startMoney;
    }

    public bool TryPurchase(float price, ItemSO item, int amount)
    {
        if (price > money) return false;
        AddItem(item, amount);
        return true;
    }
}