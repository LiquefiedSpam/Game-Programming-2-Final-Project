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
    [SerializeField] Sprite purchasedSprite;
    [SerializeField] Button stallButton;
    [SerializeField] CustomerReactions reactionSprites;
    [SerializeField] bool buyable;
    [SerializeField] bool debug;

    public Action<StallSlot> OnSlotClicked;

    int daysWaited = 0;
    bool waitingToBuy;
    public int DaysToWait;
    public CustomerReaction CustomerReaction;

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
                // iconImage.sprite = purchasedSprite;
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
        if (debug) Debug.Log("Clear slot");
        heldItem = null;
        itemAmount = 0;
        itemPurchased = false;
        daysWaited = 0;
        waitingToBuy = false;
        UpdateSlot();
    }

    public void SetPurchased()
    {
        if (debug) Debug.Log("Set purchased");
        itemPurchased = true;
        CustomerReaction = heldItem.GetCustomerReaction(DaysToWait);
        iconImage.sprite = Data.CustomerReactions.GetSprite(CustomerReaction);
        UpdateSlot();
    }

    public void ClearPurchased()
    {
        if (debug) Debug.Log("Clear purchased");
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
            if (debug) Debug.Log("Not waiting to buy");
            return;
        }

        daysWaited++;

        if (daysWaited >= DaysToWait)
        {
            if (debug) Debug.Log("Apparently set purchase got called");
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
            if (debug) Debug.Log($"Get wait time successful, wait to buy = {DaysToWait}");

            waitingToBuy = true;
            daysWaited = 0;
        }
        else
        {
            if (debug) Debug.Log($"Get wait time failed, not waiting to buy");
            waitingToBuy = false;
        }
    }
}

public enum CustomerReaction
{
    ANGRY, ANNOYED, OKAY, HAPPY, NONE
}