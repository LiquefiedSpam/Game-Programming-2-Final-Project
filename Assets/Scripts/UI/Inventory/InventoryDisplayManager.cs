using UnityEngine;

public class InventoryDisplayManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject background;
    [Space]
    [SerializeField] SlotDetailsUI slotDetails;
    [SerializeField] SlotGroupUI inventoryUI;
    [SerializeField] SlotGroupUI stallInvUI;
    [SerializeField] MerchantStallUI merchantUI;
    [SerializeField] SlotGroupUI playerStallUI;
    [Space]
    [SerializeField] RectTransform selectedSlot;

    public static InventoryDisplayManager Ins;

    public bool StallDisplayVisible => stallInvUI.IsVisible || merchantUI.IsVisible;
    public bool InventoryDisplayVisible => inventoryUI.IsVisible;

    void Awake()
    {
        if (Ins != null && Ins != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Ins = this;
        }
    }

    public void SetInventoryVisibility(bool visible)
    {
        if (visible)
        {
            inventoryUI.ShowSlots(inventory);
            Show();
        }
        else
        {
            inventoryUI.HideSlots();
            Hide();
        }
    }

    public void ShowPlayerStall(PlayerStall playerStall)
    {
        playerStallUI.ShowSlots(playerStall);
        stallInvUI.ShowSlots(inventory);
        if (inventoryUI.IsVisible)
        {
            SetInventoryVisibility(false);
        }
        Show();
    }

    public void HidePlayerStall()
    {
        playerStallUI.HideSlots();
        stallInvUI.HideSlots();
        Hide();
    }

    public void ShowMerchantStall(MerchantStall merchantStall, string merchantName, string merchantMessage, Sprite merchantPortrait)
    {
        merchantUI.ShowSlots(merchantStall);
        merchantUI.ShowMerchantInfo(merchantPortrait, merchantName, merchantMessage);
        if (!inventoryUI.IsVisible)
        {
            inventoryUI.ShowSlots(inventory);
        }
        Show();
    }

    public void HideMerchantStall()
    {
        merchantUI.HideSlots();
        inventoryUI.HideSlots();
        Hide();
    }

    public void SetSelected(RectTransform selectedSlotTransform)
    {
        selectedSlot.gameObject.SetActive(true);
        selectedSlot.position = selectedSlotTransform.position;
    }

    public void HideSelected()
    {
        selectedSlot.gameObject.SetActive(false);
    }

    void Show()
    {
        // background.SetActive(true);
    }

    void Hide()
    {
        if (slotDetails.IsVisible) slotDetails.HideSlotDetails();
        selectedSlot.gameObject.SetActive(false);
        // background.SetActive(false);
    }
}