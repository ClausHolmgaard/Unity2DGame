using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHandler : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 50f;

    [SerializeField]
    private float bulletSpeed = 100.0f;

    [SerializeField]
    private GameObject bullet;

    private Vector2 direction;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        TurnCannon();

        if(Input.GetButtonDown("Fire1")) {
            Fire();
        }
	}

    private void Fire() {
        GameObject newBullet = (GameObject)Instantiate(bullet);

        Transform origin = transform.Find("BulletOrigin");
        newBullet.transform.position = origin.position;
        Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();

        newBulletBody.AddForce(transform.up * bulletSpeed);

        Destroy(newBullet, 10.0f);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //transform.rotation = rotation;
    }
}
