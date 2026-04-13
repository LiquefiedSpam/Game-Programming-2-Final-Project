using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Actions/Go To Tavern")]
public class GoToTavernAction : DialogueOptionAction
{
    public override void Execute(NpcBehavior npc)
    {
        Debug.Log("Moving NPC + player to tavern");

        // your teleport / scene / cutscene logic here
    }
}