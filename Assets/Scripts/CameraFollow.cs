using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private GameObject targetObject;

    private float distanceToTarget;

    // Use this for initialization
    void Start () {
        //float pixelToUnits = 100;
        //float cameraSize = (Screen.height / 2) / pixelToUnits;
        //Camera.main.orthographic = true;
        //Camera.main.orthographicSize = cameraSize;

        distanceToTarget = transform.position.x - targetObject.transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        float targetObjectX = targetObject.transform.position.x;
        //float targetObjectY = targetObject.transform.position.y;
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectX + distanceToTarget;
        //newCameraPosition.y = targetObjectY;
        transform.position = newCameraPosition;

    }
}
