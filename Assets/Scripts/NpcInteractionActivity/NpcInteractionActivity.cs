using System.Collections;
using UnityEngine;

//An activity the player can engage in with an NPC.
public abstract class NpcInteractionActivity : ScriptableObject
{
    public abstract IEnumerator Begin(NpcBehavior npc);
}