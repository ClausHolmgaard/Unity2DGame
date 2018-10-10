using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour {

    [SerializeField]
    private Image content;

    [SerializeField]
    private float lerpSpeed = 2.0f;

    [HideInInspector]
    public float maxValue = 0.0f;
    private float minValue = 0.0f;
    private float fillAmount = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, lerpSpeed * Time.deltaTime);
	}

    public void setValue(float value) {
        // Make sure maxvalue is initialized
        if(maxValue == 0.0) {
            Debug.Log("Attempting to set bar value without setting bar max value.");
            return;
        }

        if(value > maxValue) {
            return;
        }

        if(value < minValue) {
            return;
        }

        fillAmount = value / maxValue;
    }
}
