using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Stat health;

    [SerializeField]
    private Stat energy;

    [SerializeField]
    private Text pointText;

    [SerializeField]
    private ParticleSystem transformParticles;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private GameObject heartIndicator;

    [SerializeField]
    private float flyingEnergyDrain = 10.0f;

    [SerializeField]
    private float energyRegen = 5.0f;

    [SerializeField]
    private float damageImmunityTime = 0.5f;

    GameState gameState;

    CarControls car;
    FlyControls fly;
    IControls currentControls;

    private float xScale;
    private int currentPoints = 0;
    public bool isAlive = true;
    bool isGrounded = true;
    float animationHideTime = 0.2f;
    float screenWidthInPoints;
    float lastTimeDamageTaken;

    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private AudioSource playerAudio;
    private SpawnHandler spawner;
    private SpriteRenderer playerRenderer;
    private Animator playerAnimator;
    private SpriteRenderer heartIndicatorRenderer;

    private Vector3 position;

    public enum VehicleStateEnum {
        Ground,
        Fly
    }

    private VehicleStateEnum vehicleState = new VehicleStateEnum();

    private void Awake() {
        health.maxValue = 100.0f;
        health.currentValue = 100.0f;

        energy.maxValue = 100.0f;
        energy.currentValue = 100.0f;
    }

    // Use this for initialization
    void Start () {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;

        spawner = GetComponent<SpawnHandler>();
        playerAudio = GetComponent<AudioSource>();
        car = GetComponent<CarControls>();
        fly = GetComponent<FlyControls>();
        gameState = GetComponent<GameState>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        heartIndicatorRenderer = heartIndicator.GetComponent<SpriteRenderer>();

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

        helicopterEnergyHandler();
        IndicateHeart();
    }

    void FixedUpdate() {
        
    }

    void helicopterEnergyHandler() {
        if(vehicleState == VehicleStateEnum.Fly) {
            energy.reduceValue(flyingEnergyDrain * Time.deltaTime);
        } else {
            energy.increaseValue(energyRegen * Time.deltaTime);
        }

        if(energy.currentValue < 0.5f && vehicleState == VehicleStateEnum.Fly) {
            doTransform();
        }
    }

    void IndicateHeart() {
        List<GameObject> hearts = spawner.GetAllHearts();

        if(hearts.Count <= 0) {
            heartIndicatorRenderer.enabled = false;
            return;
        }

        GameObject closestHeart = null;
        float closestDist = 0.0f;
        float dist;
        foreach(GameObject h in hearts) {
            dist = Vector2.Distance(transform.position, h.transform.position);
            if(closestDist == 0.0f) {
                closestDist = dist;
                closestHeart = h;
            } else if(dist < closestDist) {
                closestDist = dist;
                closestHeart = h;
            }
        }

        if (closestHeart != null) {
            if(closestDist > screenWidthInPoints/2) {
                heartIndicatorRenderer.enabled = true;
                Vector2 pos = new Vector2();
                if(closestHeart.transform.position.x < transform.position.x) {
                    pos.x = playerCamera.transform.position.x - screenWidthInPoints / 2;
                } else {
                    pos.x = playerCamera.transform.position.x + screenWidthInPoints / 2;
                }
                pos.y = closestHeart.transform.position.y;
                heartIndicator.transform.position = pos;
            } else {
                heartIndicatorRenderer.enabled = false;
            }
        }

    }
    
    public void restart() {
        health.maxValue = 100.0f;
        health.currentValue = 100.0f;
        isAlive = true;
    }

    void handleControls() {
        
        if(PersistentInputManager.Instance.isTransform()) {
            doTransform();
        }

        currentControls.setGrounded(isGrounded);
        currentControls.handleWeapons();
        currentControls.doMovement();
        currentControls.handleMoveStates();
    }

    void doTransform() {

        StartCoroutine(HideForSeconds());

        transformParticles.Play();
        if(vehicleState == VehicleStateEnum.Ground) {
            transformToFly();
        } else {
            transformToCar();
        }

    }

    IEnumerator HideForSeconds() {
        playerRenderer.enabled = false;
        yield return new WaitForSeconds(animationHideTime);
        playerRenderer.enabled = true;
    }

    void transformToCar() {

        playerAnimator.SetBool("isGroundVehicle", true);
        fly.disable();
        car.enable();
        vehicleState = VehicleStateEnum.Ground;

        currentControls = car;
    }

    void transformToFly() {

        playerAnimator.SetBool("isGroundVehicle", false);
        car.disable();
        fly.enable();
        vehicleState = VehicleStateEnum.Fly;

        currentControls = fly;
    }

    public void reduceHealth(float reduceHP) {

        if (Time.time - lastTimeDamageTaken > damageImmunityTime) {
            playerAudio.Play();
            health.reduceValue(reduceHP);
            StartCoroutine(flashRed());
            lastTimeDamageTaken = Time.time;
        }
    }

    public void increaseHealth(float increaseHP) {
        health.increaseValue(increaseHP);
    }

    public bool isHealthMax() {
        return health.isMax();
    }

    IEnumerator flashRed() {
        Color oldColor = Color.white; // playerRenderer.color;
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
        spawner.setSpawnRate(rate);
    }

}
