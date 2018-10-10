using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquareSpikeBehaviour : MonoBehaviour {

    // Target to follow, it is assumed this will be the player
    public GameObject targetObject;

    private PlayerController player;

    // Use this for initialization
    void Start () {
        player = targetObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
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
}
