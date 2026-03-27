using UnityEngine;

// this will become a ScriptableObject
public class UnitData : MonoBehaviour
{
    [Header("Basic Info")]
    public string unitName;

    [TextArea(2, 3)]
    public string description;

    public Sprite icon;
    public GameObject prefab;

    [Header("Training")]
    public int woodCost;
    public int stoneCost;
    public int goldCost;
    public float trainingTime = 5f;

    [Header("Combat Stats")]
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float attackSpeed = 1f;
    public float attackRange = 2f;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Harvesting")]
    public bool canHarvest = false;
    public int harvestAmount = 10;
    public float harvestTime = 3f;

    // helper
    public bool CanAfford(int currentWood, int currentStone, int currentGold)
    {
        return currentWood >= woodCost
            && currentStone >= stoneCost
            && currentGold >= goldCost;
    }
}
