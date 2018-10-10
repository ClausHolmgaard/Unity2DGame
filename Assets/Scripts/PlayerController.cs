using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float jumpForce = 50f;

    [SerializeField]
    private float moveSpeed = 5f;

    private float groundOverlapRadius = 0.1f;
    private bool isGrounded;
    private bool isRunning;
    private bool isAlive = true;

    private Rigidbody2D playerRigidbody;
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private Animator playerAnimator;

    private Vector3 position;

    // Use this for initialization
    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if(isAlive) {
            handleControls();
        }
    }

    void FixedUpdate() {
        
    }

    void handleControls() {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundOverlapRadius, groundCheckLayerMask);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButton("Horizontal")) {
            if (!isRunning) {
                playerAnimator.SetTrigger("triggerRun");
            }
            position = transform.position;
            position.x += Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
            transform.position = position;

            if (Input.GetAxisRaw("Horizontal") < 0) {
                transform.localScale = new Vector2(-1, transform.localScale.y);
            } else {
                transform.localScale = new Vector2(1, transform.localScale.y);
            }

            isRunning = true;
        } else {
            isRunning = false;
        }

        if (!isGrounded) {
            playerAnimator.SetTrigger("triggerJump");
        } else {
            playerAnimator.SetTrigger("triggerGround");
        }

        playerAnimator.SetBool("isRunning", isRunning);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        HitByEnemy(collider);
    }

    void HitByEnemy(Collider2D laserCollider) {
        if(isAlive) {
            isAlive = false;
            playerAnimator.SetBool("isRunning", false);
            Rigidbody2D rigidBody = transform.GetComponent<Rigidbody2D>();
            rigidBody.simulated = false;
        }
    }
}
