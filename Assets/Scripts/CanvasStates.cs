using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStates : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Text gameOverText;

    [SerializeField]
    private Text pauseText;

    private GameState gameState;

	// Use this for initialization
	void Start () {

        //gameState = player.GetComponent<PlayerController>().getGameState();
        gameState = player.GetComponent<GameState>();
        
	}
	
	// Update is called once per frame
	void Update () {

        if(gameState.isStateChanged()) {
            hideAllMenu();
        }

        switch (gameState.getState()) {
            case GameState.GameStateEnum.PauseMenu:
                showPauseMenu();
                break;
            case GameState.GameStateEnum.StartMenu:
                break;
            case GameState.GameStateEnum.Running:
                break;
            case GameState.GameStateEnum.GameOver:
                showGameOverMenu();
                break;
            default:
                break;
        }
    }

    void hideAllMenu() {
        gameOverText.gameObject.SetActive(false);
        pauseText.gameObject.SetActive(false);
    }

    void showGameOverMenu() {
        gameOverText.gameObject.SetActive(true);
    }

    void showPauseMenu() {
        pauseText.gameObject.SetActive(true);
    }

    void showStartMenu() {

    }
}
