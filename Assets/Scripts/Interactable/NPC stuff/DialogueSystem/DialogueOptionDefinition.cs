using UnityEngine;

//defines what a dialogue option is
[CreateAssetMenu(menuName = "Dialogue/Option Definition")]
public class DialogueOptionDefinition : ScriptableObject
{
    public DialogueLabel label;
}