using UnityEngine;

public class Tile : MonoBehaviour
{
    public Transform endPoint;

    public float GetLength()
    {
        return endPoint.localPosition.x;
    }
}
