using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] Image titleAmountBg;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text amountText;
    [Space]
    [SerializeField] Image priceBg;
    [SerializeField] TMP_Text priceText;
    [Space]
    [SerializeField] Image itemImage;
    [SerializeField] Image outlineImage;
    [Space]
    [SerializeField] Button button;
    [SerializeField] EventTrigger eventTrigger;
    [SerializeField] RectTransform selectionAnchor;

    public Action<RectTransform> OnButtonClick;
    public Action OnHoverEnter;
    public Action OnHoverExit;

    public void SetSlot(Slot slot, SlotType slotType)
    {
        if (slot == null || slot.item == null)
        {
            SetEmptySlot();
            return;
        }

        titleAmountBg.enabled = true;
        titleText.text = slot.item.title;
        amountText.text = "x" + slot.amount.ToString();
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = slot.item.sprite;

        SetEventTriggers();

        switch (slotType)
        {
            case SlotType.PLAYER_INVENTORY:
            case SlotType.PLAYER_STALL_INV:
                priceBg.enabled = false;
                priceText.text = "";
                break;
            case SlotType.PLAYER_LISTING:
                SetPlayerListingSlot(slot);
                break;
            case SlotType.MERCHANT_LISTING:
                priceBg.enabled = true;
                priceText.text = "$" + slot.GetMerchantPrice().ToString("F2");
                break;
        }
    }

    void SetEventTriggers()
    {
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry enter = new() { eventID = EventTriggerType.PointerEnter };
        enter.callback.AddListener((_) => OnHoverEnter?.Invoke());
        eventTrigger.triggers.Add(enter);

        EventTrigger.Entry exit = new() { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener((_) => OnHoverExit?.Invoke());
        eventTrigger.triggers.Add(exit);

        button.onClick.AddListener(() => OnButtonClick?.Invoke(selectionAnchor));
    }

    void SetEmptySlot()
    {
        button.enabled = false;
        itemImage.gameObject.SetActive(false);

        titleAmountBg.enabled = false;
        titleText.text = "";
        amountText.text = "";

        priceBg.enabled = false;
        priceText.text = "";
    }

    void SetPlayerListingSlot(Slot slot)
    {
        if (slot is PlayerListingSlot listing)
        {
            priceBg.enabled = true;
            priceText.text = "$" + listing.ListedPrice.ToString("F2");
            if (listing.HasSaleResult)
            {
                itemImage.sprite = Data.CustomerReactions.GetReactionSprite(listing.CustomerReaction);
            }
        }
    }

    public enum SlotType
    {
        PLAYER_INVENTORY, PLAYER_STALL_INV, PLAYER_LISTING, MERCHANT_LISTING
    }
}