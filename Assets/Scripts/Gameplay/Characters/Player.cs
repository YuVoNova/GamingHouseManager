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

    public Animator Animator;

    [SerializeField]
    private Transform EnergyDrinkParent;

    private List<GameObject> energyDrinks;

    public MoneyFlow MoneyFlow;
    public Transform MoneyFlowPoint;

    public AudioSource AudioSource;

    public Image InteractionFiller;

    [SerializeField]
    private Transform Navigator;


    // Values

    [Header("Values", order = 0)]

    private int currentEnergyDrinkCount;
    private int energyDrinkCapacity;

    [HideInInspector]
    public bool AvailableForEnergyDrinks;

    private Vector3 navigationTarget;
    private bool isNavigating;


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

        navigationTarget = Vector3.zero;
        isNavigating = false;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (isNavigating)
        {
            Navigator.LookAt(navigationTarget);
        }
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

        AudioSource.volume = 0.4f;
        AudioSource.clip = Manager.Instance.Audios["EnergyPickup"];
        AudioSource.Play();

        if (currentEnergyDrinkCount >= energyDrinkCapacity)
        {
            AvailableForEnergyDrinks = false;
        }

        if (Manager.Instance.PlayerData.IsTutorial && GameManager.Instance.Tutorial.CurrentState == TutorialStates.S5)
        {
            GameManager.Instance.Tutorial.EnergyDrinkAcquired();
        }
    }

    public void EnergyDrinkSpent(Booth targetBooth)
    {
        if (currentEnergyDrinkCount > 0)
        {
            currentEnergyDrinkCount--;

            energyDrinks[currentEnergyDrinkCount].GetComponent<PlayerEnergyDrink>().Magnetize(targetBooth);

            AudioSource.volume = 0.4f;
            AudioSource.clip = Manager.Instance.Audios["EnergyAcquired"];
            AudioSource.Play();

            if (currentEnergyDrinkCount < energyDrinkCapacity)
            {
                AvailableForEnergyDrinks = true;
            }
        }
    }

    public void ToggleCollider(bool isEnabled)
    {
        GetComponent<Collider>().enabled = isEnabled;
    }

    public void InitializeTutorial()
    {
        Navigator.gameObject.SetActive(true);
    }

    public void FinalizeTutorial()
    {
        Navigator.gameObject.SetActive(false);

        isNavigating = false;
    }

    public void Navigate(Vector3 target)
    {
        navigationTarget = target;

        isNavigating = true;
    }
}
