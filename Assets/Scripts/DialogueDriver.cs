using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDriver : MonoBehaviour
{
    public static DialogueDriver Ins => _instance;
    private static DialogueDriver _instance;

    private Action _onConfirm;
    public bool HasPendingConfirm => _onConfirm != null;

    private NpcBehavior _currentNpc;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(this); return; }
        _instance = this;
    }

    public void StartDialogue(NpcBehavior npc, string openingLine, List<DialogueOptionInstance> options, Sprite portrait)
    {
        Debug.Log($"DialogueDriver.StartDialogue called, npc={npc.NpcName}, line={openingLine}");

        _currentNpc = npc;
        DialogueUIManager.Ins.ShowDialogue(npc.NpcName, openingLine, portrait, options);
    }

    public void ShowResponse(string response, Sprite portrait, bool showContinue)
    {
        DialogueUIManager.Ins.ShowDialogue(_currentNpc.NpcName, response, portrait, null, showContinue);
    }

    public void HandleOptionSelected(DialogueOptionInstance option)
    {
        _currentNpc?.HandleOptionSelected(option);
    }

    public void EndDialogue()
    {
        _currentNpc = null;
        DialogueUIManager.Ins.CloseDialogue();
    }

    public void WaitForConfirm(Action onConfirm)
    {
        _onConfirm = onConfirm;
    }

    public void Confirm()
    {
        _onConfirm?.Invoke();
        _onConfirm = null;
    }
}