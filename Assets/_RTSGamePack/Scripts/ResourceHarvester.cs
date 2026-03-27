using UnityEngine;

[RequireComponent(typeof(Unit))]
public class ResourceHarvester : MonoBehaviour
{
    private enum HarvestState
    {
        Idle, MovingToResource, Harvesting, MovingToDropOff, DroppingOff
    }

    [Header("Settings")]
    [SerializeField] private float harvestDistance = 2f;
    [SerializeField] private float dropOffDistance = 3f;

    [Header("Carry Models")]
    [SerializeField] private GameObject woodCarryModel;
    [SerializeField] private GameObject bagCarryModel;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip woodHarvestClip;
    [SerializeField] private AudioClip stoneHarvestClip;
    [SerializeField] private AudioClip goldHarvestClip;
    [SerializeField] private AudioClip depositClip;

    private Unit unit;
    private UnitAnimator unitAnimator;

    private HarvestState currentState = HarvestState.Idle;
    private ResourceNode targetNode;
    private Transform dropOffPoint;
    private float harvestTimer;

    private EconomyManager.ResourceType carryingType;
    private int carryingAmount;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        unitAnimator = GetComponent<UnitAnimator>();
    }

    private void Update()
    {
        
    }

    public void AssignToResource(ResourceNode node, Transform dropOff)
    {
       
    }

    private void UpdateMovingToResource()
    {
        
    }

    private void UpdateHarvesting()
    {
       
    }

    private void UpdateMovingToDropOff()
    {
        
    }

    private void UpdateDroppingOff()
    {
        
    }
    public void StopHarvesting()
    {
        
    }
    private void UpdateCarryModels(bool carrying)
    {
       
    }

    private void PlayHarvestSound()
    {
        
    }

    private void StopHarvestSound()
    {
    
    }
}
