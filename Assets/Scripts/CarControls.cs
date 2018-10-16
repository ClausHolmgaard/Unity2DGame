using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControls : MonoBehaviour, IControls {

    [SerializeField]
    private float jumpForce = 50f;

    [SerializeField]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private GameObject cannon;

    private CannonHandler cannonHandler;
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;

    private float xScale;

    private bool isRunning = false;
    private bool isGrounded = true;

    private void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        cannonHandler = cannon.GetComponent<CannonHandler>();

        xScale = Mathf.Abs(playerRigidbody.transform.localScale.x);
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

    public void doMovement() {

        // Only jump when on ground
        if (Input.GetButtonDown("Jump") && isGrounded) {
            print("Jumping!");
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAnimator.SetTrigger("triggerJump");
        }

        // Horizontal movement
        playerRigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0));
    }

    public void die() {
        playerAnimator.SetBool("isRunning", false);
        playerRigidbody.simulated = false;
    }
}
