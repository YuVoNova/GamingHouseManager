using UnityEngine;
using TMPro;

public class InteractableStreamArea : Interactable
{
    [SerializeField]
    private TMP_Text AmountText;

    [SerializeField]
    private Transform MoneyFlowPoint;

    private int amountLimit;
    private int currentAmount;

    [SerializeField]
    private int Step;
    private int payValue;

    private float tickAmount;
    [SerializeField]
    private float TickDuration;
    private float timer;

    [SerializeField]
    private float PayDuration;
    private float payTimer;

    private bool isInteracting;

    protected override void Awake()
    {
        base.Awake();

        currentAmount = 0;
        timer = 0f;
        payTimer = 0f;

        SetMoneyAmount();

        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (!isInteracting)
        {
            if (currentAmount < amountLimit)
            {
                if (timer <= 0f)
                {
                    currentAmount = Mathf.FloorToInt(Mathf.Clamp(currentAmount + tickAmount, 0f, amountLimit));

                    UpdateText();

                    timer = TickDuration;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
        }
    }

    protected override void ProgressInteraction()
    {
        base.ProgressInteraction();

        if (isInteracting)
        {
            if (payTimer <= 0f)
            {
                if (currentAmount > 0)
                {
                    if (Step > currentAmount)
                    {
                        payValue = currentAmount;
                    }
                    else
                    {
                        payValue = Step;
                    }

                    currentAmount -= payValue;
                    UpdateText();

                    GameManager.Instance.MoneyEarned(payValue);

                    if (currentAmount == 0)
                    {
                        Player.Instance.MoneyFlow.EndFlow();
                        isInteracting = false;
                    }
                }
                else
                {
                    Player.Instance.MoneyFlow.EndFlow();
                    isInteracting = false;
                }

                payTimer = PayDuration;
            }
            else
            {
                payTimer -= Time.deltaTime;
            }
        }
    }

    public override void ExitPreInteraction()
    {
        base.ExitPreInteraction();

        Player.Instance.MoneyFlow.StartFlow(transform, Player.Instance.MoneyFlowPoint);
        isInteracting = true;
    }

    public override void ExitInteraction()
    {
        base.ExitInteraction();

        Player.Instance.MoneyFlow.EndFlow();
        isInteracting = false;
    }

    private void UpdateText()
    {
        AmountText.text = currentAmount + "/" + amountLimit;
    }

    public void SetMoneyAmount()
    {
        if (Manager.Instance.PlayerData.MainHubLevel == 1)
        {
            tickAmount = 5;
            amountLimit = 300;
        }
        else if (Manager.Instance.PlayerData.MainHubLevel == 2)
        {
            tickAmount = 10;
            amountLimit = 500;
        }
    }
}
