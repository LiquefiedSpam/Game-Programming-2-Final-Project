using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotDetailsUI : MonoBehaviour
{
    [SerializeField] protected GameObject root;
    [SerializeField] protected SimpleSlotUI simpleSlotUI;
    [SerializeField] protected ItemHistoryUI itemHistory;

    [Header("Slot Actions")]
    [SerializeField] InventorySlotActionsUI inventorySlotActions;
    [SerializeField] MerchantSlotActionsUI merchantSlotActions;
    [SerializeField] PlayerStallInvActionsUI playerStallInvActions;
    [SerializeField] PlayerListingSlotActionsUI playerListingSlotActions;

    [Header("Buttons")]
    [SerializeField] protected Button exitButton;
    [SerializeField] protected Button actionsButton;
    [SerializeField] protected Button historyButton;

    protected SlotActionsUI activeSlotActions;
    protected Slot activeSlot;

    public Action OnCloseDetails;
    public bool IsVisible => root.activeSelf;

    public void ShowSlotDetails(Slot slot, SlotUI.SlotType slotType)
    {
        if (activeSlot != null || activeSlotActions != null)
        {
            DeselectActiveSlot();
        }
        activeSlot = slot;
        simpleSlotUI.SetSlot(activeSlot);
        exitButton.onClick.AddListener(ExitSlotDetails);

        SetActiveSlotActions(slotType);
        activeSlotActions.Show(slot);
        itemHistory.gameObject.SetActive(false);

        actionsButton.onClick.AddListener(OnActionsClicked);
        historyButton.onClick.AddListener(OnHistoryClicked);
        actionsButton.interactable = false;
        historyButton.interactable = true;

        root.SetActive(true);
    }

    public void HideSlotDetails()
    {
        DeselectActiveSlot();
        root.SetActive(false);
    }

    public void RefreshDetails()
    {
        if (activeSlot == null)
        {
            return;
        }
        simpleSlotUI.SetSlot(activeSlot);
    }

    void SetActiveSlotActions(SlotUI.SlotType slotType)
    {
        activeSlotActions = slotType switch
        {
            SlotUI.SlotType.MERCHANT_LISTING => merchantSlotActions,
            SlotUI.SlotType.PLAYER_LISTING => playerListingSlotActions,
            SlotUI.SlotType.PLAYER_STALL_INV => playerStallInvActions,
            _ => inventorySlotActions
        };
    }

    void ClearSlot()
    {
        if (activeSlotActions != null)
        {
            activeSlotActions.Hide();
            activeSlotActions = null;
        }
        if (activeSlot != null)
        {
            // activeSlot.OnSlotChanged -= RefreshDetails;
            activeSlot = null;
        }
        exitButton.onClick.RemoveAllListeners();
        actionsButton.onClick.RemoveAllListeners();
        historyButton.onClick.RemoveAllListeners();
    }

    void DeselectActiveSlot()
    {
        OnCloseDetails?.Invoke();
        ClearSlot();
    }

    void ExitSlotDetails()
    {
        OnCloseDetails?.Invoke();
        HideSlotDetails();
    }

    void OnHistoryClicked()
    {
        activeSlotActions.Hide();
        historyButton.interactable = false;
        actionsButton.interactable = true;
        itemHistory.Show(activeSlot.item);
    }

    void OnActionsClicked()
    {
        activeSlotActions.Show(activeSlot);
        itemHistory.Hide();
        historyButton.interactable = true;
        actionsButton.interactable = false;
    }
}