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
    public GameState GS = GameState.Pause;
    
    private int timerCount = 3;
    private bool isWaiting = false;
    private GameObject winner = null;
    private Coroutine displayTextCoroutine = null;
    private int SingleTrackLength = 25;
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
            if (GS == GameState.Pause && menuUIControl.GetResumeButtonState())
                ResumeGame();
            else
                PauseGame();
        }

        switch (GS)
        {
            case GameState.NewGame:



                int finalTrackLength = SingleTrackLength * Random.Range(5, 10);
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

                GS++;
                
                break;

            //case GameState.PressToStart:
            //    InGameUIControl.instance.ToggleGameText(true, false);
            //    InGameUIControl.instance.GameText("Press Enter to Start Game");

            //    if (Input.GetKeyDown(KeyCode.Return))//Goes to wait time
            //    {
            //        winner = null;
            //        isWaiting = false;
            //        GS++;
            //    }
            //    break;

            case GameState.WaitTime:
                menuUIControl.UpdateResumeButtonState(true);
                if (!isWaiting)
                    WaitForRaceToBegin();
                break;

            case GameState.StartRace:
                aiPlayer.StartMove();
                GS++;
                break;

            case GameState.CheckWinner:
                if (winner != null)//Check for the winner here
                    GS++;
                break;

            case GameState.EndRace:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    NewGame();
                    //Restart Code here
                }
                break;
        }
    }

    public void NewGame()
    {
        GS = GameState.NewGame;
        SpawnObjects.instance.ResetObjects();
    }
     

    public void PauseGame()
    {
        prevGS = GS;
        gameUIControl.ToggleIngameUI(false);
        menuUIControl.ToggleMenuUI(true);
        GS = GameState.Pause;
        aiPlayer.StopMove();
    }

    public void ResumeGame()
    {
        GS = prevGS;
        gameUIControl.ToggleIngameUI(true);
        menuUIControl.ToggleMenuUI(false);
        aiPlayer.StartMove();
    }
    
    public void WaitForRaceToBegin()
    {
        timerCount = 3;
        isWaiting = true;
        //Debug.Log("Begining Game in ");      
        gameUIControl.ToggleGameText(true, false);
        gameUIControl.GameText("Begining Game in");
        gameUIControl.ToggleTrafficLight(true);
        
        StartCoroutine(DisplayRemainingTime());
    }

    public void UpdateWinner(Transform winner)
    {
        if (this.winner == null)
        {
            this.winner = winner.gameObject;

            gameUIControl.ToggleGameText(true, false);

            gameUIControl.GameText("Winner is : " + winner.name + "\n Press Enter To Restart");
            GS++;
        }
    }

    public GameState GetGameState()
    {
        return GS;
    }

    IEnumerator DisplayRemainingTime()
    {
        gameUIControl.ChangeTrafficLight(timerCount - 1);

        yield return new WaitForSeconds(1f);        

        timerCount--;

        if (timerCount > 0)
            StartCoroutine(DisplayRemainingTime());
        else
        {
            GS++;//Go to begin game
            displayTextCoroutine = StartCoroutine(DiplayTextFor("Go", 1f));
            gameUIControl.ToggleTrafficLight(false);
            isWaiting = false;
        }
    }

    IEnumerator DiplayTextFor(string text, float duration)
    {
        gameUIControl.GameText(text);
        yield return new WaitForSeconds(duration);
        gameUIControl.ToggleGameText(false);
        displayTextCoroutine = null;
    }
}
