using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StallSlot : Slot
{
    [Header("Stall")]
    [SerializeField] protected float itemPrice;
    [SerializeField] bool itemPurchased;
    [SerializeField] TextMeshProUGUI priceTxt;
    [SerializeField] Button stallButton;
    [SerializeField] bool buyable;

    public Action<StallSlot> OnSlotClicked;

    int daysWaited = 0;
    bool waitingToBuy;
    public int DaysToWait { get; private set; }
    public CustomerReaction CustomerReaction { get; private set; }

    void Awake()
    {
        if (buyable) DayManager.Ins.OnDayChanged += HandleDayChanged;
    }

    void OnEnable()
    {
        UpdateSlot();
        stallButton.onClick.AddListener(SlotClicked);
    }

    void OnDisable()
    {
        stallButton.onClick.RemoveListener(SlotClicked);
    }

    void OnDestroy()
    {
        if (buyable) DayManager.Ins.OnDayChanged -= HandleDayChanged;
    }

    public override void UpdateSlot()
    {
        if (heldItem != null)
        {
            iconImage.enabled = true;

            if (!itemPurchased)
            {
                iconImage.sprite = heldItem.icon;
                amountTxt.text = itemAmount.ToString();
                priceTxt.text = "$" + itemPrice.ToString();
            }
            else
            {
                amountTxt.text = "";
                priceTxt.text = "";
            }
        }
        else
        {
            iconImage.enabled = false;
            amountTxt.text = "";
            priceTxt.text = "";
        }
    }

    public override void SetItem(ItemSO item, int amount = 1)
    {
        base.SetItem(item, amount);
        if (buyable) SetWaitingToBuy();
    }

    public override void ClearSlot()
    {
        heldItem = null;
        itemAmount = 0;
        itemPurchased = false;
        daysWaited = 0;
        waitingToBuy = false;
        UpdateSlot();
    }

    public void SetPurchased()
    {
        itemPurchased = true;
        CustomerReaction = heldItem.GetCustomerReaction(DaysToWait);
        iconImage.sprite = Data.CustomerReactions.GetSprite(CustomerReaction);
        UpdateSlot();
    }

    public void ClearPurchased()
    {
        itemPurchased = false;
        ClearSlot();
    }

    public void SetPrice(float price)
    {
        itemPrice = price;
        priceTxt.text = "$" + price.ToString();
        UpdateSlot();
        if (buyable) SetWaitingToBuy();
    }

    public bool IsPurchased()
    {
        return itemPurchased;
    }

    public float GetPrice()
    {
        return itemPrice;
    }

    protected virtual void SlotClicked()
    {
        if (heldItem != null)
        {
            OnSlotClicked?.Invoke(this);
        }
    }

    void HandleDayChanged()
    {
        if (!waitingToBuy)
        {
            return;
        }

        daysWaited++;

        if (daysWaited >= DaysToWait)
        {
            SetPurchased();
            waitingToBuy = false;
        }
    }

    void SetWaitingToBuy()
    {
        if (heldItem == null) return;

        DaysToWait = heldItem.GetDaysBeforePurchase(itemPrice / itemAmount, PlayerInventory.Instance.CurrentTown);
        if (DaysToWait >= 0)
        {
            waitingToBuy = true;
            daysWaited = 0;
        }
        else
        {
            waitingToBuy = false;
        }
    }
}

public enum CustomerReaction
{
    ANGRY, ANNOYED, OKAY, HAPPY, NONE
}