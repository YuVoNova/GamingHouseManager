using UnityEngine;

public enum InteractionTypes
{
    OneTime,
    Progress
}

public enum MemberStates
{
    Waiting_Disabled,
    Walking_Seat,
    Playing,
    Sleeping,
    Walking_Tournament,
    Waiting_Tournament
}

public enum BusStates
{
    F_Waiting_Disabled,
    F_Driving_Arrival,
    F_Waiting_Arrival,
    F_Driving_End,
    S_Waiting_Disabled,
    S_Driving_Arrival,
    S_Waiting_Arrival,
    S_Driving_End
}
