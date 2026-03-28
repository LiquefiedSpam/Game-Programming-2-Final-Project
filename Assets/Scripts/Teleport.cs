using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class Teleport : MonoBehaviour
{
    [SerializeField] private string currentTown;
    [SerializeField] private GameObject teleportLocation;

    [SerializeField] private Image mapImage;

    private enum TravelPath
    {
        WoodToStone,
        WoodToSand,
        SandToStone
    }

    public static Action<int> OnTownChanged;

    private TravelPath currentPath;

    private int woodToStoneDangerLevel;
    private int woodToSandDangerLevel;
    private int sandToStoneDangerLevel;

    private float woodToStoneChance;
    private float woodToSandChance;
    private float sandToStoneChance;

    private string travelStatus;

    private void Start()
    {
        ReRollValues();
    }

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
            UIManager.Ins.UpdateMapUISand(woodToSandDangerLevel, woodToSandChance, sandToStoneDangerLevel, sandToStoneChance);
            OnTownChanged?.Invoke(2);
        }
        else if (teleportLocation.name == "Stone Teleport")
        {
            UIManager.Ins.UpdateMapUIStone(woodToStoneDangerLevel, woodToStoneChance, sandToStoneDangerLevel, sandToStoneChance);
            OnTownChanged?.Invoke(3);
        }
        else
        {
            UIManager.Ins.UpdateMapUIWood(woodToSandDangerLevel, woodToSandChance, woodToStoneDangerLevel, woodToStoneChance);
            OnTownChanged?.Invoke(1);
        }

        player.transform.position = new Vector3(target.x, player.transform.position.y, target.z);
        TravelSafetyCheck();
        ReRollValues();

        //unfade to black
        yield return StartCoroutine(UIManager.Ins.FadeIn());

        //state whether we were attacked or if we are ok

        //turn on player controls
        player.SetMovementEnabled(true);

        //show travel status via UI temporary UI component
        UIManager.Ins.ShowTravelStatus(travelStatus);
    }

    private void ReRollValues()
    {
        woodToStoneDangerLevel = Random.Range(1, 5);
        woodToSandDangerLevel = Random.Range(1, 5);
        sandToStoneDangerLevel = Random.Range(1, 5);

        woodToStoneChance = Random.Range(0, 1f);
        woodToSandChance = Random.Range(0, 1f);
        sandToStoneChance = Random.Range(0, 1f);
    }

    private void TravelSafetyCheck()
    {
        float percentForAttack = Random.Range(0f, 1f);

        switch (currentPath)
        {
            case TravelPath.WoodToStone:
                if (percentForAttack <= woodToStoneChance)
                {
                    MarauderAttack(woodToStoneDangerLevel);
                }
                else
                {
                    travelStatus = "No Mauraders attacked you!";
                }
                break;

            case TravelPath.WoodToSand:
                if (percentForAttack <= woodToSandChance)
                {
                    MarauderAttack(woodToSandDangerLevel);
                }
                else
                {
                    travelStatus = "No Mauraders attacked you!";
                }
                break;

            case TravelPath.SandToStone:
                if (percentForAttack <= sandToStoneChance)
                {
                    MarauderAttack(sandToStoneDangerLevel);
                }
                else
                {
                    travelStatus = "No Mauraders attacked you!";
                }
                break;
        }
    }

    private void MarauderAttack(int dangerLevel)
    {
        //perform ability of depleting items from inventory slots
        PlayerInventory.Instance.LoseItems(dangerLevel);

        travelStatus = $"You were attacked by Marauders and lost {dangerLevel} item(s)";
    }
}
