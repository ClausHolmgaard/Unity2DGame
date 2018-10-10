using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquareSpikeBehaviour : MonoBehaviour {

    public GameObject targetObject;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPosition = targetObject.transform.position;
        Vector3 direction = targetPosition - transform.position;
        direction = direction / direction.magnitude;
        transform.position += direction * Time.deltaTime;
	}
}
