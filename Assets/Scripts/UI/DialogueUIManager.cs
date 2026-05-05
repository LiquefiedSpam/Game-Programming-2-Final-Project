using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Ins => _instance;
    private static DialogueUIManager _instance;

    [SerializeField] private GameObject _dialogParent;
    [SerializeField] private TMP_Text _dialogText;
    [SerializeField] private TMP_Text _dialogNameText;
    [SerializeField] private Image _dialogPortrait;
    [SerializeField] private Transform _optionsContainer;
    [SerializeField] private GameObject _optionButtonPrefab;
    [SerializeField] private GameObject _continuePrompt;

    [Header("Dialogue Audio")]
    [SerializeField] private AudioClip[] dialogueSounds;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(this); return; }
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void ShowDialogue(string npcName, string text, Sprite portrait,
                             List<DialogueOptionInstance> options = null, bool showContinue = false)
    {
        Debug.Log($"DialogueUIManager.ShowDialogue called, name={npcName}, text={text}");
        ClearOptions();
        _dialogParent.SetActive(true);
        _dialogNameText.SetText(npcName);
        _dialogText.SetText(text);
        _dialogPortrait.sprite = portrait;
        _continuePrompt.SetActive(showContinue);

        if (options != null)
            foreach (var opt in options)
                SpawnOptionButton(opt);

        PlayRandomDialogueSound();
    }

    public void CloseDialogue()
    {
        ClearOptions();
        _dialogParent.SetActive(false);
    }

    public bool IsVisible => _dialogParent.activeSelf;

    private void SpawnOptionButton(DialogueOptionInstance opt)
    {
        var go = Instantiate(_optionButtonPrefab, _optionsContainer);
        go.GetComponentInChildren<TMP_Text>().text = opt.definition.label.ToString();
        go.GetComponent<DialogueOptionButton>().Setup(opt);
    }

    private void ClearOptions()
    {
        foreach (Transform child in _optionsContainer)
            Destroy(child.gameObject);
    }

    private void PlayRandomDialogueSound()
    {
        if (dialogueSounds == null || dialogueSounds.Length == 0) return;
        var valid = System.Array.FindAll(dialogueSounds, c => c != null);
        if (valid.Length == 0) return;
        _audioSource.Stop();
        _audioSource.clip = valid[UnityEngine.Random.Range(0, valid.Length)];
        _audioSource.pitch = 3f;
        _audioSource.Play();
    }
}