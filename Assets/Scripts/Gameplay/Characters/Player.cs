using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Singleton

    public static Player Instance;


    // Objects & Components

    [Header("Objects & Components", order = 0)]

    [SerializeField]
    private PlayerController PlayerController;

    [SerializeField]
    private Animator Animator;

    [SerializeField]
    private Transform EnergyDrinkParent;

    private List<GameObject> energyDrinks;

    public MoneyFlow MoneyFlow;
    public Transform MoneyFlowPoint;

    public AudioSource AudioSource;

    public Image InteractionFiller;


    // Values

    [Header("Values", order = 0)]

    private int currentEnergyDrinkCount;
    private int energyDrinkCapacity;

    [HideInInspector]
    public bool AvailableForEnergyDrinks;


    // Unity Functions

    private void Awake()
    {
        Instance = this;

        energyDrinks = new List<GameObject>();
        foreach (Transform child in EnergyDrinkParent)
        {
            energyDrinks.Add(child.gameObject);
        }
        currentEnergyDrinkCount = 0;
        energyDrinkCapacity = energyDrinks.Count;

        AvailableForEnergyDrinks = true;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)        // Interactable
        {
            other.GetComponent<Interactable>().StartInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)        // Interactable
        {
            other.GetComponent<Interactable>().ExitInteraction();
        }
    }


    // Methods

    public void EnergyDrinkAcquired()
    {
        energyDrinks[currentEnergyDrinkCount].SetActive(true);

        currentEnergyDrinkCount++;

        if (!UIManager.Instance.DropEnergyDrinksButtonObject.activeSelf)
        {
            UIManager.Instance.DropEnergyDrinksButtonObject.SetActive(true);
        }

        if (currentEnergyDrinkCount >= energyDrinkCapacity)
        {
            AvailableForEnergyDrinks = false;
        }
    }

    public void EnergyDrinkSpent(Booth targetBooth)
    {
        if (currentEnergyDrinkCount > 0)
        {
            currentEnergyDrinkCount--;

            energyDrinks[currentEnergyDrinkCount].GetComponent<PlayerEnergyDrink>().Magnetize(targetBooth);

            if (currentEnergyDrinkCount == 0 && UIManager.Instance.DropEnergyDrinksButtonObject.activeSelf)
            {
                UIManager.Instance.DropEnergyDrinksButtonObject.SetActive(false);
            }

            if (currentEnergyDrinkCount < energyDrinkCapacity)
            {
                AvailableForEnergyDrinks = true;
            }
        }
    }

    public void DropEnergyDrinks()
    {
        currentEnergyDrinkCount = 0;

        foreach (GameObject energyDrink in energyDrinks)
        {
            energyDrink.SetActive(false);
        }

        AvailableForEnergyDrinks = true;
    }
}
