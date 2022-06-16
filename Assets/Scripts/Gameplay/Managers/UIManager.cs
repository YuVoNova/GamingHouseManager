using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton

    public static UIManager Instance;


    // Money Panel

    [SerializeField]
    private TMP_Text MoneyText;


    // Tournament

    [SerializeField]
    private GameObject TournamentResultsScreen;

    [SerializeField]
    private RectTransform PlayerTeamIndicator;

    [SerializeField]
    private Image TournamentGameLogo;

    [SerializeField]
    private TeamPanel[] TeamPanels;


    // Unity Functions

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < TeamPanels.Length; i++)
        {
            TeamPanels[i].RankText.text = "" + (i + 1);
        }

        UpdateMoneyText();
    }

    // Methods

    public void EnableTournamentResultsScreen(int[] fixture, int gameId)
    {
        if (TournamentResultsScreen.activeSelf)
        {
            TournamentResultsScreen.SetActive(false);
        }

        TournamentGameLogo.sprite = null;
        TournamentGameLogo.sprite = Manager.Instance.Games[gameId].Logo;

        for (int i = 0; i < fixture.Length; i++)
        {
            TeamPanels[i].TeamLogo.sprite = null;
            TeamPanels[i].TeamLogo.sprite = Manager.Instance.Teams[fixture[i]].LogoCircle;

            TeamPanels[i].TeamNameText.text = Manager.Instance.Teams[fixture[i]].Name;

            if (fixture[i] == 0)
            {
                PlayerTeamIndicator.anchoredPosition = new Vector2(PlayerTeamIndicator.anchoredPosition.x, TeamPanels[i].GetComponent<RectTransform>().anchoredPosition.y);
            }
        }

        TournamentResultsScreen.SetActive(true);
    }

    public void UpdateMoneyText()
    {
        MoneyText.text = "" + Manager.Instance.PlayerData.Money;
    }
}
