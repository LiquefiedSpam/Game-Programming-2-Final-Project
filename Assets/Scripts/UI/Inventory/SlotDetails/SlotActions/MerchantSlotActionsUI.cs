using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantSlotActionsUI : SlotActionsUI
{
    [SerializeField] Button purchaseButton;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text cannotBuyText;

    public override void Show(Slot s)
    {
        slot = s;
        costText.text = "$" + s.GetMerchantPrice().ToString("F2");
        RefreshPurchasability();

        base.Show(s);
    }

    protected override void UnsubscribeFromActions()
    {
        purchaseButton.onClick.RemoveAllListeners();
        base.UnsubscribeFromActions();
    }

    void RefreshPurchasability()
    {
        bool canBuy = true;
        if (!MoneyManager.Ins.CanAfford(slot.GetMerchantPrice()))
        {
            cannotBuyText.text = "Cannot afford items!";
            canBuy = false;
        }
        else if (!Inventory.Ins.HasRoomFor(slot))
        {
            cannotBuyText.text = "Not enough room for items!";
            canBuy = false;
        }

        cannotBuyText.gameObject.SetActive(!canBuy);
        if (canBuy)
        {
            purchaseButton.interactable = true;
            purchaseButton.onClick.AddListener(OnPurchaseClicked);
        }
    }

    void OnPurchaseClicked()
    {
        MoneyManager.Ins.AddMoney(-slot.GetMerchantPrice());
        Inventory.Ins.Add(slot);

        purchaseButton.onClick.RemoveAllListeners();
        RefreshPurchasability();
    }
}