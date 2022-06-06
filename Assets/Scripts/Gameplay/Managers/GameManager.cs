using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    // Singleton

    public static GameManager Instance;


    // Objects & Components

    [Header("Objects & Components", order = 0)]

    [SerializeField]
    private NavMeshSurface NavMeshSurface;

    [SerializeField]
    private List<Booth> Booths;

    [SerializeField]
    private MainHub MainHub;

    [SerializeField]
    private GameObject InteractableUpgradeMainHub;

    [SerializeField]
    private GameObject InteractableStreamArea;

    public Tournament Tournament;

    public Transform[] MemberInitialSpawnPoints;
    public Transform[] MemberBusSpawnPoints;
    public Transform MemberBusArrivalPoint;


    // Values

    [Header("Values", order = 0)]

    [HideInInspector]
    public List<int> AvailableGameIDList;

    public float EnergyAmount;

    [HideInInspector]
    public bool IsGameOn;
    [HideInInspector]
    public bool OnMenu;

    private int waitingBusId;

    
    // Unity Functions

    private void Awake()
    {
        Instance = this;

        RebuildNavMesh();

        AvailableGameIDList = new List<int>();

        InitializeBooths();

        if (Manager.Instance.PlayerData.MainHubLevel > 0)
        {
            InteractableStreamArea.SetActive(true);
        }
        else
        {
            InteractableStreamArea.SetActive(false);
        }

        IsGameOn = false;
        OnMenu = false;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (IsGameOn)
        {

        }
    }

    private void FixedUpdate()
    {

    }


    // Methods

    private void StartGame()
    {
        IsGameOn = true;
    }

    private void InitializeBooths()
    {
        // TO DO -> Initialize Booths here depending on PlayerData.

        for (int i = 0; i < Booths.Count; i++)
        {
            Booths[i].InitializeBooth();
        }

        if (AvailableGameIDList.Count > 0)
        {
            Tournament.EnableTournaments(true);
        }
        else
        {
            Tournament.EnableTournaments(false);
        }
    }

    public void MoneyEarned(int amount)
    {
        Manager.Instance.PlayerData.Money += Mathf.FloorToInt(amount);

        //UIManager.UpdateMoneyText();

        //Player.AudioSource.volume = 0.4f;
        //Player.AudioSource.clip = Manager.Instance.Audios["Money"];
        //Player.AudioSource.Play();

        Manager.Instance.Save();
    }

    public void MoneySpent(int amount)
    {
        Manager.Instance.PlayerData.Money = Mathf.FloorToInt(Mathf.Clamp(Manager.Instance.PlayerData.Money - amount, 0f, float.MaxValue));

        //UIManager.UpdateMoneyText();

        //Player.AudioSource.volume = 0.4f;
        //Player.AudioSource.clip = Manager.Instance.Audios["Money"];
        //Player.AudioSource.Play();

        Manager.Instance.Save();
    }

    public void InitializeTournament(int boothId)
    {
        Booths[boothId].InitializeTournament();
    }

    public void FinalizeTournament(int boothId)
    {
        Booths[boothId].FinalizeTournament();
        waitingBusId = boothId;
    }

    public void RebuildNavMesh()
    {
        NavMeshSurface.BuildNavMesh();
    }

    public int GetBoothLevel(int boothId)
    {
        return Booths[boothId].CurrentBoothLevel;
    }

    public void UpgradeBooth(int boothId)
    {
        Booths[boothId].LevelUpBooth();

        RebuildNavMesh();
    } 

    public void UpgradeMainHub()
    {
        InteractableUpgradeMainHub.SetActive(false);
        InteractableUpgradeMainHub.SetActive(true);

        MainHub.LevelUp();

        if (MainHub.CurrentLevel > 0 && !InteractableStreamArea.activeSelf)
        {
            InteractableStreamArea.SetActive(true);
        }
    }

    public void AddGameToList(int gameId)
    {
        AvailableGameIDList.Add(gameId);

        if (AvailableGameIDList.Count > 0 && !Tournament.TournamentsOn)
        {
            Tournament.EnableTournaments(true);
        }
    }

    public void BusReturned()
    {
        Booths[waitingBusId].BusReturned();
    }
}
