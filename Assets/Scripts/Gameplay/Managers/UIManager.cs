using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton

    public static UIManager Instance;


    // Money Panel

    [SerializeField]
    private TMP_Text MoneyText;


    // Unity Functions

    private void Awake()
    {
        Instance = this;


    }

    // Methods


}
