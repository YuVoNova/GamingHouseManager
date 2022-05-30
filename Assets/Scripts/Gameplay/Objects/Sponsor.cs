using UnityEngine;
using TMPro;

public class Sponsor : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    [SerializeField]
    private Sprite SponsorSprite;

    [SerializeField]
    private int TournamentWinsRequired;

    [SerializeField]
    private TMP_Text AmountText;

    [HideInInspector]
    public bool IsUnlocked;

    private void Awake()
    {
        IsUnlocked = false;

        CheckRequirements();
    }

    public void CheckRequirements()
    {
        if (!IsUnlocked)
        {
            if (Manager.Instance.PlayerData.TournamentsWon >= TournamentWinsRequired)
            {
                Unlock();
            }
        }
        else
        {
            UpdateText();
        }
    }

    private void Unlock()
    {
        SpriteRenderer.sprite = SponsorSprite;

        UpdateText();

        IsUnlocked = true;
    }

    private void UpdateText()
    {
        AmountText.text = Mathf.Clamp(Manager.Instance.PlayerData.TournamentsWon, 0, TournamentWinsRequired) + "/" + TournamentWinsRequired;
    }
}
