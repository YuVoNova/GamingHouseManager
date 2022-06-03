using UnityEngine;

public class MainHub : MonoBehaviour
{
    [HideInInspector]
    public int CurrentLevel;

    [SerializeField]
    private GameObject[] Levels;

    [SerializeField]
    private Renderer[] Walls;

    [SerializeField]
    private Renderer Ground;

    [SerializeField]
    private Material[] WallMaterials = new Material[3];

    [SerializeField]
    private Material[] GroundMaterials = new Material[3];

    private void Awake()
    {
        CurrentLevel = Manager.Instance.PlayerData.MainHubLevel;

        PrepareLevel();
    }

    public void LevelUp()
    {
        Levels[CurrentLevel].SetActive(false);

        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, 2);
        Manager.Instance.PlayerData.MainHubLevel = CurrentLevel;
        Manager.Instance.Save();

        PrepareLevel();
    }

    private void PrepareLevel()
    {
        Levels[CurrentLevel].SetActive(true);

        Ground.material = GroundMaterials[CurrentLevel];

        for (int i = 0; i < Walls.Length; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Walls[i].materials[j] = WallMaterials[CurrentLevel];
            }
        }
    }
}
