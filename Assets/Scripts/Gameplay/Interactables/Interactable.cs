using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private float Duration;

    [SerializeField]
    private float PreInteractionDuration;

    private float currentDuration;

    private bool hasPreInteraction;

    private float interactionTimer;
    private float preInteractionTimer;

    public InteractionTypes InteractionType;

    protected virtual void Awake()
    {
        interactionTimer = 0f;
        preInteractionTimer = 0f;

        hasPreInteraction = true;
    }

    protected virtual void Update()
    {
        if (hasPreInteraction)
        {
            if (preInteractionTimer > 0f)
            {
                preInteractionTimer -= Time.deltaTime;

                if (preInteractionTimer <= 0f)
                {
                    hasPreInteraction = false;

                    ExitPreInteraction();
                }
            }
        }
        else
        {
            if (InteractionType == InteractionTypes.OneTime)
            {
                if (interactionTimer > 0f)
                {
                    interactionTimer -= Time.deltaTime;

                    Player.Instance.InteractionFiller.fillAmount = 1f - (interactionTimer / currentDuration);

                    if (interactionTimer <= 0f)
                    {
                        Interacted();
                    }
                }
            }
            else if (InteractionType == InteractionTypes.Progress)
            {
                ProgressInteraction();
            }
        }
    }

    protected virtual void ProgressInteraction()
    {

    }

    public virtual void StartInteraction()
    {
        currentDuration = Duration;
        interactionTimer = Duration;

        preInteractionTimer = PreInteractionDuration;

        hasPreInteraction = true;

        Player.Instance.InteractionFiller.fillAmount = 0f;
    }

    public virtual void ExitPreInteraction()
    {
        Player.Instance.InteractionFiller.fillAmount = 0f;
    }

    public virtual void ExitInteraction()
    {
        interactionTimer = 0f;
        preInteractionTimer = 0f;

        hasPreInteraction = true;

        Player.Instance.InteractionFiller.fillAmount = 0f;
    }

    protected virtual void Interacted()
    {
        Player.Instance.InteractionFiller.fillAmount = 0f;
    }
}
