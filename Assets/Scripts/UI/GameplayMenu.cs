using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameplayMenu : MonoBehaviour
{
    [SerializeField] InputActionReference menuAction;
    [SerializeField] GameObject root;
    [SerializeField] Button resumeButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button exitButton;
    [SerializeField] Slider volumeSlider;

    public bool Visible => root.activeSelf;

    void Start()
    {
        menuAction.action.performed += ChangeMenuVisibility;
        root.SetActive(false);
    }

    void ChangeMenuVisibility(InputAction.CallbackContext context)
    {
        if (root == null) Debug.Log("root is null");

        root.SetActive(true);
        if (Visible)
        {
            ActivateButtons();
        }
        else
        {
            DeactivateButtons();
        }
    }

    void MainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    void ActivateButtons()
    {
        exitButton.onClick.AddListener(() => root.SetActive(false));
        resumeButton.onClick.AddListener(() => root.SetActive(false));
        mainMenuButton.onClick.AddListener(MainMenu);
        volumeSlider.value = AudioManager.Instance.Volume;
        volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
    }

    void DeactivateButtons()
    {
        exitButton.onClick.RemoveAllListeners();
        resumeButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        volumeSlider.onValueChanged.RemoveAllListeners();
    }

    void OnDestroy()
    {
        DeactivateButtons();
        menuAction.action.performed -= ChangeMenuVisibility;
    }
}
