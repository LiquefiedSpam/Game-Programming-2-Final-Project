using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimePlayedSaver : MonoBehaviour
{
    public static TimePlayedSaver Instance { get; private set; }

    [SerializeField] int gameplaySceneIdx;

    [Header("Save Settings")]
    [SerializeField] private string saveFolder = "Saves";
    [SerializeField] private string fileExtension = ".json";
    [SerializeField] string timePlayedFile = "TimePlayed";

    public float TimePlayed { get; private set; }
    bool trackTime = false;

    string SaveFolderPath => Path.Combine(Application.persistentDataPath, saveFolder);
    string SaveFilePath => Path.Combine(SaveFolderPath, timePlayedFile + fileExtension);

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (!Directory.Exists(SaveFolderPath))
        {
            Directory.CreateDirectory(SaveFolderPath);
        }
        Load();
    }

    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void Update()
    {
        if (trackTime) TimePlayed += Time.deltaTime;
    }

    void Load()
    {
        if (!File.Exists(SaveFilePath))
        {
            return; // time played is 0 by default
        }
        TimePlayed = JsonUtility.FromJson<SaveTimePlayed>(File.ReadAllText(SaveFilePath)).Time;
    }

    public void Save()
    {
        SaveTimePlayed saveTimePlayed = new() { Time = TimePlayed };
        File.WriteAllText(SaveFilePath, JsonUtility.ToJson(saveTimePlayed));
    }

    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        trackTime = newScene.buildIndex == gameplaySceneIdx;
    }

    void OnDestroy()
    {
        Debug.Log($"Saving time played ({TimePlayed}) to file: {SaveFilePath}");
        Save();
    }

    [Serializable]
    class SaveTimePlayed
    {
        public float Time;
    }
}