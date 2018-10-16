using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

    [SerializeField]
    private GameObject canvas;

    private CanvasStates canvasStates;

	public enum GameStateEnum {
        PauseMenu,
        StartMenu,
        Running,
        GameOver
    }

    public enum GameEventsEnum {
        GameKeyExit,
        GameKeyConfirm,
        GameKeyPause,
        GameKeyFire,
        GameOver
    }

    private bool stateChanged = false;

    private GameStateEnum currentState = GameStateEnum.StartMenu;

    private void Start() {
        Time.timeScale = 0.0f;

        canvasStates = canvas.GetComponent<CanvasStates>();
    }

    private void Update() {
        if (Input.GetButtonDown("Pause")) {
            newEvent(GameEventsEnum.GameKeyPause);
        } else if (Input.GetButtonDown("Confirm")) {
            newEvent(GameEventsEnum.GameKeyConfirm);
        } else if (Input.GetButtonDown("Exit")) {
            newEvent(GameEventsEnum.GameKeyExit);
        } else if (Input.GetButtonDown("Fire1") ) {
            newEvent(GameEventsEnum.GameKeyFire);
        }
        
    }

    public GameStateEnum getState() {
        return currentState;
    }

    public void newEvent(GameEventsEnum e) {

        switch (currentState) {

            case GameStateEnum.PauseMenu:

                switch (e) {
                    case GameEventsEnum.GameKeyExit:
                        exitGame();
                        break;
                    case GameEventsEnum.GameKeyConfirm:
                    case GameEventsEnum.GameKeyFire:
                        continueGame();
                        canvasStates.showHelpText(5.0f);
                        break;
                    default:
                        noStateChange();
                        break;
                }

                break;

            case GameStateEnum.StartMenu:

                switch (e) {
                    case GameEventsEnum.GameKeyExit:
                        exitGame();
                        break;
                    case GameEventsEnum.GameKeyConfirm:
                    case GameEventsEnum.GameKeyFire:
                        continueGame();
                        canvasStates.showHelpText();
                        break;
                    default:
                        noStateChange();
                        break;
                }

                break;

            case GameStateEnum.Running:

                switch (e) {
                    case GameEventsEnum.GameKeyExit:
                        pauseGame();
                        break;
                    case GameEventsEnum.GameKeyPause:
                        pauseGame();
                        break;
                    case GameEventsEnum.GameOver:
                        gameOver();
                        break;
                    default:
                        noStateChange();
                        break;
                }

                break;

            case GameStateEnum.GameOver:

                switch (e) {
                    case GameEventsEnum.GameKeyExit:
                        exitGame();
                        break;
                    case GameEventsEnum.GameKeyConfirm:
                        restartGame();
                        break;
                    default:
                        noStateChange();
                        break;
                }

                break;
        }
    }

    void setStatePauseMenu() {
        stateChanged = true;
        currentState = GameStateEnum.PauseMenu;
    }

    void setStateStartMenu() {
        stateChanged = true;
        currentState = GameStateEnum.StartMenu;
    }

    void setStateRunning() {
        stateChanged = true;
        currentState = GameStateEnum.Running;
    }

    void setStateGameOver() {
        stateChanged = true;
        currentState = GameStateEnum.GameOver;
    }

    void noStateChange() {
        stateChanged = false;
    }

    public bool isStateChanged() {
        return stateChanged;
    }

    void changeState(GameStateEnum s) {
        print("State change: " + currentState + " => " + s);
        currentState = s;
        stateChanged = true;
    }

    void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        transform.GetComponent<PlayerController>().restart();
        changeState(GameStateEnum.Running);
        Time.timeScale = 1.0f;
    }

    void continueGame() {
        Time.timeScale = 1.0f;
        changeState(GameStateEnum.Running);
    }

    void exitGame() {
        Application.Quit();
    }

    void pauseGame() {
        Time.timeScale = 0.0f;
        changeState(GameStateEnum.PauseMenu);
    }

    void gameOver() {
        Time.timeScale = 0.0f;
        changeState(GameStateEnum.GameOver);
    }

}
