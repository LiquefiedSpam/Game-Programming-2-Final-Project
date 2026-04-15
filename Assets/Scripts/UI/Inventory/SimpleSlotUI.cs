using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleSlotUI : MonoBehaviour
{
    [SerializeField] Image slotImage;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text amountText;
    [SerializeField] TMP_Text descriptionText;

    public void SetSlot(Slot s)
    {
        slotImage.sprite = s.item.sprite;
        titleText.text = s.item.title;
        amountText.text = "x" + s.amount.ToString();
        descriptionText.text = $"Purchase from {s.item.merchant} in {s.item.town} for ${s.item.merchantSalePrice} each.";
    }
}