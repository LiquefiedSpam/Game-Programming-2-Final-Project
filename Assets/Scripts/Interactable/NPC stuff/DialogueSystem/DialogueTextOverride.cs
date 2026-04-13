using UnityEngine;

[System.Serializable]
public class DialogueTextOverride
{
    public DialogueOptionDefinition definition;

    [TextArea(2, 5)]
    public string responseOverride;
}