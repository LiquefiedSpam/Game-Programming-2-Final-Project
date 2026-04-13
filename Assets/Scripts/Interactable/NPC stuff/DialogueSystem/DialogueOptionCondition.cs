using UnityEngine;

//Inherit this class to make a new condition for when a dialogue option can appear.
public abstract class DialogueOptionCondition : ScriptableObject
{
    public abstract bool Evaluate(NpcBehavior npc);
}