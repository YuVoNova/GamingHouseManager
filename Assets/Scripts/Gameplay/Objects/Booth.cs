using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Image GameLogo;

    [SerializeField]
    private GameObject EnergyCanvas;
    [SerializeField]
    private Slider EnergyBarSlider;

    [SerializeField]
    private GameObject SleepingVFX;

    [SerializeField]
    private GameObject ClosedObject;

    [SerializeField]
    private GameObject RoomObject;


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

    //[HideInInspector]
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
        }

        IsAtTournament = false;
    }

    private void Update()
    {
        if (CurrentBoothLevel > 0)
        {
            if (IsWorking)
            {
                if (currentEnergy > 0f)
                {
                    if (energyDropTimer <= 0f)
                    {
                        currentEnergy = Mathf.FloorToInt(Mathf.Clamp(currentEnergy - EnergyStep, 0f, MaxEnergy));

                        UpdateEnergyBar();

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

            UniformEnergyValue = currentEnergy / MaxEnergy;
        }
    }


    // Methods

    public void InitializeBooth()
    {
        CurrentBoothLevel = Manager.Instance.PlayerData.BoothLevels[ID];

        currentEnergy = MaxEnergy;
        energyDropTimer = EnergyDropDuration;
        UniformEnergyValue = currentEnergy / MaxEnergy;

        InteractableMoneyArea.Initialize(CurrentBoothLevel);

        GameObject member;
        for (int i = 0; i < Members.Length; i++)
        {
            member = Instantiate(Manager.Instance.MemberPrefabs[MemberSpawnID[i]], MembersParent) as GameObject;
            Members[i] = member.GetComponent<Member>();
            Members[i].InitializeMember(i);
        }

        if (CurrentBoothLevel > 0)
        {
            DisableClosed();

            UnlockedBooth(true);

            activeMemberCount = 5;

            IsWorking = true;
        }
        else
        {
            if (GameManager.Instance.CurrentBoothOrder < ID)
            {
                RoomObject.SetActive(false);
                InteractableBuyBooth.gameObject.SetActive(false);
                ClosedObject.SetActive(true);
            }

            for (int i = 0; i < Members.Length; i++)
            {
                Members[i].gameObject.SetActive(false);
                Members[i].transform.position = GameManager.Instance.MemberInitialSpawnPoints[i].position;
                Members[i].transform.rotation = GameManager.Instance.MemberInitialSpawnPoints[i].rotation;
            }

            activeMemberCount = 0;

            IsWorking = false;
        }

        GameLogo.sprite = Game.FullLogo;

        UpdateEnergyBar();

        if (GameManager.Instance.CurrentBoothOrder >= ID)
        {
            BoothLevels[CurrentBoothLevel].SetActive(true);
        }
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

        EnergyCanvas.SetActive(true);
        InteractableMoneyArea.gameObject.SetActive(true);
        if (!Manager.Instance.PlayerData.IsTutorial)
        {
            ActivateTeamEnergy();
        }

        GameManager.Instance.BoothUnlocked(isInitial);

        if (Manager.Instance.PlayerData.IsTutorial && GameManager.Instance.Tutorial.CurrentState == TutorialStates.S1)
        {
            currentEnergy = MaxEnergy * 0.3f;
            UniformEnergyValue = currentEnergy / MaxEnergy;

            UpdateEnergyBar();
        }
    }

    public void LevelUpBooth()
    {
        BoothLevels[CurrentBoothLevel].SetActive(false);

        Manager.Instance.PlayerData.BoothLevels[ID] = Mathf.Clamp(Manager.Instance.PlayerData.BoothLevels[ID] + 1, 0, PlayerData.BoothLevelCount);
        Manager.Instance.Save();

        CurrentBoothLevel = Manager.Instance.PlayerData.BoothLevels[ID];

        InteractableBuyBooth.PaymentCompleted();

        InteractableBuyBooth.gameObject.SetActive(false);
        InteractableBuyBooth.gameObject.SetActive(true);

        if (CurrentBoothLevel == 1)
        {
            DisableClosed();
            UnlockedBooth(false);
        }

        InteractableMoneyArea.SetAreaMoneyAmount(CurrentBoothLevel);

        BoothLevels[CurrentBoothLevel].SetActive(true);
    }

    public void EnergyAcquired()
    {
        energyDropTimer = EnergyDropDuration;

        currentEnergy = Mathf.FloorToInt(Mathf.Clamp(currentEnergy + GameManager.Instance.EnergyAmount, 0f, MaxEnergy));
        UniformEnergyValue = currentEnergy / MaxEnergy;

        UpdateEnergyBar();

        if (!IsWorking && !IsAtTournament)
        {
            IsWorking = true;

            for (int i = 0; i < Members.Length; i++)
            {
                Members[i].Play();
            }
        }

        if (SleepingVFX.activeSelf)
        {
            SleepingVFX.SetActive(false);
        }

        if (Manager.Instance.PlayerData.IsTutorial && GameManager.Instance.Tutorial.CurrentState == TutorialStates.S7)
        {
            GameManager.Instance.Tutorial.EnergyAcquired();
        }
    }

    private void EnergyDepleted()
    {
        IsWorking = false;

        for (int i = 0; i < Members.Length; i++)
        {
            Members[i].Sleep();
        }

        SleepingVFX.SetActive(true);

        if (Manager.Instance.PlayerData.IsTutorial && GameManager.Instance.Tutorial.CurrentState == TutorialStates.S3)
        {
            GameManager.Instance.Tutorial.EnergyDepleted();
        }
    }

    private void UpdateEnergyBar()
    {
        EnergyBarSlider.value = UniformEnergyValue;
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

        if (activeMemberCount >= Members.Length && currentEnergy > 0f)
        {
            IsWorking = true;
        }
    }

    public void DisableClosed()
    {
        ClosedObject.SetActive(false);
        InteractableBuyBooth.gameObject.SetActive(true);
        RoomObject.SetActive(true);
        BoothLevels[CurrentBoothLevel].SetActive(true);
    }

    public void ActivateTeamEnergy()
    {
        InteractableTeamEnergy.gameObject.SetActive(true);
    }
}
