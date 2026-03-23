using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListingUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemTitle;
    [SerializeField] TextMeshProUGUI itemAmount;
    [SerializeField] Slider itemPriceSlider;
    [SerializeField] TextMeshProUGUI itemPriceTxt;
    [SerializeField] Button removeButton;
    [SerializeField] Button saveButton;

    StallSlot currentSlot;

    public void Show(StallSlot slot)
    {
        currentSlot = slot;

        itemImage.enabled = true;
        itemImage.sprite = slot.GetItem().icon;
        itemAmount.text = slot.GetAmount().ToString();

        saveButton.onClick.AddListener(SaveChanges);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void SaveChanges()
    {
        currentSlot.SetPrice(itemPriceSlider.value);
    }

    public void RemoveListing()
    {
        PlayerInventory.Instance.AddItem(currentSlot.GetItem(), currentSlot.GetAmount());
        currentSlot.ClearSlot();
        Hide();
    }
}