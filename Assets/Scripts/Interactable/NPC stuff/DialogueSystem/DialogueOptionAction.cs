using UnityEngine;

public abstract class DialogueOptionAction : ScriptableObject
{
    public abstract void Execute(NpcBehavior npc);
}
