using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquareSpikeBehaviour : MonoBehaviour {

    // Target to follow, it is assumed this will be the player
    public GameObject targetObject;

    private float bulletHitTime = 1.0f; // How long before same bullet can hit again

    private PlayerController player;
    private Stat health;

    Dictionary<int, float> recentlyHitBy;

    // Use this for initialization
    void Start () {
        health = new Stat();
        health.maxValue = 5.0f;
        health.currentValue = 5.0f;

        player = targetObject.GetComponent<PlayerController>();
        recentlyHitBy = new Dictionary<int, float>();
	}
	
	// Update is called once per frame
	void Update () {

        if(health.currentValue <= 0) {
            die();
        }

        // Only move when playe is alive
        if(!player.isAlive) {
            return;
        }

        moveToPlayer();
	}

    void moveToPlayer() {
        Vector3 targetPosition = targetObject.transform.position;
        Vector3 direction = targetPosition - transform.position;
        direction = direction / direction.magnitude;
        transform.position += direction * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        removeOldHits();

        if(collision.gameObject.CompareTag("Bullet")) {
            int hash = collision.gameObject.GetHashCode();
            if (!recentlyHitBy.ContainsKey(hash)) {
                print("OW!");
                WeaponStat weapon = collision.gameObject.GetComponent<WeaponStat>();
                health.reduceValue(weapon.damage);
                recentlyHitBy.Add(hash, Time.time);
            }
        }
    }

    private void die() {
        Destroy(transform.gameObject);
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
