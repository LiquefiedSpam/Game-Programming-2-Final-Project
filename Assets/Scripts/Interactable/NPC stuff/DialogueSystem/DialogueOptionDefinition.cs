using UnityEngine;

//defines what a dialogue option is / does
[CreateAssetMenu(menuName = "Dialogue/Option Definition")]
public class DialogueOptionDefinition : ScriptableObject
{
    public DialogueLabel label;
    public NpcInteractionActivity activity;
    public int timeCost;
}