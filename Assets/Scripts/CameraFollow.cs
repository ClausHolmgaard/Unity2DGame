using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private float offset = 0.0f;

    private float distanceToTarget;

    // Use this for initialization
    void Start () {
        distanceToTarget = transform.position.x - targetObject.transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        float targetObjectX = targetObject.transform.position.x;
        //float targetObjectY = targetObject.transform.position.y;
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectX + distanceToTarget + offset;
        //newCameraPosition.y = targetObjectY;
        transform.position = newCameraPosition;

    }
}
