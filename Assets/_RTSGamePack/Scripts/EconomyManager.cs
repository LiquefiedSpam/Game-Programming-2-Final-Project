using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{   
    public static EconomyManager Instance { get; private set; }

    public enum ResourceType
    {
        Wood, Stone, Gold
    }

    public event Action OnResourcesChanged;

    [Header("Starting Resources")]
    [SerializeField] private int wood = 100;
    [SerializeField] private int stone = 50;
    [SerializeField] private int gold = 10;

    public int Wood => wood;
    public int Stone => stone;
    public int Gold => gold;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddResource(ResourceType type, int amount)
    {
      
    }
}
