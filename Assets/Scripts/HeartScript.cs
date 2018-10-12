using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour {

    float scaleSpeed = 3.0f;
    float scaleFactor = 0.5f;

    Vector2 originalScale;
    Vector2 currentScale;

    bool shrinking = true;

	// Use this for initialization
	void Start () {
        originalScale.x = transform.localScale.x;
        originalScale.y = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 ls = transform.localScale;

        if (ls.x >= originalScale.x && ls.y >= originalScale.y) {
            shrinking = true;
        }
        if(ls.x <= originalScale.x * scaleFactor && ls.y <= originalScale.y * scaleFactor) {
            shrinking = false;
        }

        if(shrinking) {
            ls.x = Mathf.Lerp(ls.x, ls.x * scaleFactor, Time.deltaTime * scaleSpeed);
            ls.y = Mathf.Lerp(ls.y, ls.y * scaleFactor, Time.deltaTime * scaleSpeed);
            transform.localScale = ls;
        } else {
            ls.x = Mathf.Lerp(ls.x, ls.x / scaleFactor, Time.deltaTime * scaleSpeed);
            ls.y = Mathf.Lerp(ls.y, ls.y / scaleFactor, Time.deltaTime * scaleSpeed);
            transform.localScale = ls;
        }

	}
}
