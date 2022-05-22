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


    // Values

    [Header("Values", order = 0)]

    public float EnergyAmount;

    [HideInInspector]
    public bool IsGameOn;
    [HideInInspector]
    public bool OnMenu;


    // Unity Functions

    private void Awake()
    {
        Instance = this;



        NavMeshSurface.BuildNavMesh();

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

    public void MoneyEarned(int amount)
    {
        /*
        Manager.Instance.PlayerData.Money += Mathf.FloorToInt(amount * MoneyMultiplier);

        UIManager.UpdateMoneyText();

        Player.AudioSource.volume = 0.4f;
        Player.AudioSource.clip = Manager.Instance.Audios["Money"];
        Player.AudioSource.Play();

        Manager.Instance.Save();
        */
    }

    public void MoneySpent(int amount)
    {
        /*
        Manager.Instance.PlayerData.Money = Mathf.FloorToInt(Mathf.Clamp(Manager.Instance.PlayerData.Money - amount, 0f, float.MaxValue));

        UIManager.UpdateMoneyText();

        Player.AudioSource.volume = 0.4f;
        Player.AudioSource.clip = Manager.Instance.Audios["Money"];
        Player.AudioSource.Play();

        Manager.Instance.Save();
        */
    }
}
