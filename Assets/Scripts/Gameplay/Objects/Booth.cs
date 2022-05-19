using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booth : MonoBehaviour
{
    // Objects & Components

    [SerializeField]
    private Gamer[] Gamers = new Gamer[5];

    [SerializeField]
    private Game Game;

    [SerializeField]
    private GameObject[] BoothLevels;

    [SerializeField]
    private MoneyArea MoneyArea;


    // Values

    [SerializeField]
    private int CurrentBoothLevel;

    private bool isWorking;

    [SerializeField]
    private float moneySpawnDuration;
    private float moneySpawnTimer;


    // Unity Functions

    private void Awake()
    {
        isWorking = false;

        moneySpawnTimer = 0f;
    }

    private void Update()
    {
        if (isWorking)
        {
            if (moneySpawnTimer <= 0f)
            {
                MoneyArea.SpawnMoney();

                moneySpawnTimer = moneySpawnDuration;
            }
            else
            {
                moneySpawnTimer -= Time.deltaTime;
            }
        }
    }


    // Methods


}
