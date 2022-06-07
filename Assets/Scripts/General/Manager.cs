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

    private JsonData jsonData;


    // Instantiatable Objects

    public GameObject[] MemberPrefabs;


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

        Audios.Add("EnergyAcquired", Resources.Load("Audios/EnergyAcquired") as AudioClip);
        Audios.Add("EnergyPickup", Resources.Load("Audios/EnergyPickup") as AudioClip);
        Audios.Add("Money", Resources.Load("Audios/Money") as AudioClip);
    }

    public void Save()
    {
        SerializeData();
    }

    #region Data Handling

    private void InitializePlayerData()
    {
        PlayerData = new PlayerData();
        jsonData = new JsonData();

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
        jsonData.Money = PlayerData.Money;
        jsonData.TournamentsWon = PlayerData.TournamentsWon;
        jsonData.BoothLevels = PlayerData.BoothLevels;
        jsonData.MainHubLevel = PlayerData.MainHubLevel;

        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            jsonData.BoothPrices0[i] = PlayerData.BoothPrices[0][i];
        }
        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            jsonData.BoothPrices1[i] = PlayerData.BoothPrices[1][i];
        }
        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            jsonData.BoothPrices2[i] = PlayerData.BoothPrices[2][i];
        }
        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            jsonData.BoothPrices3[i] = PlayerData.BoothPrices[3][i];
        }
        for (int i = 0; i < PlayerData.MainHubLevelCount; i++)
        {
            jsonData.MainHubPrices[i] = PlayerData.MainHubPrices[i];
        }

        string jsonDataString = JsonUtility.ToJson(jsonData, true);

        File.WriteAllText(dataPath, jsonDataString);
    }

    // Loads progress data.
    private void DeserializeData()
    {
        string jsonDataString = File.ReadAllText(dataPath);

        jsonData = JsonUtility.FromJson<JsonData>(jsonDataString);

        PlayerData.Money = jsonData.Money;
        PlayerData.TournamentsWon = jsonData.TournamentsWon;
        PlayerData.BoothLevels = jsonData.BoothLevels;
        PlayerData.MainHubLevel = jsonData.MainHubLevel;

        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            PlayerData.BoothPrices[0][i] = jsonData.BoothPrices0[i];
        }
        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            PlayerData.BoothPrices[1][i] = jsonData.BoothPrices1[i];
        }
        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            PlayerData.BoothPrices[2][i] = jsonData.BoothPrices2[i];
        }
        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            PlayerData.BoothPrices[3][i] = jsonData.BoothPrices3[i];
        }
        for (int i = 0; i < PlayerData.MainHubLevelCount; i++)
        {
            PlayerData.MainHubPrices[i] = jsonData.MainHubPrices[i];
        }
    }

    #endregion
}

public class JsonData
{
    public int Money;
    public int TournamentsWon;

    public int[] BoothLevels = new int[PlayerData.BoothCount];
    public int MainHubLevel;

    public int[] BoothPrices0 = new int[PlayerData.BoothLevelCount];
    public int[] BoothPrices1 = new int[PlayerData.BoothLevelCount];
    public int[] BoothPrices2 = new int[PlayerData.BoothLevelCount];
    public int[] BoothPrices3 = new int[PlayerData.BoothLevelCount];
    public int[] MainHubPrices = new int[PlayerData.MainHubLevelCount];

    public JsonData()
    {
        
    }
}
