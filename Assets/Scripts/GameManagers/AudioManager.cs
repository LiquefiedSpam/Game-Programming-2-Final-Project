using UnityEngine;

public class AudioManager : MonoBehaviour
{
    void Awake()
    {
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (var s in sources)
        {
            s.volume *= Data.Volume;
        }
    }
}