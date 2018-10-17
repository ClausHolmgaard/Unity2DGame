using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControls : MonoBehaviour, IControls {

    [SerializeField]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private GameObject cannon;

    [SerializeField]
    private float flyDrag = 2.0f;

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private SpriteRenderer cannonSprite;
    private CannonHandler cannonHandler;
    private BoxCollider2D playerCollider;

    private float flyScale = 1.0f;
    private float flyGravScale = 0.0f;

    // Use this for initialization
    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        cannonSprite = cannon.GetComponent<SpriteRenderer>();
        cannonHandler = cannon.GetComponent<CannonHandler>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    public void handleWeapons() {
        if (PersistentInputManager.Instance.isFire()) {
            cannonHandler.Fire();
        }
    }

    public void setGrounded(bool grounded) {
        // ugly thing to get animations to work...
        playerAnimator.SetBool("isOnGround", true);
    }

    public void handleMoveStates() {
        // Not relevant
    }

    public void doMovement() {
        float xScale = Mathf.Abs(transform.localScale.x);
        // Flip character to face correct direction
        if (PersistentInputManager.Instance.getHorizontal() < 0) {
            transform.localScale = new Vector2(-xScale, transform.localScale.y);
        } else if (PersistentInputManager.Instance.getHorizontal() > 0) {
            transform.localScale = new Vector2(xScale, transform.localScale.y);
        }

        //playerRigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed));
        Vector2 pos = transform.position;
        pos.x += PersistentInputManager.Instance.getHorizontal() * moveSpeed * Time.deltaTime;
        pos.y += PersistentInputManager.Instance.getVertical() * moveSpeed * Time.deltaTime;
        transform.position = pos;


    }

    public void die() {
        playerRigidbody.simulated = false;
    }

    public void enable() {
        cannonSprite.enabled = true;

        playerRigidbody.gravityScale = flyGravScale;
        playerRigidbody.drag = flyDrag;

        // ugly transforms, due to different sprite sizes
        setScale(flyScale);

        Vector2 boxSize = new Vector2(1.0f, 0.85f);
        playerCollider.size = boxSize;

        Vector2 boxOffset = new Vector2(0.1f, 0.0f);
        playerCollider.offset = boxOffset;
    }

    public void disable() {
        cannonSprite.enabled = false;
    }

    void setScale(float scaleFactor) {
        Vector2 scale = new Vector2(scaleFactor, scaleFactor);
        transform.localScale = scale;
    }
}
