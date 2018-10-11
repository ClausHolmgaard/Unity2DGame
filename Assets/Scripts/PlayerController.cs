using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Stat health;

    [SerializeField]
    private Text gameOverText;

    [SerializeField]
    private Text pointText;

    GameState gameState = new GameState();
    CarControls car;

    private float groundOverlapRadius = 0.1f;
    private bool isGrounded = true;
    private bool isRunning;
    private float xScale;
    private int currentPoints = 0;
    public bool isAlive = true;

    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private Animator playerAnimator;
    private AudioSource audio;
    private EnemyGenerator spawner;

    private Vector3 position;

    private void Awake() {
        health.maxValue = 100.0f;
        health.currentValue = 100.0f;
    }

    // Use this for initialization
    void Start () {
        playerAnimator = GetComponent<Animator>();
        spawner = GetComponent<EnemyGenerator>();
        audio = GetComponent<AudioSource>();
        car = GetComponent<CarControls>();
    }
	
	// Update is called once per frame
	void Update () {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundOverlapRadius, groundCheckLayerMask);

        if(health.currentValue <= 0 && isAlive) {
            Debug.Log("Player died!");
            die();
        }

        // Only move when alive
        if (gameState.getState() == GameState.GameStates.Running) {
            handleControls();
        } else if (gameState.getState() == GameState.GameStates.GameOver) {
            handleRestartExit();
        }
    }

    void FixedUpdate() {
        
    }

    void handleRestartExit() {
        if(Input.GetButtonDown("Confirm")) {
            print("Starting new game");
            startNewGame();
        } else if(Input.GetButtonDown("Exit")) {
            print("Exiting game");
            Application.Quit();
        }
    }

    void startNewGame() {
        gameState.newEvent(GameState.GameEvents.GameStart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void exitGame() {

    }

    void handleControls() {
        car.handleWeapons();
        car.doMovement();
        car.handleMoveStates();
    }

    public void reduceHealth(float reduceHP) {
        audio.Play();
        health.reduceValue(reduceHP);
    }
    

    void die() {
        gameOverText.gameObject.SetActive(true);
        isAlive = false;
        //playerAnimator.SetBool("isRunning", false);
        //Rigidbody2D rigidBody = transform.GetComponent<Rigidbody2D>();
        //rigidBody.simulated = false;
        spawner.stopAll();
        gameState.newEvent(GameState.GameEvents.GameEnd);
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
