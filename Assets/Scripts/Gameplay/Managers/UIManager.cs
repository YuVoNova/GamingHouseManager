using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton

    public static UIManager Instance;


    // In-Game Buttons

    public GameObject DropEnergyDrinksButtonObject;


    // Money Panel

    [SerializeField]
    private TMP_Text MoneyText;


    // Unity Functions

    private void Awake()
    {
        Instance = this;

        DropEnergyDrinksButtonObject.SetActive(false);
    }

    // Methods

    public void OnClickDropEnergyDrinksButton()
    {
        Player.Instance.DropEnergyDrinks();

        DropEnergyDrinksButtonObject.SetActive(false);
    }
}
