using UnityEngine;

public class PathingManager : MonoBehaviour
{
    [SerializeField] private GameObject signPrefab;
    
    [Header("Path Settings")]
    [SerializeField] private GameObject PathA;
    [SerializeField] private GameObject PathB;

    private GameObject selectedPath;

    public void spawnSign()
    {

    }
    public void selectPathA()
    {
        selectedPath = PathA;
    }

    public void selectPathB()
    {
        selectedPath = PathB;
    }

    public GameObject getSelectedPath()
    {
        return selectedPath;
    }
}
