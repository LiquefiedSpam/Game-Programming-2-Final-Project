using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 offset;
    Vector3 newPos;
    public GameObject player;
    public GameObject mockPlayer;

    Transform target;

    void Awake()
    {
        target = player.transform;
    }

    void Start()
    {
        offset = player.transform.position - transform.position;
    }

    void Update()
    {
        transform.position = target.position - offset;
    }

    public void FollowPlayer()
    {
        target = player.transform;
    }

    public void FollowMockPlayer()
    {
        target = mockPlayer.transform;
    }
}
