using UnityEngine;

[System.Serializable]
public class DialogueOptionInstance
{
    public DialogueOptionDefinition definition;

    [TextArea(2, 5)]
    public string response;

    // [TextArea(2, 5)]
    // public string nightResponse;
    public DialogueOptionAction action;
}