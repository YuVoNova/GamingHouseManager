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


    // Unity Functions

    private void Awake()
    {
        currentEnergy = MaxEnergy;

        energyDropTimer = EnergyDropDuration;

        UniformEnergyValue = currentEnergy / MaxEnergy;
    }

    private void Update()
    {
        if (CurrentBoothLevel > 0)
        {
            UniformEnergyValue = currentEnergy / MaxEnergy;

            if (currentEnergy > 0f)
            {
                if (energyDropTimer <= 0f)
                {
                    currentEnergy = Mathf.FloorToInt(Mathf.Clamp(currentEnergy - EnergyStep, 0f, MaxEnergy));

                    energyDropTimer = EnergyDropDuration;

                    if (currentEnergy <= 0f)
                    {
                        InteractableMoneyArea.IsWorking = false;

                        // TO DO -> Enable any VFX / Animation / Sound based on depleted energy here.
                    }
                }
                else
                {
                    energyDropTimer -= Time.deltaTime;
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
        }
        else
        {
            for (int i = 0; i < Members.Length; i++)
            {
                Members[i].gameObject.SetActive(false);
                Members[i].transform.position = GameManager.Instance.MemberInitialSpawnPoints[i].position;
                Members[i].transform.rotation = GameManager.Instance.MemberInitialSpawnPoints[i].rotation;
            }
        }

        BoothLevels[CurrentBoothLevel].SetActive(true);
        GameManager.Instance.RebuildNavMesh();
    }

    private void UnlockedBooth(bool isInitial)
    {
        if (!GameManager.Instance.AvailableGameIDList.Contains(Game.ID))
        {
            GameManager.Instance.AvailableGameIDList.Add(Game.ID);
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
                Members[i].Unlocked();
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

        if (!InteractableMoneyArea.IsWorking)
        {
            InteractableMoneyArea.IsWorking = true;

            // TO DO -> Disable any VFX / Animation / Sound based on depleted energy here.
        }
    }

    private void UpdateEnergyBar()
    {
        // TO DO -> Update EnergyBar from BoothCanvas here.
    }
}
