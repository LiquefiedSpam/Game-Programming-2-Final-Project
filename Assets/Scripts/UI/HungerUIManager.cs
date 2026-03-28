using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HungerUIManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private TMP_Text hungerText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HungerManager.Ins.OnHungerChanged += Refresh;
        Refresh();
    }

    void OnDestroy()
    {
        if (HungerManager.Ins != null)
            HungerManager.Ins.OnHungerChanged -= Refresh;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Refresh()
    {
        float currentHunger = HungerManager.Ins.CurrentHunger;
        hungerSlider.value = currentHunger;

        hungerText.text = ((int)currentHunger).ToString();
        Debug.Log(currentHunger);
    }
}
