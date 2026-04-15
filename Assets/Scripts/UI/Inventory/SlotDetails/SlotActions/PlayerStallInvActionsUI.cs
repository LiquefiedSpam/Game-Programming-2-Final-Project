using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStallInvActionsUI : InventorySlotActionsUI
{
    [Header("Create Listing")]
    [SerializeField] Slider amountSlider;
    [SerializeField] TMP_Text sliderText;
    [SerializeField] TMP_InputField priceInput;
    [SerializeField] Button createListingButton;
    [SerializeField] TMP_Text noOpenSlotText;

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
        priceInput.text = "1.00";

        if (Data.ClosestPlayerStall.CanAddListing())
        {
            createListingButton.interactable = true;
            createListingButton.onClick.AddListener(OnCreateListingClicked);
            amountSlider.interactable = true;
            amountSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            sliderText.text = "1";
            noOpenSlotText.text = "";
        }
        else
        {
            createListingButton.interactable = false;
            amountSlider.interactable = false;
            sliderText.text = "0";
            noOpenSlotText.text = "No room for another sale!";
        }
    }

    void OnCreateListingClicked()
    {
        if (float.TryParse(priceInput.text, out var price))
        {
            int sellingAmount = (int)amountSlider.value;
            PlayerListingSlot listing = new(slot.item, sellingAmount, price, Data.CurrentTown); // TODO actually fix
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
    }

    void OnSliderValueChanged()
    {
        int sliderValue = (int)amountSlider.value;
        if (sliderText.text != sliderValue.ToString())
        {
            sliderText.text = sliderValue.ToString();
        }
    }

    protected override void UnsubscribeFromActions()
    {
        createListingButton.onClick.RemoveAllListeners();
        amountSlider.onValueChanged.RemoveAllListeners();
        base.UnsubscribeFromActions();
    }
}