using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotActionsUI : SlotActionsUI
{
    // TODO whatever fields needed to change hunger
    [SerializeField] protected Button consumeButton;
    [SerializeField] protected TMP_Text healthText;

    public override void Show(Slot s)
    {
        if (s.item.consumable)
        {
            consumeButton.gameObject.SetActive(true);
            if (HungerManager.Ins.CanConsume())
            {
                healthText.SetText($"+{s.item.healthIncrease} health");
                consumeButton.interactable = true;
                consumeButton.onClick.AddListener(OnConsumeClicked);
            }
            else
            {
                healthText.text = "Cannot eat at full health!";
                consumeButton.interactable = false;
            }
        }
        else
        {
            consumeButton.gameObject.SetActive(false);
            healthText.text = "";
        }
        base.Show(s);
    }

    protected override void UnsubscribeFromActions()
    {
        consumeButton.onClick.RemoveAllListeners();
        base.UnsubscribeFromActions();
    }

    void OnConsumeClicked()
    {
        // TODO health stuff actually work
        float healthGained = slot.item.healthIncrease;
        HungerManager.Ins.ModifyHungerByAmt(slot.item.healthIncrease);

        if (slot.SubtractAmount(1) == 0)
        {
            Inventory.Ins.Remove(slot);
            Hide();
        }
        else if (!HungerManager.Ins.CanConsume())
        {
            consumeButton.onClick.RemoveAllListeners();
            consumeButton.interactable = false;
            healthText.text = "Cannot eat at full health!";
        }
    }
}