using System;
using System.Threading.Tasks;
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
        _ = LoadSceneAsync(mainMenuSceneIdx);
    }

    public async void LoadGameplay()
    {
        await LoadSceneAsync(gameplaySceneIdx);
        Data.Init();
    }

    async Task LoadSceneAsync(int sceneIdx)
    {
        await SceneManager.LoadSceneAsync(sceneIdx);
    }
}