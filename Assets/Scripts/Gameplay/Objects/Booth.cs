using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booth : MonoBehaviour
{
    // Objects & Components

    [Header("Objects & Components", order = 0)]

    [SerializeField]
    private Gamer[] Gamers = new Gamer[5];

    [SerializeField]
    private Game Game;

    [SerializeField]
    private GameObject[] BoothLevels;

    [SerializeField]
    private InteractableMoneyArea InteractableMoneyArea;

    [SerializeField]
    private InteractableTeamEnergy InteractableTeamEnergy;

    public Transform EnergyPoint;


    // Values

    [Header("Values", order = 0)]

    //[HideInInspector]
    public int CurrentBoothLevel;

    [SerializeField]
    private float MaxEnergy;
    [SerializeField]
    private float EnergyStep;
    [SerializeField]
    private float EnergyDropDuration;

    private float currentEnergy;
    private float energyDropTimer;

    //[HideInInspector]
    public float UniformEnergyValue;


    // Unity Functions

    private void Awake()
    {
        currentEnergy = MaxEnergy;

        energyDropTimer = EnergyDropDuration;

        UniformEnergyValue = currentEnergy / MaxEnergy;
    }

    private void Update()
    {
        if (CurrentBoothLevel > 0)
        {
            UniformEnergyValue = currentEnergy / MaxEnergy;

            if (currentEnergy > 0f)
            {
                if (energyDropTimer <= 0f)
                {
                    currentEnergy = Mathf.FloorToInt(Mathf.Clamp(currentEnergy - EnergyStep, 0f, MaxEnergy));

                    energyDropTimer = EnergyDropDuration;

                    if (currentEnergy <= 0f)
                    {
                        InteractableMoneyArea.IsWorking = false;

                        // TO DO -> Enable any VFX / Animation / Sound based on depleted energy here.
                    }
                }
                else
                {
                    energyDropTimer -= Time.deltaTime;
                }
            }
        }
    }


    // Methods

    private void LevelUpBooth()
    {
        // TO DO -> If leveled up to 1, add Game.ID to GameManager.AvailableGameIDList.
        // TO DO -> If leveled up to 1, activate Interactables.


    }

    public void EnergyAcquired()
    {
        energyDropTimer = EnergyDropDuration;

        currentEnergy = Mathf.FloorToInt(Mathf.Clamp(currentEnergy + GameManager.Instance.EnergyAmount, 0f, MaxEnergy));

        UpdateEnergyBar();

        if (!InteractableMoneyArea.IsWorking)
        {
            InteractableMoneyArea.IsWorking = true;

            // TO DO -> Disable any VFX / Animation / Sound based on depleted energy here.
        }
    }

    private void UpdateEnergyBar()
    {
        // TO DO -> Update EnergyBar from BoothCanvas here.
    }
}
