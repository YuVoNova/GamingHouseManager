using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableBuyBooth : Interactable
{
    [SerializeField]
    private TMP_Text BuyText;
    [SerializeField]
    private TMP_Text PriceText;
    [SerializeField]
    private GameObject MoneyIcon;

    private int price;
    private int payValue;
    private int step;
    private int boothId;
    private int currentLevel;

    protected override void Awake()
    {
        base.Awake();

        boothId = transform.parent.GetComponent<Booth>().ID;

        PaymentCompleted();
    }

    public override void ExitPreInteraction()
    {
        base.ExitPreInteraction();

        if (price != 0)
        {
            Player.Instance.MoneyFlow.StartFlow(Player.Instance.transform, transform);
        }
    }

    public override void ExitInteraction()
    {
        base.ExitInteraction();

        Player.Instance.MoneyFlow.EndFlow();
    }

    protected override void ProgressInteraction()
    {
        base.ProgressInteraction();

        if (Manager.Instance.PlayerData.Money > 0)
        {
            if (price != 0)
            {
                if (Manager.Instance.PlayerData.Money >= step)
                {
                    payValue = Mathf.FloorToInt(Mathf.Clamp(step, 0f, price));
                }
                else
                {
                    if (Manager.Instance.PlayerData.Money >= price)
                    {
                        payValue = price;
                    }
                    else
                    {
                        payValue = Manager.Instance.PlayerData.Money;
                    }
                }

                price = Mathf.FloorToInt(Mathf.Clamp(price - payValue, 0f, float.MaxValue));
                Manager.Instance.PlayerData.BoothPrices[boothId][currentLevel] = price;

                GameManager.Instance.MoneySpent(payValue);

                UpdatePriceText();

                if (price == 0)
                {
                    Player.Instance.MoneyFlow.EndFlow();

                    GameManager.Instance.UpgradeBooth(boothId);
                    PaymentCompleted();
                }
            }
        }
        else
        {
            Player.Instance.MoneyFlow.EndFlow();
        }
    }

    private void PaymentCompleted()
    {
        currentLevel = Manager.Instance.PlayerData.BoothLevels[boothId];

        if (currentLevel < 4)
        {
            price = Manager.Instance.PlayerData.BoothPrices[boothId][currentLevel];
            step = Mathf.FloorToInt(Mathf.Clamp(price / 40f, 1f, float.MaxValue));

            if (currentLevel == 0)
            {
                BuyText.text = "Buy Room";
            }
            else
            {
                BuyText.text = "Level " + (currentLevel + 1);
            }

            UpdatePriceText();
        }
        else
        {
            BuyText.gameObject.SetActive(false);
            PriceText.text = "MAX";
            MoneyIcon.SetActive(false);
        }
    }

    private void UpdatePriceText()
    {
        PriceText.text = price + "";
    }
}
