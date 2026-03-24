using UnityEngine;
using UnityEngine.EventSystems;

public class MerchantStallUI : MonoBehaviour
{
    [SerializeField] StallSlot[] slots;

    public void Show()
    {
        foreach (var s in slots) s.UpdateSlot();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}