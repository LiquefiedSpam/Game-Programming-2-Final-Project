using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListingUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemTitle;
    [SerializeField] TMP_InputField itemPriceInput;
    [SerializeField] Button removeButton;
    [SerializeField] Button saveButton;
    [SerializeField] Button closeButton;

    StallSlot currentSlot;

    public Action OnListingRemoved;
    public Action OnUIClosed;

    public void Show(StallSlot slot)
    {
        currentSlot = slot;

        itemImage.enabled = true;
        itemImage.sprite = slot.GetItem().icon;
        itemTitle.text = slot.GetItem().itemName + "  x" + slot.GetAmount().ToString();
        itemPriceInput.text = slot.GetPrice().ToString();

        saveButton.onClick.AddListener(SaveChanges);
        removeButton.onClick.AddListener(RemoveListing);
        closeButton.onClick.AddListener(CloseWindow);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        // OnUIClosed?.Invoke();

        gameObject.SetActive(false);
        saveButton.onClick.RemoveListener(SaveChanges);
        removeButton.onClick.RemoveListener(RemoveListing);
        closeButton.onClick.RemoveListener(CloseWindow);

        currentSlot = null;
        OnListingRemoved = null;
        OnUIClosed = null;
    }

    void OnDisable()
    {
        Hide();
    }

    void SaveChanges()
    {
        if (float.TryParse(itemPriceInput.text, out float newPrice))
        {
            currentSlot.SetPrice(newPrice);
        }
        Hide();
    }

    public void RemoveListing()
    {
        PlayerInventory.Instance.AddItem(currentSlot.GetItem(), currentSlot.GetAmount());
        currentSlot.ClearSlot();
        OnListingRemoved?.Invoke();
        Hide();
    }

    void CloseWindow()
    {
        Hide();
    }
}