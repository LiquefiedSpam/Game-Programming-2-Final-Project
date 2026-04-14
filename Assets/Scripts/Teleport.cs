using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Teleport : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject[] grassTilePrefabs;
    public GameObject[] sandTilePrefabs;
    public GameObject[] stoneTilePrefabs;

    [SerializeField] private GameObject teleportLocation;

    [SerializeField] private Image mapImage;

    private enum TravelPath
    {
        WoodToStone,
        WoodToSand,
        SandToStone
    }

    public static Action<int> OnTownChanged;

    public float speed = 2f;

    private TravelPath currentPath;

    private int woodToStoneDangerLevel;
    private int woodToSandDangerLevel;
    private int sandToStoneDangerLevel;

    private float woodToStoneChance;
    private float woodToSandChance;
    private float sandToStoneChance;

    private string travelStatus;

    private List<GameObject> interactionZones;

    private void Start()
    {
        //ReRollValues();
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
        //if (teleportLocation.name == "Sand Teleport")
        //{
        //    UIManager.Ins.UpdateMapUISand(woodToSandDangerLevel, woodToSandChance, sandToStoneDangerLevel, sandToStoneChance);
        //    OnTownChanged?.Invoke(2);
        //}
        //else if (teleportLocation.name == "Stone Teleport")
        //{
        //    UIManager.Ins.UpdateMapUIStone(woodToStoneDangerLevel, woodToStoneChance, sandToStoneDangerLevel, sandToStoneChance);
        //    OnTownChanged?.Invoke(3);
        //}
        //else
        //{
        //    UIManager.Ins.UpdateMapUIWood(woodToSandDangerLevel, woodToSandChance, woodToStoneDangerLevel, woodToStoneChance);
        //    OnTownChanged?.Invoke(1);
        //}

        player.transform.position = new Vector3(target.x, player.transform.position.y, target.z);
        if (teleportLocation.name == "TravelPath")
        {
            ConstructTravelPath(player);
        }
        else
        {
            //TravelSafetyCheck();
            //ReRollValues();

            //unfade to black
            yield return StartCoroutine(UIManager.Ins.FadeIn());

            //state whether we were attacked or if we are ok

            //turn on player controls
            player.SetMovementEnabled(true);

            ////show travel status via UI temporary UI component
            //UIManager.Ins.ShowTravelStatus(travelStatus);
        }
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

        if (percentForAttack <= woodToStoneChance)
        {
            MarauderAttack(woodToStoneDangerLevel);
        }
        else
        {
            travelStatus = "No Mauraders attacked you!";
        }

        if (percentForAttack <= woodToSandChance)
        {
            MarauderAttack(woodToSandDangerLevel);
        }
        else
        {
            travelStatus = "No Mauraders attacked you!";
        }

        if (percentForAttack <= sandToStoneChance)
        {
            MarauderAttack(sandToStoneDangerLevel);
        }
        else
        {
            travelStatus = "No Mauraders attacked you!";
        }
    }

    private void ConstructTravelPath(PlayerController player)
    {
        switch (currentPath)
        {
            case TravelPath.WoodToStone:
                interactionZones = TileManager.Instance.GenerateTravelPath(grassTilePrefabs, stoneTilePrefabs);
                SetInteractionZones();
                break;

            case TravelPath.WoodToSand:
                interactionZones = TileManager.Instance.GenerateTravelPath(grassTilePrefabs, sandTilePrefabs);
                SetInteractionZones();
                break;

            case TravelPath.SandToStone:
                interactionZones = TileManager.Instance.GenerateTravelPath(sandTilePrefabs, stoneTilePrefabs);
                SetInteractionZones();
                break;
        }

        StartCoroutine(TraverseBetweenTowns(player));
        
    }

    private void SetInteractionZones()
    {
        for (int i = 0; i < interactionZones.Count; i++)
        {
            //check to see if it is a path selector or a marauder attack event
            if (interactionZones[i].GetComponentInChildren<PathingManager>())
            {
                interactionZones[i].GetComponentInChildren<PathingManager>().spawnSign();
            }
            else if (interactionZones[i].GetComponentInChildren<MarauderCampManager>())
            {
                interactionZones[i].GetComponentInChildren<MarauderCampManager>().spawnHut();
            }
        }
    }

    private IEnumerator TraverseBetweenTowns(PlayerController player)
    {
        yield return StartCoroutine(UIManager.Ins.FadeIn());
        //make the player move from where they are to next Interaction Zone
        if (interactionZones != null) {
            for (int i = 0; i < interactionZones.Count; i++)
            {
                Vector3 navgiationLocation = new Vector3(
                    interactionZones[i].transform.position.x,
                    0, 
                    interactionZones[i].transform.position.z
                );
                
                player.transform.position = Vector3.MoveTowards(player.transform.position, navgiationLocation, speed * Time.deltaTime);

                //wait until we arrive at point
                
                if (interactionZones[i].GetComponentInChildren<PathingManager>())
                {
                    PathingManager pathManager = interactionZones[i].GetComponentInChildren<PathingManager>();
                    //display pathing options
                    //await for player to make decision on where to go
                    //add to start of path to navigate to
                    interactionZones.Insert(0, pathManager.getSelectedPath());
                }
                //if location is marauder camp
                else if (interactionZones[i].GetComponentInChildren<MarauderCampManager>())
                {
                    //review stats for area and calculate probablity/effect of being attacked

                }

            }
            //move them towards the teleport location to next town (change x and z)
            Vector3 teleportLocation = new Vector3(0f, 0f, 0f);
            player.transform.position = Vector3.MoveTowards(player.transform.position, teleportLocation, speed * Time.deltaTime);
        }
    }

    private void MarauderAttack(int dangerLevel)
    {
        //perform ability of depleting items from inventory slots
        PlayerInventory.Instance.LoseItems(dangerLevel);

        travelStatus = $"You were attacked by Marauders and lost {dangerLevel} item(s)";
    }
}
