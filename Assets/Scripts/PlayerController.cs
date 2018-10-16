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
    FlyControls fly;
    IControls currentControls;

    private float xScale;
    private int currentPoints = 0;
    public bool isAlive = true;
    bool isGrounded = true;

    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private AudioSource audioS;
    private SpawnHandler spawner;
    private SpriteRenderer thisRenderer;

    private Vector3 position;

    public enum VehicleStateEnum {
        Ground,
        Fly
    }

    private VehicleStateEnum vehicleState = new VehicleStateEnum();

    private void Awake() {
        health.maxValue = 100.0f;
        health.currentValue = 100.0f;
    }

    // Use this for initialization
    void Start () {
        spawner = GetComponent<SpawnHandler>();
        audioS = GetComponent<AudioSource>();
        car = GetComponent<CarControls>();
        fly = GetComponent<FlyControls>();
        gameState = GetComponent<GameState>();
        thisRenderer = GetComponent<SpriteRenderer>();

        // Start on ground
        vehicleState = VehicleStateEnum.Ground;
        currentControls = car;
    }
	
	// Update is called once per frame
	void Update () {

        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        if (health.currentValue <= 0 && isAlive) {
            Debug.Log("Player died!");
            die();
        }

        if (gameState.getState() == GameState.GameStateEnum.Running) {
            handleControls();
        }

        if (vehicleState == VehicleStateEnum.Ground) {
            currentControls = car;
        } else if (vehicleState == VehicleStateEnum.Fly) {
            currentControls = fly;
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
        
        if(Input.GetButtonDown("Transform")) {
            print("Transforming");
        }

        currentControls.setGrounded(isGrounded);
        currentControls.handleWeapons();
        currentControls.doMovement();
        currentControls.handleMoveStates();
    }

    void doTransform() {
        if(vehicleState == VehicleStateEnum.Ground) {
            transforToFly();
        } else {
            transformToCar();
        }
    }

    void transformToCar() {
        vehicleState = VehicleStateEnum.Ground;
    }

    void transforToFly() {
        vehicleState = VehicleStateEnum.Fly;
    }

    public void reduceHealth(float reduceHP) {
        audioS.Play();
        health.reduceValue(reduceHP);
        StartCoroutine(flashRed());
    }

    public void increaseHealth(float increaseHP) {
        health.increaseValue(increaseHP);
        // TODO: Play sound?
        // TODO: Flash green?
    }

    public bool isHealthMax() {
        return health.isMax();
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
