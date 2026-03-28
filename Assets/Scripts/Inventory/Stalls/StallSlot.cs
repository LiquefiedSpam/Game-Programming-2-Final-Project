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

    public Action<StallSlot> OnSlotClicked;

    (int, DayInterval) timePosted;
    bool waitingToBuy;
    public Vector2Int WaitToBuy;
    public CustomerReaction CustomerReaction;

    void Awake()
    {
        if (buyable) DayManager.Ins.OnTimeChanged += HandleTimeChanged;
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
        if (buyable) DayManager.Ins.OnTimeChanged -= HandleTimeChanged;
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
                iconImage.sprite = purchasedSprite;
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
        UpdateSlot();
        if (buyable) waitingToBuy = false;
    }

    public void SetPurchased()
    {
        Debug.Log("Set purchased");
        itemPurchased = true;
        CustomerReaction = Data.GetReaction(GetItem(), WaitToBuy);
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

    void HandleTimeChanged()
    {
        if (!waitingToBuy)
        {
            Debug.Log("Not waiting to buy");
            return;
        }

        Vector2Int timePassed = DayManager.Ins.TimePassed(timePosted);
        if (timePassed.x > WaitToBuy.x
            || (timePassed.x == WaitToBuy.x && timePassed.y >= WaitToBuy.y))
        {
            SetPurchased();
            waitingToBuy = false;
        }
    }

    void SetWaitingToBuy()
    {
        if (GetItem() == null) return;

        if (Data.TryGetPurchaseTime(GetItem(), itemPrice / GetAmount(), out WaitToBuy))
        {
            Debug.Log($"Try get purchased tim successful, wait to buy = {WaitToBuy}");
            timePosted = (DayManager.Ins.Day, DayManager.Ins.DayInterval);
            waitingToBuy = true;
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