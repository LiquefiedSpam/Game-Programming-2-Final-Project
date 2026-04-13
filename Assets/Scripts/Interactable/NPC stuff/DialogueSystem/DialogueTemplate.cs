using UnityEngine;
using System.Collections.Generic;

//dialogue template of what options are allowed per type of NPC
[CreateAssetMenu(menuName = "Dialogue/NPC Template")]
public class DialogueTemplate : ScriptableObject
{
    public List<DialogueOptionDefinition> options;
}