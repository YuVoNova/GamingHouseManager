using System.IO;
using System.Collections.Generic;
using UnityEngine;
//using GameAnalyticsSDK;
//using Facebook.Unity;

public class Manager : MonoBehaviour
{
    // Singleton

    private static Manager instance;
    public static Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("Manager").GetComponent<Manager>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }


    // Player

    [HideInInspector]
    public PlayerData PlayerData;


    // Game Data

    public List<Game> Games;
    public List<Team> Teams;

    public Dictionary<string, AudioClip> Audios;


    // Data Handling

    private string dataPath;


    // Instantiatable Objects
    



    // Unity Functions

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Save();
    }

    private void OnDestroy()
    {
        Save();
    }

    private void OnApplicationPause(bool pause)
    {
        /*
        if (!pause)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                FB.Init(() => { FB.ActivateApp(); });
            }
        }
        */
    }


    // Functions

    private void Initialize()
    {
        InitializeSDK();

        InitializePlayerData();

        InitializeSounds();
    }

    private void InitializeSDK()
    {
        //GameAnalytics.Initialize();

        /*
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            FB.Init(() => { FB.ActivateApp(); });
        }
        */
    }

    private void InitializeSounds()
    {
        Audios = new Dictionary<string, AudioClip>();


    }

    public void Save()
    {
        SerializeData();
    }

    #region Data Handling

    private void InitializePlayerData()
    {
        PlayerData = new PlayerData();

        dataPath = Path.Combine(Application.persistentDataPath, "GamingHouseManager.json");

        if (File.Exists(dataPath))
        {
            Debug.Log("File exists, loading.");

            DeserializeData();
        }
        else
        {
            Debug.Log("File doesn't exist, creating new.");

            File.Create(dataPath).Close();

            SerializeData();
        }
    }

    // Saves progress data.
    private void SerializeData()
    {
        string jsonDataString = JsonUtility.ToJson(PlayerData, true);

        File.WriteAllText(dataPath, jsonDataString);
    }

    // Loads progress data.
    private void DeserializeData()
    {
        string jsonDataString = File.ReadAllText(dataPath);

        PlayerData = JsonUtility.FromJson<PlayerData>(jsonDataString);
    }

    #endregion
}
