using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TransitionToTravel : MonoBehaviour
{
    const float FADE_TIME = 0.3f;

    [SerializeField] Town fromTown;
    [SerializeField] Town toTown;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartPath();
        }
    }

    async void StartPath()
    {
        DayManager.Ins.ConsumeUnit(1);
        Data.Player.SetMovementDisable(true);
        await Task.WhenAll(UIManager.Ins.FadeAlpha(FADE_TIME, 1f), PathManager.Ins.BuildPath(fromTown, toTown));
        PathManager.Ins.StartPlayerOnPath(); // path manager handles fade out
    }
}