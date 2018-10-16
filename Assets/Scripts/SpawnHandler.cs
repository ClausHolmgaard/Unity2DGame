using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour {

    [SerializeField]
    private GameObject squareSpikeEnemy;

    [SerializeField]
    private GameObject heart;

    private struct SpawnSettings {
        public float minSpawnAhead;
        public float minSpawnHeight;
        public float maxSpawnAhead;
        public float maxSpawnHeight;
        public float spawnRate;

        public SpawnSettings(float minAhead, float maxAhead, float minHeight, float maxHeight, float rate) {
            minSpawnAhead = minAhead;
            maxSpawnAhead = maxAhead;
            minSpawnHeight = minHeight;
            maxSpawnHeight = maxHeight;
            spawnRate = rate;
        }
    }

    SpawnSettings squareSpikeSettings = new SpawnSettings(-5.0f, 30.0f, 1.5f, 8.0f, 4.0f);
    SpawnSettings heartSettings = new SpawnSettings(1.0f, 30.0f, 1.5f, 8.0f, 10.0f); 

    private GameObject enemySpawn;
    private GameObject heartSpawn;


    private List<IEnumerator> spawners = new List<IEnumerator>();

    // Use this for initialization
    void Start() {
        
        spawners.Add(SpawnSquareSpikeEnemies());
        spawners.Add(SpawnHearts());

        startAll();
    }

    // Update is called once per frame
    void Update() {

    }

    public void stopAll() {
        //StopCoroutine(SpawnSquareSpikeEnemies());
        foreach (var cr in spawners) {
            StopCoroutine(cr);
        }
    }

    public void startAll() {
        foreach (var cr in spawners) {
            StartCoroutine(cr);
        }
    }

    GameObject Spawn(GameObject objectToSpawn, SpawnSettings objectSpawnSettings) {
        float spawnX = Random.Range(transform.position.x + objectSpawnSettings.minSpawnAhead, transform.position.x + objectSpawnSettings.maxSpawnAhead);
        float spawnY = Random.Range(transform.position.y + objectSpawnSettings.minSpawnHeight, transform.position.y + objectSpawnSettings.maxSpawnHeight);
        if (spawnX < -6) {
            spawnX = -6;
        }
        Vector2 pos = new Vector2(spawnX, spawnY);

        GameObject spawn = (GameObject)Instantiate(objectToSpawn);
        spawn.transform.position = pos;
        return spawn;
    }

    void SpawnSquareSpike() {
        enemySpawn = Spawn(squareSpikeEnemy, squareSpikeSettings);
        EnemySquareSpikeBehaviour enemyBehaviour = enemySpawn.GetComponent<EnemySquareSpikeBehaviour>();
        enemyBehaviour.targetObject = transform.gameObject;
    }

    void SpawnHeart() {
        heartSpawn = Spawn(heart, heartSettings);
    }

    public bool setSpawnRate(float rate) {
        if (rate <= 0) {
            return false;
        }

        squareSpikeSettings.spawnRate = rate;

        return true;
    }

    private IEnumerator SpawnSquareSpikeEnemies() {
        while (true) {
            SpawnSquareSpike();
            yield return new WaitForSeconds(squareSpikeSettings.spawnRate);
        }
    }

    private IEnumerator SpawnHearts() {
        while(true) {
            SpawnHeart();
            yield return new WaitForSeconds(heartSettings.spawnRate);
        }
    }
}
