using System.Collections;
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
    [SerializeField]
    private GameObject InteractableEnergyDrinkDispenser;
    [SerializeField]
    private GameObject TournamentScreen;
    [SerializeField]
    private GameObject SponsorsObject;

    public Tournament Tournament;

    public Tutorial Tutorial;

    public Transform[] MemberInitialSpawnPoints;
    public Transform[] MemberBusSpawnPoints;
    public Transform MemberBusArrivalPoint;

    public CameraMovement CameraMovement;

    public Transform[] BoothCameraPoints;


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

    [HideInInspector]
    public int CurrentBoothOrder;


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

        IsGameOn = true;
        OnMenu = false;

        Tutorial.CheckTutorial();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {

    }


    // Methods

    private void InitializeBooths()
    {
        CurrentBoothOrder = 4;
        for (int i = 0; i < PlayerData.BoothCount; i++)
        {
            if (Manager.Instance.PlayerData.BoothLevels[i] == 0)
            {
                CurrentBoothOrder = i;
                break;
            }
        }

        for (int i = 0; i < Booths.Count; i++)
        {
            Booths[i].InitializeBooth();
        }

        if (CurrentBoothOrder < PlayerData.BoothCount)
        {
            Booths[CurrentBoothOrder].DisableClosed();
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

        UIManager.Instance.UpdateMoneyText();

        Player.Instance.AudioSource.volume = 0.5f;
        Player.Instance.AudioSource.clip = Manager.Instance.Audios["Money"];
        Player.Instance.AudioSource.Play();

        Manager.Instance.Save();
    }

    public void MoneySpent(int amount)
    {
        Manager.Instance.PlayerData.Money = Mathf.FloorToInt(Mathf.Clamp(Manager.Instance.PlayerData.Money - amount, 0f, float.MaxValue));

        UIManager.Instance.UpdateMoneyText();

        Player.Instance.AudioSource.volume = 0.5f;
        Player.Instance.AudioSource.clip = Manager.Instance.Audios["Money"];
        Player.Instance.AudioSource.Play();

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

    public void UpgradeBoothFunction(int boothId)
    {
        StartCoroutine(UpgradeBooth(boothId));
    }

    private IEnumerator UpgradeBooth(int boothId)
    {
        CameraMovement.ChangeTarget(BoothCameraPoints[boothId].position);

        yield return new WaitForSeconds(1.5f);

        Booths[boothId].LevelUpBooth();

        RebuildNavMesh();

        yield return new WaitForSeconds(1.5f);

        CameraMovement.DefaultTarget();

        if (Manager.Instance.PlayerData.IsTutorial && Tutorial.CurrentState == TutorialStates.S1)
        {
            Tutorial.BoothUnlocked();
        }
    }

    public void UpgradeMainHub()
    {
        InteractableUpgradeMainHub.SetActive(false);
        InteractableUpgradeMainHub.SetActive(true);

        MainHub.LevelUp();

        InteractableStreamArea.GetComponent<InteractableStreamArea>().SetMoneyAmount();

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

    public void BoothUnlocked()
    {
        CurrentBoothOrder++;

        if (CurrentBoothOrder < PlayerData.BoothCount)
        {
            Booths[CurrentBoothOrder].DisableClosed();
        }
    }

    public void InitializeTutorial()
    {
        InteractableUpgradeMainHub.SetActive(false);
        InteractableEnergyDrinkDispenser.SetActive(false);
        TournamentScreen.SetActive(false);
        SponsorsObject.SetActive(false);
    }

    public void ActivateEnergyDrinkDispenser()
    {
        InteractableEnergyDrinkDispenser.SetActive(true);
    }

    public void ActivateTeamEnergy()
    {
        Booths[0].ActivateTeamEnergy();
    }

    public void TutorialTournament()
    {
        Tournament.TutorialTournament();
        TournamentScreen.SetActive(true);
    }

    public void FinalizeTutorial()
    {
        SponsorsObject.SetActive(true);
        InteractableUpgradeMainHub.SetActive(true);
        Player.Instance.FinalizeTutorial();

        Manager.Instance.PlayerData.IsTutorial = false;
        Manager.Instance.Save();
    }
}
