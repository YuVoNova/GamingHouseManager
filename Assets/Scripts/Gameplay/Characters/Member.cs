using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Member : MonoBehaviour
{
    [HideInInspector]
    public int ID;
    
    [HideInInspector]
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

    public void Unlocked()
    {
        Agent.enabled = true;

        // TO DO -> Play "Run" animation here.

        Agent.SetDestination(SeatTransform.position);
        CurrentState = MemberStates.Walking_Seat;
    }

    public void TakeSeat()
    {
        Agent.enabled = false;

        transform.position = SeatTransform.GetChild(0).position;
        transform.rotation = SeatTransform.GetChild(0).rotation;

        if (transform.parent.parent.GetComponent<Booth>().UniformEnergyValue > 0f)
        {
            // TO DO -> Play "Playing" animation here.

            CurrentState = MemberStates.Playing;
        }
        else
        {
            // TO DO -> Play "Sleeping" animation here.

            CurrentState = MemberStates.Sleeping;
        }
    }
}
