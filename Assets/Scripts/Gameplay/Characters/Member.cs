using UnityEngine;
using UnityEngine.AI;

public class Member : MonoBehaviour
{
    [HideInInspector]
    public int ID;
    
    //[HideInInspector]
    public MemberStates CurrentState;

    private NavMeshAgent Agent;

    private Animator Animator;

    private Transform SeatTransform;

    private const float StopDistance = 0.25f;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case MemberStates.Waiting_Disabled:

                

                break;

            case MemberStates.Walking_Seat:

                if (Vector3.Distance(transform.position, Agent.destination) < StopDistance)
                {
                    TakeSeat();
                }

                break;

            case MemberStates.Playing:



                break;

            case MemberStates.Sleeping:



                break;

            case MemberStates.Walking_Tournament:

                if (Vector3.Distance(transform.position, Agent.destination) < StopDistance)
                {
                    Agent.enabled = false;

                    transform.position = GameManager.Instance.MemberBusSpawnPoints[ID].position;
                    GameManager.Instance.Tournament.Bus.MemberEntered();

                    CurrentState = MemberStates.Waiting_Tournament;

                    gameObject.SetActive(false);
                }

                break;

            case MemberStates.Waiting_Tournament:



                break;

            default:



                break;
        }
    }

    public void InitializeMember(int id)
    {
        ID = id;
        SeatTransform = transform.parent.parent.GetComponent<Booth>().SeatTransforms[ID];
    }

    public void GoToSeat()
    {
        Agent.enabled = true;

        // TO DO -> Play "Run" animation here.
        Animator.SetBool("isWalking", true);

        Agent.SetDestination(SeatTransform.position);
        CurrentState = MemberStates.Walking_Seat;
    }

    public void TakeSeat()
    {
        Agent.enabled = false;

        transform.position = SeatTransform.GetChild(0).position;
        transform.rotation = SeatTransform.GetChild(0).rotation;

        Animator.SetBool("isWalking", false);
        Animator.SetBool("isSitting", true);
        
        if (transform.parent.parent.GetComponent<Booth>().UniformEnergyValue > 0f)
        {
            Play();
        }
        else
        {
            Sleep();
        }

        transform.parent.parent.GetComponent<Booth>().MemberSeated();
    }

    public void Sleep()
    {
        // TO DO -> Play "Sleeping" animation here.
        Animator.SetBool("isWorking", false);

        CurrentState = MemberStates.Sleeping;
    }

    public void Play()
    {
        // TO DO -> Play "Playing" animation here.
        Animator.SetBool("isWorking", true);


        CurrentState = MemberStates.Playing;
    }

    public void GoToTournament()
    {
        transform.position = SeatTransform.GetChild(1).position;

        Agent.enabled = true;

        // TO DO -> Play "Run" animation here.
        Animator.SetBool("isWorking", false);
        Animator.SetBool("isSitting", false);
        Animator.SetBool("isWalking", true);


        Agent.SetDestination(GameManager.Instance.MemberBusArrivalPoint.position);

        CurrentState = MemberStates.Walking_Tournament;
    }
}
