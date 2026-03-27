using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private EconomyManager.ResourceType resourceType;
    [SerializeField] private int totalAmount = 300;

    [Header("Depletion Settings")]
    [SerializeField] private GameObject[] visualStages;
    [SerializeField] private bool destroyWhenDepleted = true;
    [SerializeField] private float destroyDelay = 3f;
    [SerializeField] private GameObject depletedRemainsPrefab;
    [SerializeField] private Transform harvestPoint;
    private int remainingAmount;
    private int currentStageIndex;
    private float resourcePerStage;

    public EconomyManager.ResourceType Type => resourceType;
    public bool IsDepleted => remainingAmount <= 0;

    public Vector3 HarvestPosition => harvestPoint != null ? harvestPoint.position : transform.position;

    private void Start()
    {
        remainingAmount = totalAmount;
        currentStageIndex = 0;

        // Calculate how much resource must be harvested before the visual changes
    }

    public int Harvest(int requestedAmount)
    {

        // Calculate how much we can actually give

        // Update the visual stage based on how much has been harvested

        // Check if the node is now fully depleted

        return 0;
    }
    private void UpdateVisualStage()
    {

    }

    private void OnDepleted()
    {

    }
}
