using System.Collections.Generic;
using UnityEngine;

public class InteractableMoneyArea : Interactable
{
    [SerializeField]
    private Transform AreaMoneyParent;

    [SerializeField]
    private int AmountPerMoney;

    private List<AreaMoney> areaMoneyList;

    private int currentIndex;

    [SerializeField]
    private float MagnetizeDuration;
    private float magnetizeTimer;

    [SerializeField]
    private float MoneySpawnDuration;
    private float moneySpawnTimer;

    [HideInInspector]
    public bool IsWorking;

    private bool isInteracting;


    protected override void Awake()
    {
        base.Awake();

        currentIndex = 0;

        areaMoneyList = new List<AreaMoney>();
        foreach (Transform child in AreaMoneyParent)
        {
            areaMoneyList.Add(child.GetComponent<AreaMoney>());
        }

        SetAreaMoneyAmount();

        moneySpawnTimer = MoneySpawnDuration;
        magnetizeTimer = MagnetizeDuration;

        IsWorking = false;
        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (IsWorking)
        {
            if (moneySpawnTimer <= 0f)
            {
                SpawnMoney();

                moneySpawnTimer = MoneySpawnDuration;
            }
            else
            {
                moneySpawnTimer -= Time.deltaTime;
            }
        }
        if (isInteracting)
        {
            if (currentIndex > 0 && currentIndex <= areaMoneyList.Count)
            {
                if (magnetizeTimer <= 0f)
                {
                    currentIndex--;

                    areaMoneyList[currentIndex].Magnetize();

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

        moneySpawnTimer = MoneySpawnDuration;
        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }

    private void SpawnMoney()
    {
        if (currentIndex != areaMoneyList.Count)
        {
            if (!areaMoneyList[currentIndex].gameObject.activeSelf)
            {
                areaMoneyList[currentIndex].gameObject.SetActive(true);
                currentIndex++;
            }
        }
    }

    private void SetAreaMoneyAmount()
    {
        // TO DO -> Set AmountPerMoney depending on Booth's level.

        foreach (AreaMoney areaMoney in areaMoneyList)
        {
            areaMoney.Amount = AmountPerMoney;
        }
    }
}
