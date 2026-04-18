using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MerchantStallUI : SlotGroupUI
{
    [SerializeField] Image merchantPortrait;
    [SerializeField] TMP_Text merchantMessage;
    [SerializeField] TMP_Text merchantStallTitle;

    public void ShowMerchantInfo(Sprite portrait, string merchantName, string message)
    {
        merchantPortrait.sprite = portrait;
        merchantMessage.text = message;
        merchantStallTitle.text = $"{merchantName}'s Stall";
    }
}