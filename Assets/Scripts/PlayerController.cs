using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float jumpForce = 50f;

    [SerializeField]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private Stat health;

    [SerializeField]
    private Text gameOverText;

    private float groundOverlapRadius = 0.1f;
    private bool isGrounded = true;
    private bool isRunning;
    private float xScale;
    public bool isAlive = true;

    private Rigidbody2D playerRigidbody;
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private Animator playerAnimator;

    private Vector3 position;

    private void Awake() {
        health.maxValue = 100.0f;
        health.currentValue = 100.0f;
    }

    // Use this for initialization
    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        xScale = Mathf.Abs(transform.localScale.x);
    }
	
	// Update is called once per frame
	void Update () {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundOverlapRadius, groundCheckLayerMask);

        if(health.currentValue <= 0 && isAlive) {
            Debug.Log("Player died!");
            die();
        }

        // Only move when alive
        if (isAlive) {
            handleControls();
        }
    }

    void FixedUpdate() {
        
    }

    void handleControls() {
        doMovement();
        handleMoveStates();
    }

    void handleMoveStates() {

        // Moving on x axis
        if (Input.GetAxisRaw("Horizontal") != 0) {
            if (!isRunning) {
                playerAnimator.SetTrigger("triggerRun");
            }
            isRunning = true;
        } else {
            isRunning = false;
        }

        // Moving on y axis
        if (Input.GetAxisRaw("Vertical") != 0) {
            // Ready for flying!
        }

        // Flip character to face correct direction
        if (Input.GetAxisRaw("Horizontal") < 0) {
            transform.localScale = new Vector2(-xScale, transform.localScale.y);
        } else if (Input.GetAxisRaw("Horizontal") > 0) {
            transform.localScale = new Vector2(xScale, transform.localScale.y);
        }

        playerAnimator.SetBool("isOnGround", isGrounded);
        playerAnimator.SetBool("isRunning", isRunning);
    }

    void doMovement() {

        // Only jump when on ground
        if (Input.GetButtonDown("Jump") && isGrounded) {
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAnimator.SetTrigger("triggerJump");
        }

        // Horizontal movement
        // TODO: Add vertical for flying
        /* Alternative movement
        position = transform.position;
        position.x += Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        transform.position = position;
        */
        playerRigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0));
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("EnemySquareSpikes")) {
            HitByEnemy(collision);
        }
    }

    void HitByEnemy(Collider2D enemyCollider) {
        if(isAlive) {
            Vector2 enemyPosition = enemyCollider.transform.position;
            Vector2 enemyDirection = enemyPosition - (Vector2)transform.position;
            enemyDirection = enemyDirection / enemyDirection.magnitude;

            playerRigidbody.AddForce(-enemyDirection * 500.0f);
            print(enemyDirection);

            // TODO: Implement value elsewhere
            health.reduceValue(5.0f);
        }
    }

    void die() {
        gameOverText.gameObject.SetActive(true);
        isAlive = false;
        playerAnimator.SetBool("isRunning", false);
        Rigidbody2D rigidBody = transform.GetComponent<Rigidbody2D>();
        rigidBody.simulated = false;
    }
}
