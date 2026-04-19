using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    [SerializeField] GameObject maraudersPrefab;
    [SerializeField] Transform prefabParent;
    [SerializeField] Transform playerDest;
    public Vector3 PlayerDestination => playerDest.position;

    public void SpawnPrefab(InteractionResult interactionResult)
    {
        if (interactionResult == InteractionResult.MARAUDERS)
        {
            GameObject obj = Instantiate(maraudersPrefab, prefabParent);
            obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}