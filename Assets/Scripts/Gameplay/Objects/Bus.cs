using UnityEngine;

public class Bus : MonoBehaviour
{
    [SerializeField]
    private GameObject BusModel;

    [SerializeField]
    private Transform BusStartPoint;
    [SerializeField]
    private Transform BusArrivalPoint;
    [SerializeField]
    private Transform BusEndPoint;

    [SerializeField]
    private float Speed;
    [SerializeField]
    private float StoppingDistance;

    [HideInInspector]
    public BusStates CurrentState;

    private int memberCount;


    private void Awake()
    {
        CurrentState = BusStates.F_Waiting_Disabled;

        memberCount = 0;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case BusStates.F_Waiting_Disabled:



                break;
            case BusStates.F_Driving_Arrival:

                transform.position = Vector3.Lerp(transform.position, BusArrivalPoint.position, Speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, BusArrivalPoint.position) < StoppingDistance)
                {
                    transform.position = BusArrivalPoint.position;

                    memberCount = 0;

                    CurrentState = BusStates.F_Waiting_Arrival;
                }

                break;
            case BusStates.F_Waiting_Arrival:

                if (memberCount >= 5)
                {
                    CurrentState = BusStates.F_Driving_End;
                }

                break;
            case BusStates.F_Driving_End:

                transform.position = Vector3.MoveTowards(transform.position, BusEndPoint.position, Speed * 25f * Time.deltaTime);

                if (Vector3.Distance(transform.position, BusEndPoint.position) < StoppingDistance)
                {
                    transform.position = BusStartPoint.position;

                    BusModel.SetActive(false);

                    CurrentState = BusStates.S_Waiting_Disabled;
                }

                break;
            case BusStates.S_Waiting_Disabled:



                break;
            case BusStates.S_Driving_Arrival:

                transform.position = Vector3.Lerp(transform.position, BusArrivalPoint.position, Speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, BusArrivalPoint.position) < StoppingDistance)
                {
                    transform.position = BusArrivalPoint.position;

                    CurrentState = BusStates.S_Waiting_Arrival;
                }

                break;
            case BusStates.S_Waiting_Arrival:

                memberCount = 0;

                GameManager.Instance.BusReturned();

                CurrentState = BusStates.S_Driving_End;

                break;
            case BusStates.S_Driving_End:

                transform.position = Vector3.MoveTowards(transform.position, BusEndPoint.position, Speed * 25f * Time.deltaTime);

                if (Vector3.Distance(transform.position, BusEndPoint.position) < StoppingDistance)
                {
                    transform.position = BusStartPoint.position;

                    BusModel.SetActive(false);

                    CurrentState = BusStates.F_Waiting_Disabled;
                }

                break;
            default:

                break;
        }
    }

    public void StartBus(bool isFirst)
    {
        transform.position = BusStartPoint.position;
        BusModel.SetActive(true);

        if (isFirst)
        {
            CurrentState = BusStates.F_Driving_Arrival;
        }
        else
        {
            CurrentState = BusStates.S_Driving_Arrival;
        }
    }

    public void MemberEntered()
    {
        memberCount++;
    }
}
