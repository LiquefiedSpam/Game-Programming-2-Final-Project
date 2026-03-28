using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] MenuController _menuController;

    [Header("Always Active")]
    [SerializeField] Slider _hungerSlider;

    [Header("Sign")]
    [SerializeField] GameObject _signParent;
    [SerializeField] TextMeshProUGUI _signText;

    [Header("Dialog")]
    [SerializeField] GameObject _dialogParent;
    [SerializeField] TextMeshProUGUI _dialogText;
    [SerializeField] TextMeshProUGUI _dialogNameText;
    [SerializeField] Image _dialogPortrait;

    [Header("Inventory")]
    [SerializeField] public Inventory _merchantInventory;
    [SerializeField] public Inventory _playerInventory;
    [SerializeField] public Inventory _playerStall;

    [Header("Transition")]
    [SerializeField] Image transitionImage;
    [SerializeField] float transitionTime;

    [Header("MapUI")]
    [SerializeField] private Image mapImage;

    [SerializeField] private Sprite woodMap;
    [SerializeField] private Sprite sandMap;
    [SerializeField] private Sprite stoneMap;

    [SerializeField] private TextMeshProUGUI woodToStoneUI;
    [SerializeField] private TextMeshProUGUI woodToSandUI;
    [SerializeField] private TextMeshProUGUI sandToStoneUI;

    [Header("Travel Status")]
    [SerializeField] private GameObject travelStatusUI;
    [SerializeField] private TextMeshProUGUI travelStatusText;
    [SerializeField] float statusFadeOutTime = 5f;

    public bool Visible => _canvas.gameObject.activeInHierarchy;

    public static UIManager Ins => _instance;
    private static UIManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of UIManager in scene, destroying component on {gameObject.name}");
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        _hungerSlider.maxValue = PlayerController.MAX_HUNGER;
        _hungerSlider.value = PlayerController.MAX_HUNGER;
        UpdateMapUIWood(Random.Range(1, 5), Random.Range(0, 1f), Random.Range(1, 5), Random.Range(0, 1f));
    }

    public void Show(bool show)
    {
        _canvas.gameObject.SetActive(show);
    }

    public void UpdateHungerUI(float hunger)
    {
        _hungerSlider.value = hunger;
    }

    public void ShowSign(bool show, string message = "")
    {
        if (show) _signText.SetText(message);
        _signParent.SetActive(show);
    }

    public void ShowDialog(bool show, string npcName = "Name", string dialog = "Dialog", Sprite portrait = null)
    {
        if (show)
        {
            _dialogText.SetText(dialog);
            _dialogNameText.SetText(npcName);
            _dialogPortrait.sprite = portrait;
        }
        _dialogParent.SetActive(show);
    }

    public void UpdateMapUIWood(float woodToSandDangerLevel, float woodToSandChance, float woodToStoneDangerLevel, float woodToStoneChance)
    {
        mapImage.sprite = woodMap;
        woodToSandUI.text = $"Danger Level: {woodToSandDangerLevel}\n Chance: {(woodToSandChance * 100f):F2}%";
        woodToStoneUI.text = $"Danger Level: {woodToStoneDangerLevel}\n Chance: {(woodToStoneChance * 100f):F2}%";
        woodToSandUI.enabled = true;
        woodToStoneUI.enabled = true;
        sandToStoneUI.enabled = false;
    }

    public void UpdateMapUIStone(float woodToStoneDangerLevel, float woodToStoneChance, float sandToStoneDangerLevel, float sandToStoneChance)
    {
        mapImage.sprite = stoneMap;
        woodToStoneUI.text = $"Danger Level: {woodToStoneDangerLevel}\n Chance: {(woodToStoneChance * 100f):F2}%";
        sandToStoneUI.text = $"Danger Level: {sandToStoneDangerLevel}\n Chance: {(sandToStoneChance * 100f):F2}%";
        sandToStoneUI.enabled = true;
        woodToStoneUI.enabled = true;
        woodToSandUI.enabled = false;
    }

    public void UpdateMapUISand(float woodToSandDangerLevel, float woodToSandChance, float sandToStoneDangerLevel, float sandToStoneChance)
    {
        mapImage.sprite = sandMap;
        woodToSandUI.text = $"Danger Level: {woodToSandDangerLevel}\n Chance: {(woodToSandChance * 100f):F2}%";
        sandToStoneUI.text = $"Danger Level: {sandToStoneDangerLevel}\n Chance: {(sandToStoneChance * 100f):F2}%";
        woodToSandUI.enabled = true;
        sandToStoneUI.enabled = true;
        woodToStoneUI.enabled = false;
    }

    public void ShowTravelStatus(string message)
    {
        travelStatusText.text = message;

        travelStatusUI.SetActive(true);

        StartCoroutine(StatusFadeOut());
    }

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(0f, 1f));
    }
    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float start, float end)
    {
        float time = 0f;
        Color color = transitionImage.color;

        while (time < transitionTime)
        {
            float t = time / transitionTime;
            color.a = Mathf.Lerp(start, end, t);
            transitionImage.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = end;
        transitionImage.color = color;
    }

    private IEnumerator StatusFadeOut()
    {
        float time = 0f;

        Color panelColor = travelStatusUI.GetComponent<Image>().color;
        panelColor.a = 1f;

        while (time < statusFadeOutTime)
        {
            float t = time / statusFadeOutTime;

            panelColor.a = Mathf.Lerp(1f, 0f, t);
            travelStatusUI.GetComponent<Image>().color = panelColor;

            time += Time.deltaTime;
            yield return null;
        }

        panelColor.a = 0f;
        travelStatusUI.GetComponent<Image>().color = panelColor;
        travelStatusUI.gameObject.SetActive(false);
    }
}