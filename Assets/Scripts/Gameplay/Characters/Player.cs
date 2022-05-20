using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Singleton

    public static Player Instance;


    // Objects & Components

    [SerializeField]
    private PlayerController PlayerController;

    [SerializeField]
    private Animator Animator;

    public AudioSource AudioSource;

    public Image InteractionFiller;


    // Values




    // Unity Functions

    private void Awake()
    {
        Instance = this;


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


}
