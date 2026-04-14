using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

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

    private List<GameObject> interactionZones;
    private List<GameObject> travelPath;

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

            //unfade to black
            yield return StartCoroutine(UIManager.Ins.FadeIn());

            //turn on player controls
            player.SetMovementEnabled(true);
        }
    }

    private void ConstructTravelPath(PlayerController player)
    {

        //move camera closer to player so they can't see interaction
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
                travelPath.Add(interactionZones[i]);
                interactionZones[i].GetComponentInChildren<PathingManager>().spawnSign();
            }
            else if (interactionZones[i].GetComponentInChildren<MarauderCampManager>())
            {
                interactionZones[i].GetComponentInChildren<MarauderCampManager>().spawnHut();
            }
            else
            {
                travelPath.Add(interactionZones[i]);
            }
        }
    }

    private IEnumerator TraverseBetweenTowns(PlayerController player)
    {
        yield return StartCoroutine(UIManager.Ins.FadeIn());
        //make the player move from where they are to next Interaction Zone
        if (travelPath != null) {
            for (int i = 0; i < travelPath.Count; i++)
            {
                Vector3 navgiationLocation = new Vector3(
                    travelPath[i].transform.position.x,
                    player.transform.position.y, 
                    travelPath[i].transform.position.z
                );

                //wait until we arrive at point
                while (Vector3.Distance(player.transform.position, navgiationLocation) > 0.05f) {
                    player.transform.position = Vector3.MoveTowards(
                        player.transform.position, 
                        navgiationLocation, 
                        speed * Time.deltaTime
                    );

                    yield return null;
                }
                
                if (travelPath[i].GetComponentInChildren<PathingManager>())
                {
                    PathingManager pathManager = travelPath[i].GetComponentInChildren<PathingManager>();

                    pathManager.ResetSelection();

                    //display pathing options
                    UIManager.Ins.UpdatePathingDisplay();

                    //await for player to make decision on where to go
                    yield return new WaitUntil(() => pathManager.HasSelectedPath);

                    UIManager.Ins.UpdatePathingDisplay();

                    //add to start of path to navigate to
                    travelPath.Insert(i + 1, pathManager.getSelectedPath());
                }
                //if location is marauder camp
                else if (travelPath[i].GetComponentInChildren<MarauderCampManager>())
                {
                    MarauderCampManager marauderCamp = interactionZones[i].GetComponentInChildren<MarauderCampManager>();
                    //check to see if marauder camp is there (yes, lose items and change map icon, no change map icon and continue)
                    //display status of encountered area
                    UIManager.Ins.ShowTravelStatus(marauderCamp.EncounterDetails());
                }
            }
        }
    }
}
