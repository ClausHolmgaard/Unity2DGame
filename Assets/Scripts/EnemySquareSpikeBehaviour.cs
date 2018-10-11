using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquareSpikeBehaviour : MonoBehaviour {

    // Target to follow, it is assumed this will be the player
    public GameObject targetObject;

    [SerializeField]
    private int worthPoints = 10;

    [SerializeField]
    private float damage = 25.0f;

    [SerializeField]
    private float pushStrength = 500.0f;

    [SerializeField]
    private float moveSpeed = 2.0f;

    [SerializeField]
    private float maxHP = 3.0f;

    private float bulletHitTime = 1.0f; // How long before same bullet can hit again

    private PlayerController player;
    private Rigidbody2D playerRigidBody;
    private Animator animator;
    private AudioSource audio;

    private Stat health;
    private bool isAlive = true;

    Rigidbody2D thisRigidBody;

    Dictionary<int, float> recentlyHitBy;

    // Use this for initialization
    void Start () {
        health = new Stat();
        health.maxValue = maxHP;
        health.currentValue = maxHP;

        player = targetObject.GetComponent<PlayerController>();
        playerRigidBody = targetObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        thisRigidBody = transform.GetComponent<Rigidbody2D>();
        recentlyHitBy = new Dictionary<int, float>();
	}
	
	// Update is called once per frame
	void Update () {

        if(health.currentValue <= 0 && isAlive) {
            die();
        }

        // Only move when playe is alive
        if(!player.isAlive) {
            return;
        }

        moveToPlayer();

        animator.SetBool("isAlive", isAlive);
	}

    void moveToPlayer() {
        Vector3 targetPosition = targetObject.transform.position;
        Vector3 direction = targetPosition - transform.position;
        direction = direction / direction.magnitude;
        //transform.position += direction * Time.deltaTime * moveSpeed;
        thisRigidBody.AddForce(direction * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!isAlive) {
            return;
        }
        removeOldHits();

        if(collision.gameObject.CompareTag("Bullet")) {
            int hash = collision.gameObject.GetHashCode();
            if (!recentlyHitBy.ContainsKey(hash)) {
                WeaponStat weapon = collision.gameObject.GetComponent<WeaponStat>();
                health.reduceValue(weapon.damage);
                recentlyHitBy.Add(hash, Time.time);
                animator.SetTrigger("triggerDamage");
            }
        }

        if (collision.gameObject.CompareTag("Player")) {
            if (player.isAlive) {
                //Vector2 enemyPosition = enemyCollider.transform.position;
                Vector2 playerDirection = (Vector2)transform.position - (Vector2)player.transform.position;
                Vector2 direction = playerDirection / playerDirection.magnitude;
                playerRigidBody.AddForce(-direction * pushStrength);

                player.reduceHealth(damage);
            }
        }
    }

    private void die() {
        isAlive = false;
        player.addPoints(worthPoints);
        transform.GetComponent<Rigidbody2D>().simulated = false;
        Destroy(transform.gameObject, 0.2f);
        animator.SetTrigger("triggerDie");
        audio.Play();
    }

    void removeOldHits() {
        List<int> removeList = new List<int>();

        foreach (KeyValuePair<int, float> pair in recentlyHitBy) {
            if(Time.time - pair.Value > bulletHitTime) {
                removeList.Add(pair.Key);
            }
        }

        foreach (var id in removeList) {
            recentlyHitBy.Remove(id);
        }
    }
}
