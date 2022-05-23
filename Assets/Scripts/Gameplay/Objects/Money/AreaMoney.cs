using UnityEngine;

public class AreaMoney : MonoBehaviour
{
    [HideInInspector]
    public int Amount;

    private Vector3 originPosition;
    private Quaternion originRotation;

    [SerializeField]
    private float Speed;

    private bool isMagnetized;

    private void Awake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;

        isMagnetized = false;
    }

    private void Update()
    {
        if (isMagnetized)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, Speed * Time.deltaTime);
            transform.LookAt(Player.Instance.transform);

            if (Vector3.Distance(transform.position, Player.Instance.transform.position) < 0.2f)
            {
                GameManager.Instance.MoneyEarned(Amount);

                isMagnetized = false;

                transform.position = originPosition;
                transform.rotation = originRotation;

                gameObject.SetActive(false);
            }
        }
    }

    public void Magnetize()
    {
        isMagnetized = true;
    }
}