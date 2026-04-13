using UnityEngine;
using System.Collections.Generic;

public enum DialogueLabel
{
    Purchase,
    Drink,
    Leave
}

[System.Serializable]

public class DialogueOption
{
    [SerializeField] public DialogueLabel label;
    [SerializeField][TextArea(2, 5)] public string response;
    //[SerializeField][TextArea(2, 5)] public string nightResponse;
}

public static class DialogueLabelExtensions
{
    public static string ToDisplayString(this DialogueLabel label)
    {
        switch (label)
        {
            case DialogueLabel.Purchase: return "Purchase";
            case DialogueLabel.Drink: return "Tavern";
            case DialogueLabel.Leave: return "Leave";
            default: return label.ToString();
        }
    }
}