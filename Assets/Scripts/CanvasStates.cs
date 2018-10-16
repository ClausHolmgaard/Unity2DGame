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

    [SerializeField]
    private Text startText;

    [SerializeField]
    private Text helpText;

    [SerializeField]
    private float helpTextDuration = 10.0f;

    [SerializeField]
    private float fadeOutTime = 1.0f;

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
                showStartMenu();
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
        startText.gameObject.SetActive(false);
    }

    void showGameOverMenu() {
        gameOverText.gameObject.SetActive(true);
    }

    void showPauseMenu() {
        helpText.gameObject.SetActive(false);
        pauseText.gameObject.SetActive(true);
    }

    void showStartMenu() {
        startText.gameObject.SetActive(true);
    }

    public void showHelpText() {
        StartCoroutine(HelpTextWithTimeout(helpTextDuration));
    }

    public void showHelpText(float duration) {
        StartCoroutine(HelpTextWithTimeout(duration));
    }

    IEnumerator HelpTextWithTimeout(float duration) {
        helpText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration - fadeOutTime);

        Color originalColor = helpText.color;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime) {
            helpText.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }

        helpText.gameObject.SetActive(false);
        helpText.color = originalColor;
    }

}
