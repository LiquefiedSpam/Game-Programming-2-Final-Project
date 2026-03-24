using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Teleport : MonoBehaviour
{
    [SerializeField] private GameObject teleportLocation;
    [SerializeField] private Image mapImage;
    [SerializeField] private Sprite woodMap;
    [SerializeField] private Sprite sandMap;
    [SerializeField] private Sprite stoneMap;

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

        Vector3 target = teleportLocation.transform.position;
        if (teleportLocation.name == "Sand Teleport")
        {
            mapImage.sprite = sandMap;
        }
        else if (teleportLocation.name == "Stone Teleport")
        {
            mapImage.sprite = stoneMap;
        }
        else
        {
            mapImage.sprite = woodMap;
        }
        
        player.transform.position = new Vector3(target.x, player.transform.position.y, target.z);

        //unfade to black
        yield return StartCoroutine(UIManager.Ins.FadeIn());

        //turn on player controls
        player.SetMovementEnabled(true);
    }
}
