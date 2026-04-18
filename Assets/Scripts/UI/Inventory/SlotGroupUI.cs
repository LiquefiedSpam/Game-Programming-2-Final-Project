using UnityEngine;

public class SlotGroupUI : MonoBehaviour
{
    public const int MAX_SLOTS = 5;

    [SerializeField] public SlotUI.SlotType SlotType;
    [SerializeField] protected GameObject root;
    [SerializeField] protected GameObject slotPrefab;
    [SerializeField] protected Transform slotParent;
    [SerializeField] protected SlotDetailsUI slotDetails;

    public bool IsVisible => root.activeSelf;

    protected Slot selectedSlot;
    protected SlotGroup activeSlots;

    public virtual void ShowSlots(SlotGroup slots)
    {
        activeSlots = slots;
        activeSlots.OnSlotGroupChanged += RefreshSlots;

        RefreshSlots();
        root.SetActive(true);
    }

    public virtual void HideSlots()
    {
        if (activeSlots != null)
        {
            activeSlots.OnSlotGroupChanged -= RefreshSlots;
            activeSlots = null;
        }
        root.SetActive(false);
    }

    void RefreshSlots()
    {
        DestroySlots();
        if (activeSlots == null) return;
        for (int i = 0; i < Mathf.Min(MAX_SLOTS, activeSlots.Count); i++)
        {
            Slot slot = activeSlots.GetAt(i);
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            slotObj.name = "Slot_" + (slot == null ? "EMPTY" : slot.item.title);

            if (!slotObj.TryGetComponent<SlotUI>(out var slotUI)) return;

            slotUI.SetSlot(slot, SlotType);

            // slotUI.OnHoverEnter += () => SlotHoverEnter(slot);
            // slotUI.OnHoverExit += () => SlotHoverExit();
            slotUI.OnButtonClick += _ => SlotClicked(_, slot);
        }

        if (selectedSlot != null)
        {
            if (activeSlots.Contains(selectedSlot))
            {
                if (slotDetails.IsVisible) slotDetails.RefreshDetails();
                else slotDetails.ShowSlotDetails(selectedSlot, SlotType);
            }
            else
            {
                slotDetails.HideSlotDetails();
                selectedSlot = null;
            }
        }
    }

    protected virtual void SlotHoverEnter(Slot s)
    {
        if (selectedSlot == null)
        {
            slotDetails.ShowSlotDetails(s, SlotType);
        }
    }

    protected virtual void SlotHoverExit()
    {
        if (selectedSlot == null)
        {
            slotDetails.HideSlotDetails();
        }
    }

    protected virtual void SlotClicked(RectTransform selectionAnchor, Slot s)
    {
        Debug.Log("slot details clicked");
        selectedSlot = s;
        slotDetails.ShowSlotDetails(s, SlotType);
        slotDetails.OnCloseDetails += ExitDetails;
        InventoryDisplayManager.Ins.SetSelected(selectionAnchor);
    }

    protected virtual void ExitDetails()
    {
        slotDetails.OnCloseDetails -= ExitDetails;
        selectedSlot = null;
        InventoryDisplayManager.Ins.HideSelected();
    }

    protected virtual void DestroySlots()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }
    }
}