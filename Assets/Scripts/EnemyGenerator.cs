using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject squareSpikeEnemy;

    private float minSpawnAhead = -10.0f;
    private float minSpawnHeight = 1.5f;
    private float maxSpawnAhead = 30.0f;
    private float maxSpawnHeight = 8.0f;
    private float spawnRate = 1.0f;

    private GameObject enemy;

    private List<IEnumerator> coroutines = new List<IEnumerator>();

    // Use this for initialization
    void Start () {
        coroutines.Add(SpawnSquareSpikeEnemies());

        startAll();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void stopAll() {
        //StopCoroutine(SpawnSquareSpikeEnemies());
        foreach (var cr in coroutines) {
            StopCoroutine(cr);
        }
    }

    public void startAll() {
        foreach (var cr in coroutines) {
            StartCoroutine(cr);
        }
    }

    void SpawnSquareSpike() {

        float spawnX = Random.Range(transform.position.x + minSpawnAhead, transform.position.x + maxSpawnAhead);
        float spawnY = Random.Range(transform.position.y + minSpawnHeight, transform.position.y + maxSpawnHeight);
        Vector2 enemyPos = new Vector2(spawnX, spawnY);


        //Vector2 enemyPos = new Vector2(transform.position.x + minSpawnAhead, transform.position.y + maxSpawnHeight);

        GameObject enemy = (GameObject)Instantiate(squareSpikeEnemy);
        EnemySquareSpikeBehaviour enemyBehaviour = enemy.GetComponent<EnemySquareSpikeBehaviour>();
        enemyBehaviour.targetObject = transform.gameObject;
        enemy.transform.position = enemyPos;
    }

    public bool setSpawnRate(float rate) {
        if(rate <= 0) {
            return false;
        }

        spawnRate = rate;

        return true;
    }

    private IEnumerator SpawnSquareSpikeEnemies() {
        while (true) {
            SpawnSquareSpike();
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
