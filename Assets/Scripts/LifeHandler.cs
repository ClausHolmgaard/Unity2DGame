using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHandler : MonoBehaviour {

    private GameObject[] LifeList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void addLife() {

    }

    void addLife(int numLife) {
        for (int i = 0; i < numLife; i++) {
            addLife();
        }
    }

    void removeLife() {

    }
}
