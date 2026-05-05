using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "NpcInteractionActivity/TavernActivity")]
public class TavernActivity : NpcInteractionActivity
{
    public override IEnumerator Begin(NpcBehavior npc)
    {
        bool done = false;
        TavernManager.Ins.GoToTavernAndDrink(npc, () => done = true);
        while (!done) yield return null;
        npc.FlushTally();
    }
}
