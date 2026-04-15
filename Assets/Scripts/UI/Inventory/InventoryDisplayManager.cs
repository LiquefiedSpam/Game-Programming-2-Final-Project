using UnityEngine;

public class InventoryDisplayManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject background;
    [Space]
    [SerializeField] SlotDetailsUI slotDetails;
    [SerializeField] SlotGroupUI inventoryUI;
    [SerializeField] SlotGroupUI stallInvUI;
    [SerializeField] SlotGroupUI merchantUI;
    [SerializeField] SlotGroupUI playerStallUI;

    public static InventoryDisplayManager Ins;

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
            background.SetActive(false);
            Hide();
        }
    }

    public void ShowPlayerStall(PlayerStall playerStall)
    {
        playerStallUI.ShowSlots(playerStall);
        stallInvUI.ShowSlots(inventory);
        Show();
    }

    public void HidePlayerStall()
    {
        playerStallUI.HideSlots();
        stallInvUI.HideSlots();
        Hide();
    }

    public void ShowMerchantStall(MerchantStall merchantStall)
    {
        merchantUI.ShowSlots(merchantStall);
        if (!inventoryUI.IsVisible) inventoryUI.ShowSlots(inventory);
        Show();
    }

    public void HideMerchantStall()
    {
        merchantUI.HideSlots();
        inventoryUI.HideSlots();
        Hide();
    }

    void Show()
    {
        background.SetActive(true);
    }

    void Hide()
    {
        if (slotDetails.IsVisible) slotDetails.HideSlotDetails();
        background.SetActive(false);
    }
}