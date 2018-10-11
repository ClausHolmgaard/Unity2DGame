using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject squareSpikeEnemy;

    private float minSpawnAhead = 2.0f;
    private float minSpawnHeight = 0.0f;
    private float maxSpawnAhead = 10.0f;
    private float maxSpawnHeight = 4.0f;
    private float spawnRate = 1.0f;


    // Use this for initialization
    void Start () {
        SpawnSquareSpike();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnSquareSpike() {
        

        Vector2 downLeft = new Vector2(minSpawnAhead, minSpawnHeight);
        Vector2 upLeft = new Vector2(minSpawnAhead, maxSpawnHeight);
        Vector2 downRight = new Vector2(maxSpawnAhead, minSpawnHeight);
        Vector2 upRight = new Vector2(maxSpawnAhead, maxSpawnHeight);

        //enemy.transform.position

        //enemy.transform.position = new Vector3(roomCenter, background_y, 0);
    }

    void spawn(GameObject enemy, Transform position) {
        GameObject enemyInstance = (GameObject)Instantiate(enemy);
        EnemySquareSpikeBehaviour enemyBehaviour = enemyInstance.GetComponent<EnemySquareSpikeBehaviour>();
        enemyBehaviour.targetObject = transform.gameObject;
    }
}
