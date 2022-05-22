using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyDrink : MonoBehaviour
{
    private Vector3 originPosition;

    [SerializeField]
    private float Speed;

    private bool isMagnetized;

    private Booth targetBooth;
    private Vector3 targetPosition;

    private void Awake()
    {
        originPosition = transform.position;

        isMagnetized = false;
    }

    private void Update()
    {
        if (isMagnetized)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
            {
                targetBooth.EnergyAcquired();

                isMagnetized = false;

                transform.position = originPosition;

                targetBooth = null;
                targetPosition = Vector3.zero;

                gameObject.SetActive(false);
            }
        }
    }

    public void Magnetize(Booth targetBooth)
    {
        this.targetBooth = targetBooth;

        targetPosition = targetBooth.EnergyPoint.position;

        isMagnetized = true;
    }
}
