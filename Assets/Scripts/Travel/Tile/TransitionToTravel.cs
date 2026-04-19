using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TransitionToTravel : MonoBehaviour
{
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
        await pathManager.BuildPath(fromTown, toTown);
        pathManager.StartPlayerOnPath();
    }
}