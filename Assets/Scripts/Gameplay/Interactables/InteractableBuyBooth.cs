using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableBuyBooth : Interactable
{
    [SerializeField]
    private TMP_Text PriceText;

    private int price;
    private int payValue;
    private int step;
    private int boothId;

    protected override void Awake()
    {
        base.Awake();

        boothId = transform.parent.GetComponent<Booth>().ID;
        price = Manager.Instance.PlayerData.BoothPrices[boothId];
        step = Mathf.FloorToInt(Mathf.Clamp(price / 40f, 1f, float.MaxValue));

        UpdatePriceText();
    }

    public override void ExitPreInteraction()
    {
        base.ExitPreInteraction();

        Player.Instance.MoneyFlow.StartFlow(Player.Instance.transform, transform);
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
                Manager.Instance.PlayerData.BoothPrices[boothId] = price;

                GameManager.Instance.MoneySpent(payValue);

                UpdatePriceText();

                if (price == 0)
                {
                    Player.Instance.MoneyFlow.EndFlow();
                    GameManager.Instance.BoughtBooth(boothId);
                }
            }
        }
        else
        {
            Player.Instance.MoneyFlow.EndFlow();
        }
    }

    private void UpdatePriceText()
    {
        PriceText.text = price + "";
    }
}
