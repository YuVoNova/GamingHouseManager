using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booth : MonoBehaviour
{
    // Objects & Components

    [Header("Objects & Components", order = 0)]

    [SerializeField]
    private Game Game;

    [SerializeField]
    private GameObject[] BoothLevels;

    private Member[] Members = new Member[5];

    [SerializeField]
    private int[] MemberSpawnID = new int[5];

    [SerializeField]
    private Transform MembersParent;

    [SerializeField]
    private InteractableMoneyArea InteractableMoneyArea;

    [SerializeField]
    private InteractableTeamEnergy InteractableTeamEnergy;

    [SerializeField]
    private InteractableBuyBooth InteractableBuyBooth;

    public Transform[] SeatTransforms;
    public Transform EnergyPoint;

    [SerializeField]
    private Renderer GroundRenderer;
    [SerializeField]
    private Renderer[] WallRenderers;


    // Values

    [Header("Values", order = 0)]

    public int ID;

    [HideInInspector]
    public int CurrentBoothLevel;

    [SerializeField]
    private float MaxEnergy;
    [SerializeField]
    private float EnergyStep;
    [SerializeField]
    private float EnergyDropDuration;

    private float currentEnergy;
    private float energyDropTimer;

    [HideInInspector]
    public float UniformEnergyValue;

    [HideInInspector]
    public bool IsWorking;
    [HideInInspector]
    public bool IsAtTournament;

    private int activeMemberCount;


    // Unity Functions

    private void Awake()
    {
        GroundRenderer.material = Manager.Instance.Games[ID].GroundMaterial;
        for (int i = 0; i < WallRenderers.Length; i++)
        {
            WallRenderers[i].materials = new Material[2] { Manager.Instance.Games[ID].WallMaterial, Manager.Instance.Games[ID].WallMaterial };
            //WallRenderers[i].materials[1] = Manager.Instance.Games[ID].WallMaterial;
        }

        currentEnergy = MaxEnergy;

        energyDropTimer = EnergyDropDuration;

        UniformEnergyValue = currentEnergy / MaxEnergy;

        IsAtTournament = false;
    }

    private void Update()
    {
        if (CurrentBoothLevel > 0)
        {
            UniformEnergyValue = currentEnergy / MaxEnergy;

            if (IsWorking)
            {
                if (currentEnergy > 0f)
                {
                    if (energyDropTimer <= 0f)
                    {
                        currentEnergy = Mathf.FloorToInt(Mathf.Clamp(currentEnergy - EnergyStep, 0f, MaxEnergy));

                        energyDropTimer = EnergyDropDuration;

                        if (currentEnergy <= 0f)
                        {
                            EnergyDepleted();
                        }
                    }
                    else
                    {
                        energyDropTimer -= Time.deltaTime;
                    }
                }
            }
        }
    }


    // Methods

    public void InitializeBooth()
    {
        CurrentBoothLevel = Manager.Instance.PlayerData.BoothLevels[ID];

        GameObject member;
        for (int i = 0; i < Members.Length; i++)
        {
            member = Instantiate(Manager.Instance.MemberPrefabs[MemberSpawnID[i]], MembersParent) as GameObject;
            Members[i] = member.GetComponent<Member>();
            Members[i].InitializeMember(i);
        }

        if (CurrentBoothLevel > 0)
        {
            UnlockedBooth(true);

            activeMemberCount = 5;

            IsWorking = true;
        }
        else
        {
            for (int i = 0; i < Members.Length; i++)
            {
                Members[i].gameObject.SetActive(false);
                Members[i].transform.position = GameManager.Instance.MemberInitialSpawnPoints[i].position;
                Members[i].transform.rotation = GameManager.Instance.MemberInitialSpawnPoints[i].rotation;
            }

            activeMemberCount = 0;

            IsWorking = false;
        }

        BoothLevels[CurrentBoothLevel].SetActive(true);
        GameManager.Instance.RebuildNavMesh();
    }

    private void UnlockedBooth(bool isInitial)
    {
        if (!GameManager.Instance.AvailableGameIDList.Contains(Game.ID))
        {
            GameManager.Instance.AddGameToList(Game.ID);
        }

        for (int i = 0; i < Members.Length; i++)
        {
            Members[i].gameObject.SetActive(true);

            if (isInitial)
            {
                Members[i].TakeSeat();
            }
            else
            {
                Members[i].GoToSeat();
            }
        }

        InteractableMoneyArea.gameObject.SetActive(true);
    }

    public void LevelUpBooth()
    {
        InteractableBuyBooth.gameObject.SetActive(false);
        InteractableBuyBooth.gameObject.SetActive(true);

        BoothLevels[CurrentBoothLevel].SetActive(false);

        Manager.Instance.PlayerData.BoothLevels[ID] = Mathf.Clamp(Manager.Instance.PlayerData.BoothLevels[ID] + 1, 0, 4);
        Manager.Instance.Save();

        CurrentBoothLevel = Manager.Instance.PlayerData.BoothLevels[ID];

        if (CurrentBoothLevel == 1)
        {
            UnlockedBooth(false);
        }

        BoothLevels[CurrentBoothLevel].SetActive(true);
    }

    public void EnergyAcquired()
    {
        energyDropTimer = EnergyDropDuration;

        currentEnergy = Mathf.FloorToInt(Mathf.Clamp(currentEnergy + GameManager.Instance.EnergyAmount, 0f, MaxEnergy));

        UpdateEnergyBar();

        if (!IsWorking && !IsAtTournament)
        {
            IsWorking = true;

            for (int i = 0; i < Members.Length; i++)
            {
                Members[i].Play();
            }

            // TO DO -> Disable any VFX / Animation / Sound based on depleted energy here.
        }
    }

    private void EnergyDepleted()
    {
        IsWorking = false;

        for (int i = 0; i < Members.Length; i++)
        {
            Members[i].Sleep();
        }

        // TO DO -> Enable any VFX / Animation / Sound based on depleted energy here.
    }

    private void UpdateEnergyBar()
    {
        // TO DO -> Update EnergyBar from BoothCanvas here.
    }

    public void InitializeTournament()
    {
        IsAtTournament = true;
        IsWorking = false;

        activeMemberCount = 0;

        for (int i = 0; i < Members.Length; i++)
        {
            Members[i].GoToTournament();
        }
    }
    
    public void FinalizeTournament()
    {
        IsAtTournament = false;
    }

    public void BusReturned()
    {
        for (int i = 0; i < Members.Length; i++)
        {
            Members[i].gameObject.SetActive(true);
            Members[i].GoToSeat();
        }
    }

    public void MemberSeated()
    {
        activeMemberCount = Mathf.Clamp(activeMemberCount + 1, 0, Members.Length);

        if (activeMemberCount >= Members.Length)
        {
            IsWorking = true;
        }
    }
}
