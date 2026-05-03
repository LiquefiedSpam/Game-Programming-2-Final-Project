using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] DisplayController _menuController;

    [Header("Dialogue Audio")]
    [SerializeField] private AudioClip[] dialogueSounds = new AudioClip[4];

    [Header("Always Active")]
    [SerializeField] Slider _hungerSlider;
    [SerializeField] TextMeshProUGUI _moneyText;

    [Header("Sign")]
    [SerializeField] GameObject _signParent;
    [SerializeField] TextMeshProUGUI _signText;

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

    [Header("Tavern")]
    [SerializeField] private GameObject tavernUI;
    [SerializeField] private GameObject tavernBeerUI;
    [SerializeField] private TextMeshProUGUI tavernBeerQueryText;
    [SerializeField] private Button smallBeerButton;
    [SerializeField] private Button mediumBeerButton;
    [SerializeField] private Button largeBeerButton;
    [SerializeField] private TextMeshProUGUI smallBeerPriceText;
    [SerializeField] private TextMeshProUGUI mediumBeerPriceText;
    [SerializeField] private TextMeshProUGUI largeBeerPriceText;
    [SerializeField] private Button tavernQuitButton;

    [SerializeField] private GameObject tavernDestUI;
    [SerializeField] private Button townDestButton1;
    [SerializeField] private Button townDestButton2;
    [SerializeField] private TextMeshProUGUI townDestButton1Text;
    [SerializeField] private TextMeshProUGUI townDestButton2Text;

    [SerializeField] private TextMeshProUGUI tavernDestQueryText;


    [Header("Misc")]
    [SerializeField] private TextMeshProUGUI errorMsgPrefab;

    private AudioSource audioSource;
    private Coroutine statusFadeOutRoutine;

    public bool Visible => _canvas.gameObject.activeInHierarchy;
    public bool Transitioning { get; private set; } = false;

    public bool DisplayBlocksOthers =>
        _signParent.activeInHierarchy
        || DialogueUIManager.Ins.IsVisible
        || Transitioning;

    public static UIManager Ins => _instance;
    private static UIManager _instance;

    public Action OnDisplayBlocksOthers;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of UIManager in scene, destroying component on {gameObject.name}");
            Destroy(this);
            return;
        }
        _instance = this;
    }

    void Start()
    {
        // _hungerSlider.maxValue = PlayerController.MAX_HUNGER;
        // _hungerSlider.value = PlayerController.MAX_HUNGER;

        if (tavernUI.activeSelf)
            tavernUI.SetActive(false);

        InventoryDisplayManager.Ins.OnStallUIShown += HandleStallUIShown;

        smallBeerButton.onClick.AddListener(() => StartCoroutine(GameManager.Ins.HandleBeerSelected(Data.BeersBySize[BeerSize.SMALL])));
        mediumBeerButton.onClick.AddListener(() => StartCoroutine(GameManager.Ins.HandleBeerSelected(Data.BeersBySize[BeerSize.MEDIUM])));
        largeBeerButton.onClick.AddListener(() => StartCoroutine(GameManager.Ins.HandleBeerSelected(Data.BeersBySize[BeerSize.LARGE])));
        tavernQuitButton.onClick.AddListener(() => StartCoroutine(GameManager.Ins.HandleTavernQuitButton()));

        audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        InventoryDisplayManager.Ins.OnStallUIShown -= HandleStallUIShown;
    }

    public void Show(bool show)
    {
        _canvas.gameObject.SetActive(show);
    }

    public void UpdateHungerUI(float hunger)
    {
        _hungerSlider.value = hunger;
    }

    public void UpdateMoneyUI(float money)
    {
        _moneyText.text = $"$ {money}";
    }

    public void ShowSign(bool show, string message = "")
    {
        if (show)
        {
            OnDisplayBlocksOthers?.Invoke();
            _signText.SetText(message);
        }
        _signParent.SetActive(show);
    }

    public void ShowTravelStatus(string message)
    {
        travelStatusText.text = message;
        travelStatusUI.SetActive(true);
        statusFadeOutRoutine = StartCoroutine(StatusFadeOut());
    }

    public void DisplayError(string errorMsg)
    {
        var txt = Instantiate(errorMsgPrefab, _canvas.transform);
        txt.text = errorMsg;
        txt.rectTransform.position = Input.mousePosition;
        StartCoroutine(UIAnimations.FloatUpAndFade(txt));
    }

    public void playAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    // Tavern UI
    public void EnableTavernUI(string npcName)
    {
        smallBeerButton.interactable = true;
        mediumBeerButton.interactable = true;
        largeBeerButton.interactable = true;
        tavernBeerQueryText.text = $"What'll you get {npcName}?";
        tavernUI.SetActive(true);
        tavernBeerUI.SetActive(true);
    }

    public void DisableTavernUI()
    {
        tavernDestUI.SetActive(false);
        tavernUI.SetActive(false);
    }

    public void EnableTavernDestUI(Town dest1, Town dest2)
    {
        tavernDestUI.SetActive(true);
        townDestButton1.interactable = true;
        townDestButton2.interactable = true;
        townDestButton1Text.text = dest1.TownToString();
        townDestButton2Text.text = dest2.TownToString();

        townDestButton1.onClick.RemoveAllListeners();
        townDestButton2.onClick.RemoveAllListeners();
        townDestButton1.onClick.AddListener(() => StartCoroutine(GameManager.Ins.HandleDestSelected(dest1)));
        townDestButton2.onClick.AddListener(() => StartCoroutine(GameManager.Ins.HandleDestSelected(dest2)));
    }

    public void HideBeerUI()
    {
        tavernBeerUI.SetActive(false);
    }

    public void DisableBeerButtons()
    {
        smallBeerButton.interactable = false;
        mediumBeerButton.interactable = false;
        largeBeerButton.interactable = false;
    }

    public void DisableDestButtons()
    {
        townDestButton1.interactable = false;
        townDestButton2.interactable = false;
        DisableTavernUI();
    }

    public IEnumerator WaitForBeerButtonAnim()
    {
        bool done = false;
        void onDone() => done = true;
        smallBeerButton.GetComponent<ButtonAnimator>().OnClickAnimFinished += onDone;
        mediumBeerButton.GetComponent<ButtonAnimator>().OnClickAnimFinished += onDone;
        largeBeerButton.GetComponent<ButtonAnimator>().OnClickAnimFinished += onDone;
        while (!done) yield return null;
        smallBeerButton.GetComponent<ButtonAnimator>().OnClickAnimFinished -= onDone;
        mediumBeerButton.GetComponent<ButtonAnimator>().OnClickAnimFinished -= onDone;
        largeBeerButton.GetComponent<ButtonAnimator>().OnClickAnimFinished -= onDone;
    }

    public IEnumerator WaitForDestButtonAnim()
    {
        bool done = false;
        void onDone() => done = true;
        townDestButton1.GetComponent<ButtonAnimator>().OnClickAnimFinished += onDone;
        townDestButton2.GetComponent<ButtonAnimator>().OnClickAnimFinished += onDone;
        while (!done) yield return null;
        townDestButton1.GetComponent<ButtonAnimator>().OnClickAnimFinished -= onDone;
        townDestButton2.GetComponent<ButtonAnimator>().OnClickAnimFinished -= onDone;
    }

    // Transitions
    public async Task FadeInOut(float inTime = 0.5f, float holdTime = 0.5f, float outTime = 0.5f)
    {
        Transitioning = true;
        OnDisplayBlocksOthers?.Invoke();

        float elapsed = 0f;
        while (elapsed < inTime)
        {
            await Task.Yield();
            elapsed += Time.deltaTime;
            transitionImage.SetAlpha(elapsed / inTime);
        }

        await Task.Delay(Mathf.CeilToInt(holdTime * 1000));

        elapsed = 0f;
        while (elapsed < outTime)
        {
            await Task.Yield();
            elapsed += Time.deltaTime;
            transitionImage.SetAlpha(1 - (elapsed / inTime));
        }

        Transitioning = false;
    }

    public async Task FadeAlpha(float duration, float targetAlpha)
    {
        Transitioning = true;
        OnDisplayBlocksOthers?.Invoke();

        float startAlpha = transitionImage.color.a;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            await Task.Yield();
            elapsed += Time.deltaTime;
            transitionImage.SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration));
        }
        transitionImage.SetAlpha(targetAlpha);

        Transitioning = false;
    }

    public IEnumerator FadeOut() => Fade(0f, 1f);
    public IEnumerator FadeIn() => Fade(1f, 0f);

    private IEnumerator Fade(float start, float end)
    {
        Transitioning = true;
        OnDisplayBlocksOthers?.Invoke();

        float time = 0f;
        Color color = transitionImage.color;
        while (time < transitionTime)
        {
            color.a = Mathf.Lerp(start, end, time / transitionTime);
            transitionImage.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = end;
        transitionImage.color = color;
        Transitioning = false;
    }

    private IEnumerator StatusFadeOut()
    {
        float time = 0f;
        Image panel = travelStatusUI.GetComponent<Image>();
        Color color = panel.color;
        color.a = 1f;

        while (time < statusFadeOutTime)
        {
            color.a = Mathf.Lerp(1f, 0f, time / statusFadeOutTime);
            panel.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = 0f;
        panel.color = color;
        travelStatusUI.SetActive(false);
    }

    private void HandleStallUIShown()
    {
        if (statusFadeOutRoutine != null)
        {
            StopCoroutine(statusFadeOutRoutine);
            Image panel = travelStatusUI.GetComponent<Image>();
            Color color = panel.color;
            color.a = 0f;
            panel.color = color;
            travelStatusUI.SetActive(false);
        }
    }

    public void UpdatePathingDisplay()
    {
        //check to see if panel is active (if so, disable it; if not, enable it)
    }
}