using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableMoneyArea : Interactable
{
    [SerializeField]
    private Transform AreaMoneyParent;

    [SerializeField]
    private TMP_Text MoneyPerSecondText;

    [SerializeField]
    private int[] AmountPerMoneyLevels;
    private int amountPerMoney;

    private List<AreaMoney> areaMoneyList;

    private int currentIndex;

    [SerializeField]
    private float MagnetizeDuration;
    private float magnetizeTimer;

    [SerializeField]
    private float MoneySpawnDuration;
    private float moneySpawnTimer;

    private bool isInteracting;


    protected override void Awake()
    {
        base.Awake();

        currentIndex = 0;

        moneySpawnTimer = MoneySpawnDuration;
        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (transform.parent.GetComponent<Booth>().IsWorking)
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

    public void Initialize(int level)
    {
        areaMoneyList = new List<AreaMoney>();
        foreach (Transform child in AreaMoneyParent)
        {
            areaMoneyList.Add(child.GetComponent<AreaMoney>());
        }

        SetAreaMoneyAmount(level);
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

    public void SetAreaMoneyAmount(int level)
    {
        amountPerMoney = AmountPerMoneyLevels[level];

        foreach (AreaMoney areaMoney in areaMoneyList)
        {
            areaMoney.Amount = amountPerMoney;
        }

        MoneyPerSecondText.text = (amountPerMoney / MoneySpawnDuration).ToString("F1") + "$/s";
    }
}
