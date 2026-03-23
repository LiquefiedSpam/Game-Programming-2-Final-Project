using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform teleportLocation;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            StartCoroutine(TeleportRoutine(player));
        }
    }

    private IEnumerator TeleportRoutine(PlayerController player)
    {

        //turn off player controls here
        player.SetMovementEnabled(false);

        //start fade to black sequence
        yield return StartCoroutine(UIManager.Ins.FadeOut());

        Vector3 target = teleportLocation.position;
        player.transform.position = new Vector3(target.x, player.transform.position.y, target.z);

        //unfade to black
        yield return StartCoroutine(UIManager.Ins.FadeIn());

        //turn on player controls
        player.SetMovementEnabled(true);
    }
}
