﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHandler : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 50f;

    [SerializeField]
    private float bulletSpeed = 100.0f;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletLifeTime = 3.0f;

    [SerializeField]
    private GameObject player;

    private Vector2 direction;

    private BoxCollider2D playerCollider;

	// Use this for initialization
	void Start () {
        playerCollider = player.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale > 0.0f) {
            TurnCannon();
        }
	}

    public void Fire() {
        GameObject newBullet = (GameObject)Instantiate(bullet);

        Transform origin = transform.Find("BulletOrigin");
        newBullet.transform.position = origin.position;
        Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
        
        foreach (CircleCollider2D coll in newBullet.GetComponents<CircleCollider2D>()) {
            if(!coll.isTrigger) {
                Physics2D.IgnoreCollision(coll, playerCollider);
                print(Physics2D.GetIgnoreCollision(coll, playerCollider));
            }
        }

        newBulletBody.AddForce(transform.up * bulletSpeed);

        Destroy(newBullet, bulletLifeTime);
    }

    private void TurnCannon() {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //direction.x = Input.GetAxisRaw("CannonX");
        //direction.y = Input.GetAxisRaw("CannonY");
        
        
        if(direction.x == 0.0 && direction.y == 0.0) {
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;  // -90 due to sprite offset
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }
}
