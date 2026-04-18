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
    [SerializeField] TMP_InputField pricePerItemInput;
    [SerializeField] TMP_Text totalPriceText;
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

        if (listing.HasSaleResult)
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

        reactionImage.sprite = Data.CustomerReactions.GetReactionSprite(listing.CustomerReaction);

        if (!listing.Sold)
        {
            reactionText.text = $"Sale price ($" + listing.ListedPrice.ToString("F2") + ") rejected.";
            profitButton.gameObject.SetActive(false);
            removeParent.SetActive(true);
            SetRemoveButton();
        }
        else
        {
            reactionText.text = "Sold for $" + listing.ListedPrice.ToString("F2") + "($" + listing.ListedPricePerItem.ToString("F2") + " / item)";
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

        pricePerItemInput.text = listing.ListedPricePerItem.ToString("F2");
        pricePerItemInput.onEndEdit.AddListener(_ => OnPricePerItemEdited());
        totalPriceText.text = listing.ListedPrice.ToString("F2");
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
        pricePerItemInput.onEndEdit.RemoveAllListeners();
    }

    void OnRemoveClicked()
    {
        if (!Inventory.Ins.HasRoomFor(listing)) return;

        listing.Cancel();
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

    void OnPricePerItemEdited()
    {
        if (float.TryParse(pricePerItemInput.text, out var price))
        {
            if (price == listing.ListedPricePerItem) return;

            if (price >= PlayerListingSlot.MIN_PRICE_PER_ITEM)
            {
                listing.SetPricePerItem(price);
                pricePerItemInput.text = price.ToString("F2");
            }
            else
            {
                pricePerItemInput.text = listing.ListedPricePerItem.ToString("F2");
            }

            totalPriceText.text = listing.ListedPrice.ToString("F2");
        }
    }

    void OnSavePriceClicked()
    {
        if (float.TryParse(pricePerItemInput.text, out var price))
        {
            if (price >= PlayerListingSlot.MIN_PRICE_PER_ITEM)
            {
                listing.SetPricePerItem(price);
                pricePerItemInput.text = price.ToString("F2");
                return;
            }
            else
            {
                pricePerItemInput.text = listing.ListedPrice.ToString("F2");
            }
        }
    }

    void OnRevertPriceClicked()
    {
        pricePerItemInput.text = listing.ListedPrice.ToString("F2");
    }
}