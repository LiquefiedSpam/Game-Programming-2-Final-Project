using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListingSlotActionsUI : SlotActionsUI
{
    [Header("Remove")]
    [SerializeField] GameObject removeParent;
    [SerializeField] TMP_Text cantRemoveText;
    [SerializeField] Button removeButton;
    [Header("Listing")]
    [SerializeField] GameObject priceParent;
    [SerializeField] TMP_InputField priceInput;
    [SerializeField] Button savePriceButton;
    [SerializeField] Button revertPriceButton;
    [Header("Sale")]
    [SerializeField] GameObject saleParent;
    [SerializeField] Image reactionImage;
    [SerializeField] TMP_Text reactionText;
    [SerializeField] Button profitButton;

    PlayerListingSlot listing;

    public override void Show(Slot s)
    {
        if (s is PlayerListingSlot playerListing)
        {
            listing = playerListing;
        }
        else return;

        if (listing.HasResult)
        {
            ShowListingResult();
        }
        else
        {
            ShowWaitingListing();
        }

        base.Show(s);
    }

    void ShowListingResult()
    {
        priceParent.SetActive(false);
        saleParent.SetActive(true);

        reactionImage.sprite = listing.ReactionSprite;

        if (listing.SaleResult == SaleResult.REJECTED)
        {
            reactionText.text = $"Sale price (${listing.ListedPrice}) rejected.";
            profitButton.gameObject.SetActive(false);
            removeParent.SetActive(true);
            SetRemoveButton();
        }
        else
        {
            reactionText.text = $"Sold for ${listing.ListedPrice} in {listing.ResultWaitTime.x} days and {listing.ResultWaitTime.y} hours!";
            removeParent.SetActive(false);
            profitButton.gameObject.SetActive(true);
            profitButton.onClick.AddListener(OnProfitClicked);
        }
    }

    void ShowWaitingListing()
    {
        saleParent.SetActive(false);
        profitButton.gameObject.SetActive(false);
        priceParent.SetActive(true);
        removeParent.SetActive(true);
        SetRemoveButton();

        priceInput.text = $"{listing.ListedPrice}";
        savePriceButton.onClick.AddListener(OnSavePriceClicked);
        revertPriceButton.onClick.AddListener(OnRevertPriceClicked);
    }

    void SetRemoveButton()
    {
        if (Inventory.Ins.HasRoomFor(listing))
        {
            cantRemoveText.text = "";
            removeButton.interactable = true;
            removeButton.onClick.AddListener(OnRemoveClicked);
        }
        else
        {
            cantRemoveText.text = "Not enough room to return items!";
            removeButton.interactable = false;
        }
    }

    protected override void UnsubscribeFromActions()
    {
        removeButton.onClick.RemoveAllListeners();
        profitButton.onClick.RemoveAllListeners();
        savePriceButton.onClick.RemoveAllListeners();
        revertPriceButton.onClick.RemoveAllListeners();
    }

    void OnRemoveClicked()
    {
        if (!Inventory.Ins.HasRoomFor(listing)) return;

        listing.CancelSale();
        Data.ClosestPlayerStall.Remove(listing);
        Inventory.Ins.Add(new(listing));
        Hide();
    }

    void OnProfitClicked()
    {
        MoneyManager.Ins.AddMoney(listing.ListedPrice);
        Data.ClosestPlayerStall.Remove(listing);
        Hide();
    }

    void OnSavePriceClicked()
    {
        if (float.TryParse(priceInput.text, out var price))
        {
            if (price >= PlayerListingSlot.MIN_PRICE)
            {
                listing.SetPrice(price);
                priceInput.text = price.ToString("F2");
                return;
            }
            else
            {
                priceInput.text = listing.ListedPrice.ToString("F2");
            }
        }
    }

    void OnRevertPriceClicked()
    {
        priceInput.text = listing.ListedPrice.ToString("F2");
    }
}