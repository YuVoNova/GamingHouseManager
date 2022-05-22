using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEnergyDrinkDispenser : Interactable
{
    [SerializeField]
    private Transform EnergyDrinkParent;

    private List<EnergyDrink> energyDrinkList;

    private int currentIndex;

    [SerializeField]
    private float MagnetizeDuration;
    private float magnetizeTimer;

    [SerializeField]
    private float EnergyDrinkSpawnDuration;
    private float energyDrinkSpawnTimer;

    private bool isInteracting;


    protected override void Awake()
    {
        base.Awake();

        currentIndex = 0;

        energyDrinkList = new List<EnergyDrink>();
        foreach (Transform child in EnergyDrinkParent)
        {
            energyDrinkList.Add(child.GetComponent<EnergyDrink>());
        }

        energyDrinkSpawnTimer = EnergyDrinkSpawnDuration;
        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (energyDrinkSpawnTimer <= 0f)
        {
            SpawnEnergyDrink();

            energyDrinkSpawnTimer = EnergyDrinkSpawnDuration;
        }
        else
        {
            energyDrinkSpawnTimer -= Time.deltaTime;
        }
        if (isInteracting)
        {
            if (currentIndex > 0 && currentIndex <= energyDrinkList.Count && Player.Instance.AvailableForEnergyDrinks)
            {
                if (magnetizeTimer <= 0f)
                {
                    currentIndex--;

                    energyDrinkList[currentIndex].Magnetize();

                    magnetizeTimer = MagnetizeDuration;
                }
                else
                {
                    magnetizeTimer -= Time.deltaTime;
                }
            }
        }
        else
        {

        }
    }

    public override void ExitPreInteraction()
    {
        base.ExitPreInteraction();

        isInteracting = true;
    }

    public override void ExitInteraction()
    {
        base.ExitInteraction();

        energyDrinkSpawnTimer = EnergyDrinkSpawnDuration;
        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }

    private void SpawnEnergyDrink()
    {
        if (currentIndex != energyDrinkList.Count)
        {
            if (!energyDrinkList[currentIndex].gameObject.activeSelf)
            {
                energyDrinkList[currentIndex].gameObject.SetActive(true);
                currentIndex++;
            }
        }
    }


}
