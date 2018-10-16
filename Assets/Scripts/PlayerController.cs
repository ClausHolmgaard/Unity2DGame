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
    private AudioSource playerAudio;
    private SpawnHandler spawner;
    private SpriteRenderer playerRenderer;
    private Animator playerAnimator;
    private BoxCollider2D playerCollider;
    private Rigidbody2D playerRigidBody;

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
        playerAudio = GetComponent<AudioSource>();
        car = GetComponent<CarControls>();
        fly = GetComponent<FlyControls>();
        gameState = GetComponent<GameState>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();

        // Start on ground
        vehicleState = VehicleStateEnum.Ground;
        currentControls = car;

        //playerAnimator.runtimeAnimatorController = groundAnimator;
    }
	
	// Update is called once per frame
	void Update () {

        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        if (health.currentValue <= 0 && isAlive) {
            Debug.Log("Player died!");
            die();
        }

        if (vehicleState == VehicleStateEnum.Ground) {
            currentControls = car;
        } else if (vehicleState == VehicleStateEnum.Fly) {
            currentControls = fly;
        }
    }

    void FixedUpdate() {
        if (gameState.getState() == GameState.GameStateEnum.Running) {
            handleControls();
        }
    }
    
    public void restart() {
        health.maxValue = 100.0f;
        health.currentValue = 100.0f;
        isAlive = true;
    }

    void handleControls() {
        
        if(Input.GetButtonDown("Transform")) {
            doTransform();
        }

        currentControls.setGrounded(isGrounded);
        currentControls.handleWeapons();
        currentControls.doMovement();
        currentControls.handleMoveStates();
    }

    void doTransform() {
        if(vehicleState == VehicleStateEnum.Ground) {
            transformToFly();
        } else {
            transformToCar();
        }
    }

    void transformToCar() {
        print("Driving!");
        car.enable();
        fly.disable();
        vehicleState = VehicleStateEnum.Ground;

        playerAnimator.SetBool("isGroundVehicle", true);

        

        currentControls = car;
    }

    void transformToFly() {
        print("Flying!");
        car.disable();
        fly.enable();
        vehicleState = VehicleStateEnum.Fly;
        playerAnimator.SetBool("isGroundVehicle", false);
    
        currentControls = fly;
    }

    public void reduceHealth(float reduceHP) {
        playerAudio.Play();
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
        Color oldColor = playerRenderer.color;
        playerRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        playerRenderer.color = oldColor;
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
