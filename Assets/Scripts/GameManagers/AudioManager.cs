using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public float Volume => volume;
    float volume = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += (_, _) => SetVolume(volume);
    }

    public void SetVolume(float level)
    {
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (var s in sources)
        {
            float ogVolume = s.volume * (1f / volume);
            s.volume = ogVolume * level;
        }
        volume = level;
    }
}