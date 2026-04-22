using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] TMP_Text timePlayedText;
    [Header("Menu")]
    [SerializeField] GameObject menuParent;
    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [Header("Settings")]
    [SerializeField] GameObject settingsParent;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Button backButton;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(ShowSettings);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        backButton.onClick.AddListener(ShowMenu);

        timePlayedText.text = SecondsToString(TimePlayedSaver.Instance.TimePlayed);
    }

    void StartGame()
    {
        SceneLoader.Instance.LoadGameplay();
    }

    void ShowMenu()
    {
        settingsParent.SetActive(false);
        menuParent.SetActive(true);
    }

    void ShowSettings()
    {
        menuParent.SetActive(false);
        settingsParent.SetActive(true);
    }

    void SetVolume(float value)
    {
        Data.Volume = value;
    }

    void OnDestroy()
    {
        startButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        volumeSlider.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    string SecondsToString(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return "Total Time Played: " + $"{(int)timeSpan.TotalHours:00}h {timeSpan.Minutes:00}m {timeSpan.Seconds:00}s";
    }
}