using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour {

    [SerializeField]
    private GameObject squareSpikeEnemy;

    [SerializeField]
    private GameObject heart;

    [SerializeField]
    private GameObject platform;

    private float globalSpawnModifier = 1.0f;
    private float minSpawnX = -6.0f;
    private float platformOffset = 0.2f;
    private float lastHeartPlayerX = 0.0f;
    private float playerMaxX = 0.0f;
    private float minSpawnFromPlayer = 5.0f;

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

    SpawnSettings squareSpikeSettings = new SpawnSettings(-10.0f, 30.0f, 1.5f, 8.0f, 1.0f);
    SpawnSettings heartSettings = new SpawnSettings(1.0f, 30.0f, 1.5f, 8.0f, 10.0f); 

    private GameObject enemySpawn;
    private GameObject heartSpawn;
    private GameObject grassPlatform;
    private List<GameObject> allHearts = new List<GameObject>();
    private BoxCollider2D playerCollider;

    private List<IEnumerator> spawners = new List<IEnumerator>();

    // Use this for initialization
    void Start() {

        playerCollider = transform.GetComponent<BoxCollider2D>();

        spawners.Add(SpawnSquareSpikeEnemies());
        spawners.Add(SpawnHearts());

        startAll();
    }

    // Update is called once per frame
    void Update() {
        if(transform.position.x > playerMaxX) {
            playerMaxX = transform.position.x;
        }
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
        if (spawnX < minSpawnX) {
            spawnX = minSpawnX;
        }
        Vector2 pos = new Vector2(spawnX, spawnY);

        if (Vector2.Distance(pos, transform.position) < minSpawnFromPlayer) {
            if(pos.x < transform.position.x) {
                pos.x -= minSpawnFromPlayer;
            } else {
                pos.x += minSpawnFromPlayer;
            }
        }

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
        // Only spawn hearts if player moves forward
        if (!(playerMaxX > lastHeartPlayerX + (heartSettings.minSpawnAhead + heartSettings.maxSpawnAhead) / 2)) {
            return;
        }

        heartSpawn = Spawn(heart, heartSettings);
        lastHeartPlayerX = transform.position.x;

        foreach (BoxCollider2D coll in heartSpawn.GetComponents<BoxCollider2D>()) {
            if (!coll.isTrigger) {
                Physics2D.IgnoreCollision(coll, playerCollider);
            }
        }

        Vector2 pos = new Vector2(heartSpawn.transform.position.x, heartSpawn.transform.position.y - platformOffset);
        GameObject platformSpawn = (GameObject)Instantiate(platform);
        platformSpawn.transform.position = pos;

        allHearts.Add(heartSpawn);
    }

    public bool setSpawnRate(float rateModifier) {
        if (rateModifier <= 0) {
            return false;
        }
        
        globalSpawnModifier = rateModifier;
        print("Square enemy spawn rate: " + squareSpikeSettings.spawnRate * globalSpawnModifier);
        return true;
    }

    private IEnumerator SpawnSquareSpikeEnemies() {
        while (true) {
            SpawnSquareSpike();
            yield return new WaitForSeconds(squareSpikeSettings.spawnRate * globalSpawnModifier);
        }
    }

    private IEnumerator SpawnHearts() {
        while(true) {
            SpawnHeart();
            yield return new WaitForSeconds(heartSettings.spawnRate);
        }
    }

    public List<GameObject> GetAllHearts() {

        List<GameObject> removeHearts = new List<GameObject>();

        foreach (GameObject h in allHearts) {
            if (h == null) {
                //allHearts.Remove(h);
                removeHearts.Add(h);
            }
        }

        foreach (GameObject g in removeHearts) {
            allHearts.Remove(g);
        }

        return allHearts;
    }
}
