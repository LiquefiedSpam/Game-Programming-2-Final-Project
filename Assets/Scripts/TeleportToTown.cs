using System;
using System.Collections;
using UnityEngine;

public class TeleportToTown : MonoBehaviour
{
    [SerializeField] private string teleportLocation;

    public static Action<int> OnTownChanged;

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

        Vector3 target = GameObject.Find(teleportLocation).transform.position;
        if (teleportLocation == "Sand Teleport")
        {
            OnTownChanged?.Invoke(2);
        }
        else if (teleportLocation == "Stone Teleport")
        {
            OnTownChanged?.Invoke(3);
        }
        else
        {
            OnTownChanged?.Invoke(1);
        }

        player.transform.position = new Vector3(target.x, player.transform.position.y, target.z);
        
        //unfade to black
        yield return StartCoroutine(UIManager.Ins.FadeIn());

        //turn on player controls
        player.SetMovementEnabled(true);
    }
}
