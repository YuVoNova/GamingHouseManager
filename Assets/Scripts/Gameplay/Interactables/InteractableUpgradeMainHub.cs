using UnityEngine;
using TMPro;

public class InteractableUpgradeMainHub : Interactable
{
    [SerializeField]
    private TMP_Text BuyText;
    [SerializeField]
    private TMP_Text PriceText;
    [SerializeField]
    private GameObject MoneyIcon;
    [SerializeField]
    private GameObject ArrowIcon;

    private int price;
    private int payValue;
    private int step;
    private int currentLevel;

    protected override void Awake()
    {
        base.Awake();

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

        if (Manager.Instance.PlayerData.Money > 0 && GameManager.Instance.IsGameOn)
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
                Manager.Instance.PlayerData.MainHubPrices[currentLevel] = price;

                GameManager.Instance.MoneySpent(payValue);

                UpdatePriceText();

                if (price == 0)
                {
                    Player.Instance.MoneyFlow.EndFlow();

                    GameManager.Instance.UpgradeMainHub();
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
        currentLevel = Manager.Instance.PlayerData.MainHubLevel;

        if (currentLevel < PlayerData.MainHubLevelCount)
        {
            price = Manager.Instance.PlayerData.MainHubPrices[currentLevel];
            step = Mathf.FloorToInt(Mathf.Clamp(price / 40f, 1f, float.MaxValue));

            BuyText.text = "Lv" + (currentLevel + 1) + "         " + "Lv" + (currentLevel + 2);

            if (!ArrowIcon.activeSelf)
            {
                ArrowIcon.SetActive(true);
            }

            UpdatePriceText();
        }
        else
        {
            BuyText.text = "Lv" + currentLevel;
            PriceText.text = "MAX";
            MoneyIcon.SetActive(false);
            ArrowIcon.SetActive(false);
        }
    }

    private void UpdatePriceText()
    {
        PriceText.text = price + "";
    }
}
