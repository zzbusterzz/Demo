using System.Collections;
using UnityEngine;

public enum GameState
{
    NewGame,
    WaitTime,
    StartRace,
    CheckWinner,
    EndRace,
    Pause
}

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public AIMove aiPlayer;
    public PlayerMove user;
    public MoveCamera gameCamera;
    public GameUIControl gameUIControl;
    public MenuUIControl menuUIControl;

    public Transform beginPoint;
    public Transform endPoint;
    public GameState currentGS = GameState.Pause;
    
    private int timerCount = 3;
    private int singleTrackLength = 25;
    private float currentTime = 0f;
    private bool isWaiting = false;
    private GameObject winner = null;
    private Coroutine displayTextCoroutine = null;
    private GameState prevGS = GameState.NewGame;

    public void Start()
    {
        instance = this;
        menuUIControl.UpdateResumeButtonState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGS == GameState.Pause && menuUIControl.GetResumeButtonState())
                ResumeGame();
            else
                PauseGame();
        }

        switch (currentGS)
        {
            case GameState.NewGame:
                if (displayTextCoroutine != null)
                    StopCoroutine(displayTextCoroutine);

                int finalTrackLength = singleTrackLength * Random.Range(5, 10);
                endPoint.position = beginPoint.transform.position + Vector3.right * finalTrackLength;

                aiPlayer.ResetAI();
                user.ResetPlayer();
                gameCamera.ResetCamera();
                menuUIControl.ToggleMenuUI(false);
                menuUIControl.UpdateResumeButtonState(false);
                gameUIControl.ToggleIngameUI(true);
                winner = null;
                isWaiting = false;
                SpawnObjects.instance.ResetObjects();

                currentGS++;
                
                break;

            case GameState.WaitTime:
                menuUIControl.UpdateResumeButtonState(true);
               
                WaitForRaceToBegin();
                break;

            case GameState.StartRace:
                aiPlayer.StartMove();
                currentGS++;
                break;

            case GameState.CheckWinner:
                if (winner != null)//Check for the winner here
                    currentGS++;
                break;

            case GameState.EndRace:
                menuUIControl.UpdateResumeButtonState(false);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //Restart Code here
                    NewGame();                    
                }
                break;
        }
    }

    public void NewGame()
    {
        currentGS = GameState.NewGame;
        SpawnObjects.instance.ResetObjects();
    }
     

    public void PauseGame()
    {
        prevGS = currentGS;
        gameUIControl.ToggleIngameUI(false);
        menuUIControl.ToggleMenuUI(true);
        currentGS = GameState.Pause;
        aiPlayer.StopMove();
    }

    public void ResumeGame()
    {
        currentGS = prevGS;
        gameUIControl.ToggleIngameUI(true);
        menuUIControl.ToggleMenuUI(false);
        if(currentGS == GameState.StartRace || currentGS == GameState.CheckWinner)
            aiPlayer.StartMove();
    }

    public void WaitForRaceToBegin()
    {
        if (!isWaiting)
        {
            timerCount = 3;
            currentTime = 0f;
            isWaiting = true;
            //Debug.Log("Begining Game in ");      
            gameUIControl.ToggleGameText(true, false);
            gameUIControl.GameText("Begining Game in");
            gameUIControl.ToggleTrafficLight(true);
        }
;
        if (timerCount - currentTime > 0)
        {
            gameUIControl.ChangeTrafficLight(timerCount - 1 - (int)currentTime);
            currentTime += Time.deltaTime;
        }
        else
        {
            currentGS++;//Go to begin game
            displayTextCoroutine = StartCoroutine(DiplayTextFor("Go", 1f));
            gameUIControl.ToggleTrafficLight(false);
            isWaiting = false;
        }
    }

    public void UpdateWinner(Transform winner)
    {
        if (this.winner == null)
        {
            this.winner = winner.gameObject;

            gameUIControl.ToggleGameText(true, false);

            gameUIControl.GameText("Winner is : " + winner.name + "\n Press Enter To Restart");
            currentGS++;
        }
    }

    public GameState GetGameState()
    {
        return currentGS;
    }

    IEnumerator DiplayTextFor(string text, float duration)
    {
        gameUIControl.GameText(text);
        yield return new WaitForSeconds(duration);
        gameUIControl.ToggleGameText(false);
        displayTextCoroutine = null;
    }
}
