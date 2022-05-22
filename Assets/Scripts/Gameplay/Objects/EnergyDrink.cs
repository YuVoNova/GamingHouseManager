using UnityEngine;

public class EnergyDrink : MonoBehaviour
{
    private Vector3 originPosition;

    [SerializeField]
    private float Speed;

    private bool isMagnetized;

    private void Awake()
    {
        originPosition = transform.position;

        isMagnetized = false;
    }

    private void Update()
    {
        if (isMagnetized)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, Player.Instance.transform.position) < 0.2f)
            {
                if (Player.Instance.AvailableForEnergyDrinks)
                {
                    Player.Instance.EnergyDrinkAcquired();
                }

                isMagnetized = false;

                transform.position = originPosition;

                gameObject.SetActive(false);
            }
        }
    }

    public void Magnetize()
    {
        isMagnetized = true;
    }
}
