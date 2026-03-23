using TMPro;
using UnityEngine;

public class StallSlot : Slot
{
    [Header("Stall")]
    [SerializeField] float itemPrice;
    [SerializeField] bool itemPurchased;
    [SerializeField] TextMeshProUGUI priceTxt;
    [SerializeField] Sprite purchasedSprite;

    public override void UpdateSlot()
    {
        if (heldItem != null)
        {
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
}