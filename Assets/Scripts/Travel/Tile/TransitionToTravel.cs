using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TransitionToTravel : MonoBehaviour
{
    const float FADE_TIME = 0.3f;

    [SerializeField] Town fromTown;
    [SerializeField] Town toTown;
    [SerializeField] PathManager pathManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartPath();
        }
    }

    async void StartPath()
    {
        Data.Player.SetMovementDisable(true);
        await Task.WhenAll(UIManager.Ins.FadeAlpha(FADE_TIME, 1f), pathManager.BuildPath(fromTown, toTown));
        pathManager.StartPlayerOnPath(); // path manager handles fade out
    }
}