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

    public Action<StallSlot> OnSlotClicked;

    void OnEnable()
    {
        UpdateSlot();
        if (heldItem != null)
        {
            Debug.Log("Add listener");
            stallButton.onClick.AddListener(SlotClicked);
        }
    }

    void OnDisable()
    {
        stallButton.onClick.RemoveListener(SlotClicked);
    }

    public override void UpdateSlot()
    {
        if (heldItem != null)
        {
            Debug.Log("Held item is not null");
            iconImage.enabled = true;

            if (!itemPurchased)
            {
                iconImage.sprite = heldItem.icon;
                amountTxt.text = itemAmount.ToString();
                priceTxt.text = "";
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
            Debug.Log("Held item is null");
            iconImage.enabled = false;
            amountTxt.text = "";
            priceTxt.text = "";
        }
    }

    public override void ClearSlot()
    {
        heldItem = null;
        itemAmount = 0;
        itemPurchased = false;
        UpdateSlot();
    }

    public void SetPurchased(bool purchased)
    {
        itemPurchased = purchased;
        UpdateSlot();
    }

    public void SetPrice(float price)
    {
        itemPrice = price;
        UpdateSlot();
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
        OnSlotClicked?.Invoke(this);
    }
}