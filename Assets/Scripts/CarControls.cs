using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControls : MonoBehaviour, IControls {

    [SerializeField]
    private float jumpForce = 50f;

    [SerializeField]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private float groundDrag = 1.0f;

    [SerializeField]
    private GameObject cannon;

    private CannonHandler cannonHandler;
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer cannonSprite;
    private BoxCollider2D playerCollider;

    private float xScale;
    private float groundScale = 4.0f;
    private float groundGravScale = 1.0f;

    private bool isRunning = false;
    private bool isGrounded = true;

    private void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        cannonHandler = cannon.GetComponent<CannonHandler>();
        cannonSprite = cannon.GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    public void handleWeapons() {
        if (Input.GetButtonDown("Fire1")) {
            cannonHandler.Fire();
        }
    }

    public void setGrounded(bool grounded) {
        isGrounded = grounded;
    }

    public void handleMoveStates() {

        // Moving on x axis
        if (Input.GetAxisRaw("Horizontal") != 0) {
            if (!isRunning) {
                playerAnimator.SetTrigger("triggerRun");
            }
            isRunning = true;
        } else {
            isRunning = false;
        }

        xScale = Mathf.Abs(transform.localScale.x);
        // Flip character to face correct direction
        if (Input.GetAxisRaw("Horizontal") < 0) {
            transform.localScale = new Vector2(-xScale, transform.localScale.y);
        } else if (Input.GetAxisRaw("Horizontal") > 0) {
            transform.localScale = new Vector2(xScale, transform.localScale.y);
        }

        playerAnimator.SetBool("isOnGround", isGrounded);
        playerAnimator.SetBool("isRunning", isRunning);
    }

    public void doMovement() {

        // Only jump when on ground
        if (Input.GetButtonDown("Jump") && isGrounded) {
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAnimator.SetTrigger("triggerJump");
        }

        // Horizontal movement
        playerRigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0));
    }

    public void die() {
        playerAnimator.SetBool("isRunning", false);
        playerRigidbody.simulated = false;
    }

    public void disable() {
        cannonSprite.enabled = false;
    }

    public void enable() {
        cannonSprite.enabled = true;

        playerRigidbody.gravityScale = groundGravScale;
        playerRigidbody.drag = groundDrag;

        // ugly transforms, due to different sprite sizes
        setScale(groundScale);

        Vector2 boxSize = new Vector2(0.4f, 0.15f);
        playerCollider.size = boxSize;

        Vector2 boxOffset = new Vector2(0.0f, -0.02f);
        playerCollider.offset = boxOffset;
    }

    void setScale(float scaleFactor) {
        Vector2 scale = new Vector2(scaleFactor, scaleFactor);
        transform.localScale = scale;
    }
}
