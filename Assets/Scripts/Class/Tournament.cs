using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tournament : MonoBehaviour
{
    // Objects & Components

    [Header("Objects & Components", order = 0)]

    [SerializeField]
    private GameObject TournamentCanvas;
    [SerializeField]
    private Image GameLogo;
    [SerializeField]
    private TMP_Text TournamentTimerText;
    [SerializeField]
    private GameObject TournamentOnText;

    [SerializeField]
    private Sponsor[] Sponsors;

    public Bus Bus;


    // Values

    [Header("Values", order = 0)]

    [HideInInspector]
    public bool TournamentsOn;

    private bool isTournamentInitialized;

    private int tournamentGameId;
    private int prizePool;

    private List<int> teamList;

    private int[] teamId = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
    private int[] fixture = new int[8];

    [SerializeField]
    private float TournamentCooldownDuration;
    private float tournamentCooldownTimer;

    [SerializeField]
    private float TournamentDuration;
    private float tournamentTimer;

    private int minutes;
    private int seconds;


    private void Awake()
    {
        tournamentCooldownTimer = TournamentCooldownDuration;
        tournamentTimer = TournamentDuration;

        teamList = new List<int>();

        isTournamentInitialized = false;
    }

    private void Update()
    {
        if (TournamentsOn)
        {
            if (tournamentCooldownTimer <= 0f)
            {
                if (tournamentTimer <= 0f)
                {
                    Bus.StartBus(false);

                    GameManager.Instance.FinalizeTournament(tournamentGameId);

                    if (fixture[0] == 0)
                    {
                        Manager.Instance.PlayerData.TournamentsWon++;

                        for (int i = 0; i < Sponsors.Length; i++)
                        {
                            if (!Sponsors[i].IsUnlocked)
                            {
                                Sponsors[i].CheckRequirements();
                            }
                        }
                    }

                    UIManager.Instance.EnableTournamentResultsScreen(fixture, tournamentGameId);

                    isTournamentInitialized = false;

                    tournamentCooldownTimer = TournamentCooldownDuration;
                }
                else
                {
                    tournamentTimer -= Time.deltaTime;
                }
            }
            else
            {
                if (!isTournamentInitialized)
                {
                    InitializeTournament();
                }

                tournamentCooldownTimer -= Time.deltaTime;

                UpdateTournamentTimerText();

                if (tournamentCooldownTimer <= 0f)
                {
                    Bus.StartBus(true);

                    GameManager.Instance.InitializeTournament(tournamentGameId);

                    TournamentTimerText.gameObject.SetActive(false);
                    TournamentOnText.SetActive(true);
                }
            }
        }
    }


    // Private Methods

    private void InitializeTournament()
    {
        teamList.Clear();

        for (int i = 0; i < teamId.Length; i++)
        {
            teamList.Add(teamId[i]);
        }

        tournamentGameId = GameManager.Instance.AvailableGameIDList[Random.Range(0, GameManager.Instance.AvailableGameIDList.Count)];

        int boothLevel = Manager.Instance.PlayerData.BoothLevels[tournamentGameId];
        int playerPositionInFixture = 8 - Mathf.FloorToInt(boothLevel * Random.Range(1f, 8f / boothLevel));

        fixture[playerPositionInFixture] = 0;
        teamList.Remove(0);

        for (int i = 0; i < teamId.Length; i++)
        {
            if (i != playerPositionInFixture)
            {
                fixture[i] = teamList[Random.Range(0, teamList.Count)];
                teamList.Remove(fixture[i]);
            }
        }

        GameLogo.sprite = null;
        GameLogo.sprite = Manager.Instance.Games[tournamentGameId].Logo;
        TournamentOnText.SetActive(false);
        TournamentTimerText.gameObject.SetActive(true);

        UpdateTournamentTimerText();

        tournamentTimer = TournamentDuration;
        isTournamentInitialized = true;
    }

    private void UpdateTournamentTimerText()
    {
        minutes = Mathf.FloorToInt(tournamentCooldownTimer / 60f);
        seconds = Mathf.FloorToInt(tournamentCooldownTimer % 60);

        TournamentTimerText.text = minutes + ":" + ((seconds > 9) ? "" + seconds : "0" + seconds);
    }


    // Public Methods

    public void EnableTournaments(bool isEnabled)
    {
        TournamentsOn = isEnabled;

        if (isEnabled)
        {
            TournamentCanvas.SetActive(true);
            TournamentOnText.SetActive(false);
        }
    }
}
