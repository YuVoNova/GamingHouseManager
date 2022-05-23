using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tournament : MonoBehaviour
{
    [HideInInspector]
    public bool TournamentsOn;

    private int tournamentGameId;
    private int prizePool;

    private int[] teamId = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
    private int[] fixture = new int[8];

    private List<int> teamList;

    [SerializeField]
    private float TournamentCooldownDuration;
    private float tournamentCooldownTimer;

    [SerializeField]
    private float TournamentDuration;
    private float tournamentTimer;

    private void Awake()
    {
        tournamentCooldownTimer = TournamentCooldownDuration;
        tournamentTimer = TournamentDuration;

        teamList = new List<int>();


    }

    private void Update()
    {
        if (TournamentsOn)
        {
            if (tournamentCooldownTimer <= 0f)
            {
                if (tournamentTimer <= 0f)
                {
                    // TO DO -> Finalize tournament here.

                    tournamentCooldownTimer = TournamentCooldownDuration;
                }
                else
                {
                    tournamentTimer -= Time.deltaTime;
                }
            }
            else
            {
                tournamentCooldownTimer -= Time.deltaTime;

                if (tournamentCooldownTimer <= 0f)
                {
                    InitializeTournament();
                }
            }
        }
    }

    private void InitializeTournament()
    {
        teamList.Clear();

        for (int i = 0; i < teamId.Length; i++)
        {
            teamList.Add(teamId[i]);
        }

        tournamentGameId = GameManager.Instance.AvailableGameIDList[Random.Range(0, GameManager.Instance.AvailableGameIDList.Count)];

        int boothLevel = GameManager.Instance.GetBoothLevel(tournamentGameId);
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

        tournamentTimer = TournamentDuration;
    }
}
