using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Stat health;

    [SerializeField]
    private Text pointText;

    GameState gameState;
    CarControls car;

    private float xScale;
    private int currentPoints = 0;
    public bool isAlive = true;

    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private AudioSource audioS;
    private EnemyGenerator spawner;
    private SpriteRenderer thisRenderer;

    private Vector3 position;

    private void Awake() {
        health.maxValue = 100.0f;
        health.currentValue = 100.0f;
    }

    // Use this for initialization
    void Start () {
        spawner = GetComponent<EnemyGenerator>();
        audioS = GetComponent<AudioSource>();
        car = GetComponent<CarControls>();
        gameState = GetComponent<GameState>();
        thisRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

        if(health.currentValue <= 0 && isAlive) {
            Debug.Log("Player died!");
            die();
        }

        if (gameState.getState() == GameState.GameStateEnum.Running) {
            handleControls();
        }
    }

    void FixedUpdate() {
        
    }

    
    public void restart() {
        health.maxValue = 100.0f;
        health.currentValue = 100.0f;
        isAlive = true;
    }

    void handleControls() {
        car.handleWeapons();
        car.doMovement();
        car.handleMoveStates();
    }

    public void reduceHealth(float reduceHP) {
        audioS.Play();
        health.reduceValue(reduceHP);
        StartCoroutine(flashRed());
    }

    IEnumerator flashRed() {
        Color oldColor = thisRenderer.color;
        thisRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        thisRenderer.color = oldColor;
    }

    public GameState getGameState() {
        return gameState;
    }

    void die() {
        isAlive = false;
        gameState.newEvent(GameState.GameEventsEnum.GameOver);
    }

    public void addPoints(int points) {
        currentPoints += points;
        pointText.text = currentPoints.ToString();
        updateSpawnRate();
    }

    private void updateSpawnRate() {
        float rate = Mathf.Pow(0.9995f, currentPoints);
        print("Setting spawn rate at: " + rate);
        spawner.setSpawnRate(rate);
    }
}
