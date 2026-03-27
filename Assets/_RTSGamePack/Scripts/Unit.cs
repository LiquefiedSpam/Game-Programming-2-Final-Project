using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [Header("Unit Configuration")]
    [SerializeField] private UnitData unitData;

    [Header("Selection Visual")]
    [SerializeField] private GameObject selectionIndicator;
    private int currentHealth;
    private bool isSelected;
    private NavMeshAgent agent;

    // Public properties
    public UnitData Data => unitData;
    public bool IsSelected => isSelected;
    public int CurrentHealth => currentHealth;
    public bool IsAlive => currentHealth > 0;

    private void Awake()
    {
    }

    private void Start()
    {
    }

    public void Initialize(UnitData data)
    {
        // initialize properties from the scriptable object data

    }

    public void Select()
    {
      
    }

    public void Deselect()
    {
    }

    public void MoveTo(Vector3 destination)
    {

    }

    public void Stop()
    {

    }

    public void TakeDamage(int damage)
    {
        
    }
    void Die()
    {
        
    }
}
