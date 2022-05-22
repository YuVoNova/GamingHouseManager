using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTeamEnergy : Interactable
{
    [SerializeField]
    private float MagnetizeDuration;
    private float magnetizeTimer;

    private bool isInteracting;


    protected override void Awake()
    {
        base.Awake();

        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isInteracting)
        {
            if (magnetizeTimer <= 0f)
            {
                if (transform.parent.GetComponent<Booth>().UniformEnergyValue < 0.9f)
                {
                    magnetizeTimer = MagnetizeDuration;

                    Player.Instance.EnergyDrinkSpent(transform.parent.GetComponent<Booth>());
                }
            }
            else
            {
                magnetizeTimer -= Time.deltaTime;
            }
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

        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }
}
