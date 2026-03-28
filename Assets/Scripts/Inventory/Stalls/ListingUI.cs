using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListingUI : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemTitle;
    [SerializeField] Button closeButton;
    [Header("Active Listing")]
    [SerializeField] GameObject activeListingParent;
    [SerializeField] TMP_InputField itemPriceInput;
    [SerializeField] Button removeButton;
    [SerializeField] Button saveButton;
    [Header("Purchased Listing")]
    [SerializeField] GameObject purchaseListingParent;
    [SerializeField] Image reactionImage;
    [SerializeField] Button profitButton;
    [SerializeField] TextMeshProUGUI priceText;

    StallSlot currentSlot;

    public Action OnListingRemoved;
    public Action OnUIClosed;

    public void Show(StallSlot slot)
    {
        if (slot.IsPurchased()) ShowUIForPurchasedListing(slot);
        else ShowUIForActiveListing(slot);

        itemImage.enabled = true;
        itemImage.sprite = slot.GetItem().icon;
        itemTitle.text = slot.GetItem().itemName + "  x" + slot.GetAmount().ToString();

        gameObject.SetActive(true);
    }

    public void Hide(bool invokeAction = false)
    {
        if (invokeAction) OnUIClosed?.Invoke();

        gameObject.SetActive(false);
        saveButton.onClick.RemoveListener(SaveChanges);
        removeButton.onClick.RemoveListener(RemoveListing);
        closeButton.onClick.RemoveListener(CloseWindow);
        profitButton.onClick.RemoveListener(GainProfit);

        currentSlot = null;
        OnListingRemoved = null;
        OnUIClosed = null;
    }

    void OnDisable()
    {
        Hide();
    }

    void ShowUIForActiveListing(StallSlot slot)
    {
        if (currentSlot == null)
        {
            // we aren't already subscribed to these
            saveButton.onClick.AddListener(SaveChanges);
            removeButton.onClick.AddListener(RemoveListing);
            closeButton.onClick.AddListener(CloseWindow);
        }
        else
        {
            // resubscribe to these in PlayerStallUI.ShowListingUI after this method is called
            OnListingRemoved = null;
            OnUIClosed = null;
        }

        currentSlot = slot;

        purchaseListingParent.SetActive(false);
        activeListingParent.SetActive(true);

        itemPriceInput.text = slot.GetPrice().ToString();
    }

    void ShowUIForPurchasedListing(StallSlot slot)
    {
        if (currentSlot == null)
        {
            saveButton.onClick.AddListener(CloseWindow);
            profitButton.onClick.AddListener(GainProfit);
        }
        else
        {
            OnListingRemoved = null;
            OnUIClosed = null;
        }

        currentSlot = slot;

        activeListingParent.SetActive(false);
        purchaseListingParent.SetActive(true);

        reactionImage.sprite = Data.CustomerReactions.GetSprite(slot.CustomerReaction);
        priceText.text = $"Sold for ${slot.GetPrice()}";
    }

    void SaveChanges()
    {
        if (float.TryParse(itemPriceInput.text, out float newPrice))
        {
            currentSlot.SetPrice(newPrice);
        }
        Hide(true);
    }

    public void RemoveListing()
    {
        PlayerInventory.Instance.AddItem(currentSlot.GetItem(), currentSlot.GetAmount());
        currentSlot.ClearSlot();
        OnListingRemoved?.Invoke();
        Hide(true);
    }

    void CloseWindow()
    {
        gameObject.SetActive(false);
        Hide(true);
    }

    void GainProfit()
    {
        PlayerInventory.Instance.AddMoney(currentSlot.GetPrice());
        currentSlot.ClearSlot();
        Hide(true);
    }
}