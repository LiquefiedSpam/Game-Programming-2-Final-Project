using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;
    public static SceneLoader Instance => instance;

    [SerializeField] int mainMenuSceneIdx = 0;
    [SerializeField] int gameplaySceneIdx = 1;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        LoadSceneAsync(mainMenuSceneIdx);
    }

    public void LoadGameplay()
    {
        LoadSceneAsync(gameplaySceneIdx);
    }

    async void LoadSceneAsync(int sceneIdx)
    {
        await SceneManager.LoadSceneAsync(sceneIdx);
    }
}