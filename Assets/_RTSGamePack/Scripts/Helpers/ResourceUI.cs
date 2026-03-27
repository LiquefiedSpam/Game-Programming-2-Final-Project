using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [Header("Resource UI Text Elements")]
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text goldText;


    private void Start()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnResourcesChanged += UpdateDisplay;
        }

        UpdateDisplay();
    }

    private void OnDestroy()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnResourcesChanged -= UpdateDisplay;
        }
    }

    private void UpdateDisplay()
    {
        if (EconomyManager.Instance == null) return;

        if (woodText)
            woodText.text = EconomyManager.Instance.Wood.ToString();

        if (stoneText)
            stoneText.text = EconomyManager.Instance.Stone.ToString();

        if (goldText)
            goldText.text = EconomyManager.Instance.Gold.ToString();
    }
}
