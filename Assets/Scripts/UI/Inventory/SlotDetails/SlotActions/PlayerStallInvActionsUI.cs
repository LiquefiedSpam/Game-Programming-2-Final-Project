using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStallInvActionsUI : InventorySlotActionsUI
{
    [Header("Create Listing")]
    [SerializeField] Slider amountSlider;
    [SerializeField] TMP_Text sliderText;
    [SerializeField] TMP_InputField pricePerItemInput;
    [SerializeField] TMP_Text totalPriceText;
    [SerializeField] Button createListingButton;
    [SerializeField] TMP_Text noOpenSlotText;

    float pricePerItem;

    public override void Show(Slot s)
    {
        slot = s;
        SetCreateListingDisplay();
        base.Show(s);
    }

    void SetCreateListingDisplay()
    {
        amountSlider.maxValue = slot.amount;
        amountSlider.minValue = 1;
        amountSlider.value = 1;
        amountSlider.wholeNumbers = true;
        pricePerItemInput.text = PlayerListingSlot.MIN_PRICE_PER_ITEM.ToString("F2");
        totalPriceText.text = pricePerItemInput.text;
        pricePerItem = PlayerListingSlot.MIN_PRICE_PER_ITEM;

        if (Data.ClosestPlayerStall.CanAddListing())
        {
            createListingButton.interactable = true;
            createListingButton.onClick.AddListener(OnCreateListingClicked);
            amountSlider.interactable = true;
            amountSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            pricePerItemInput.interactable = true;
            pricePerItemInput.onEndEdit.AddListener(_ => OnPricePerItemChanged());
            sliderText.text = "1";
            noOpenSlotText.text = "";
        }
        else
        {
            createListingButton.interactable = false;
            amountSlider.interactable = false;
            pricePerItemInput.interactable = false;
            sliderText.text = "0";
            noOpenSlotText.text = "No room for another sale!";
        }
    }

    void OnCreateListingClicked()
    {
        int sellingAmount = (int)amountSlider.value;
        PlayerListingSlot listing = new(slot.item, sellingAmount, pricePerItem * sellingAmount, Data.CurrentTown);
        Data.ClosestPlayerStall.Add(listing);

        if (slot.SubtractAmount(sellingAmount) == 0)
        {
            Inventory.Ins.Remove(slot);
            Hide();
        }
        else
        {
            createListingButton.onClick.RemoveAllListeners();
            amountSlider.onValueChanged.RemoveAllListeners();
            SetCreateListingDisplay();
        }
    }

    void OnSliderValueChanged()
    {
        int sliderValue = (int)amountSlider.value;
        if (sliderText.text != sliderValue.ToString())
        {
            sliderText.text = sliderValue.ToString();
            totalPriceText.text = (pricePerItem * sliderValue).ToString("F2");
        }
    }

    void OnPricePerItemChanged()
    {
        float prevPricePerItem = pricePerItem;
        SetPricePerItem();

        if (prevPricePerItem != pricePerItem)
        {
            int sliderValue = (int)amountSlider.value;
            totalPriceText.text = (pricePerItem * sliderValue).ToString("F2");
        }
    }

    protected override void UnsubscribeFromActions()
    {
        createListingButton.onClick.RemoveAllListeners();
        amountSlider.onValueChanged.RemoveAllListeners();
        pricePerItemInput.onEndEdit.RemoveAllListeners();
        base.UnsubscribeFromActions();
    }

    void SetPricePerItem()
    {
        if (float.TryParse(pricePerItemInput.text, out var price))
        {
            pricePerItem = price;
        }
    }
}