using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    //[HideInInspector]
    public TutorialStates CurrentState;

    [SerializeField]
    private GameObject[] Indicators;

    private bool isTutorial;

    private float timer;
    private float duration;

    private void Awake()
    {
        duration = 3f;
        timer = duration;
        
        CurrentState = TutorialStates.S0;
    }

    private void Update()
    {
        if (isTutorial)
        {
            switch (CurrentState)
            {
                case TutorialStates.S0:

                    if (timer <= 0f)
                    {
                        StartCoroutine(S1());
                        CurrentState = TutorialStates.S1;
                    }
                    else
                    {
                        timer -= Time.deltaTime;
                    }

                    break;
                case TutorialStates.S1:

                    

                    break;
                case TutorialStates.S2:

                    if (timer <= 0f)
                    {
                        StartCoroutine(S3());
                        CurrentState = TutorialStates.S3;
                    }
                    else
                    {
                        timer -= Time.deltaTime;
                    }

                    break;
                case TutorialStates.S3:



                    break;
                case TutorialStates.S4:

                    StartCoroutine(S5());
                    CurrentState = TutorialStates.S5;

                    break;
                case TutorialStates.S5:



                    break;
                case TutorialStates.S6:

                    if (timer <= 0f)
                    {
                        StartCoroutine(S7());
                        CurrentState = TutorialStates.S7;
                    }
                    else
                    {
                        timer -= Time.deltaTime;
                    }

                    break;
                case TutorialStates.S7:



                    break;
                case TutorialStates.S8:

                    StartCoroutine(S9());
                    CurrentState = TutorialStates.S9;

                    break;
                case TutorialStates.S9:



                    break;
                default:
                    break;
            }
        }
    }

    public void CheckTutorial()
    {
        isTutorial = Manager.Instance.PlayerData.IsTutorial;

        if (isTutorial)
        {
            GameManager.Instance.InitializeTutorial();
            Player.Instance.InitializeTutorial();
        }
        else
        {
            GameManager.Instance.CameraMovement.DefaultTarget();
        }
    }

    public void BoothUnlocked()
    {
        Indicators[0].SetActive(false);

        duration = 10f;
        timer = duration;

        CurrentState = TutorialStates.S2;
    }

    public void EnergyDepleted()
    {
        Indicators[1].SetActive(false);

        duration = 0f;
        timer = duration;

        CurrentState = TutorialStates.S4;
    }

    public void EnergyDrinkAcquired()
    {
        Indicators[2].SetActive(false);

        duration = 2f;
        timer = duration;

        CurrentState = TutorialStates.S6;
    }

    public void EnergyAcquired()
    {
        Indicators[3].SetActive(false);

        duration = 0f;
        timer = duration;

        CurrentState = TutorialStates.S8;
    }


    // Coroutines

    private IEnumerator S1()
    {
        Indicators[0].SetActive(true);
        Player.Instance.Navigate(Indicators[0].transform.position);
        GameManager.Instance.CameraMovement.ChangeTarget(Indicators[0].transform.position);

        yield return new WaitForSecondsRealtime(3f);

        GameManager.Instance.CameraMovement.DefaultTarget();
    }

    private IEnumerator S3()
    {
        Indicators[1].SetActive(true);
        Player.Instance.Navigate(Indicators[1].transform.position);
        GameManager.Instance.CameraMovement.ChangeTarget(Indicators[1].transform.position);

        yield return new WaitForSecondsRealtime(3f);

        GameManager.Instance.CameraMovement.DefaultTarget();
    }

    private IEnumerator S5()
    {
        Indicators[2].SetActive(true);
        Player.Instance.Navigate(Indicators[2].transform.position);
        GameManager.Instance.CameraMovement.ChangeTarget(GameManager.Instance.BoothCameraPoints[0].position);

        yield return new WaitForSecondsRealtime(2.5f);

        GameManager.Instance.CameraMovement.ChangeTarget(Indicators[2].transform.position);

        yield return new WaitForSecondsRealtime(0.75f);

        GameManager.Instance.ActivateEnergyDrinkDispenser();

        yield return new WaitForSecondsRealtime(2.5f);

        GameManager.Instance.CameraMovement.DefaultTarget();
    }

    private IEnumerator S7()
    {
        Indicators[3].SetActive(true);
        Player.Instance.Navigate(Indicators[3].transform.position);
        GameManager.Instance.CameraMovement.ChangeTarget(Indicators[3].transform.position);

        yield return new WaitForSecondsRealtime(0.75f);

        GameManager.Instance.ActivateTeamEnergy();

        yield return new WaitForSecondsRealtime(2.5f);

        GameManager.Instance.CameraMovement.DefaultTarget();
    }

    private IEnumerator S9()
    {
        GameManager.Instance.CameraMovement.ChangeTarget(GameManager.Instance.BoothCameraPoints[0].position);

        yield return new WaitForSecondsRealtime(2f);

        GameManager.Instance.CameraMovement.ChangeTarget(Indicators[4].transform.position);

        yield return new WaitForSecondsRealtime(0.75f);

        GameManager.Instance.TutorialTournament();

        yield return new WaitForSecondsRealtime(4f);
        
        GameManager.Instance.CameraMovement.ChangeTarget(Indicators[5].transform.position);

        yield return new WaitForSecondsRealtime(4f);

        Indicators[6].SetActive(true);
        GameManager.Instance.CameraMovement.ChangeTarget(Indicators[6].transform.position);

        yield return new WaitForSecondsRealtime(2.5f);

        GameManager.Instance.CameraMovement.ChangeTarget(Indicators[5].transform.position);

        GameManager.Instance.CameraMovement.DefaultTarget();

        Indicators[6].SetActive(false);

        GameManager.Instance.FinalizeTutorial();
    }
}
