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
    private InteractableMoneyArea InteractableMoneyArea;


    // Values

    [SerializeField]
    private int CurrentBoothLevel;


    // Unity Functions

    private void Awake()
    {

    }

    private void Update()
    {

    }


    // Methods


}
