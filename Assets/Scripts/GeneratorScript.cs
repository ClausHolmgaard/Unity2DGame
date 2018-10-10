using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour {

    [SerializeField]
    private GameObject LifeObject;

    [SerializeField]
    private GameObject[] availableRooms;

    [SerializeField]
    private List<GameObject> currentRooms;

    [SerializeField]
    private int maxNumberOfRooms = 3;

    [SerializeField]
    private GameObject[] availableEnemeies;

    private float screenWidthInPoints;

    // Use this for initialization
    void Start () {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;

        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    void Update () {
		
	}

    void initLife() {
        GameObject life = (GameObject)Instantiate(LifeObject);
    }

    void AddRoom(float farthestRoomEndX) {
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);
        float roomWidth = room.transform.Find("Floor").localScale.x;
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        float background_y = -room.transform.Find("Background").localPosition.y;
        room.transform.position = new Vector3(roomCenter, background_y, 0);

        int randomEnemyIndex = Random.Range(0, availableEnemeies.Length);
        GameObject enemy = (GameObject)Instantiate(availableEnemeies[randomEnemyIndex]);
        EnemySquareSpikeBehaviour enemyBehaviour = enemy.GetComponent<EnemySquareSpikeBehaviour>();
        enemyBehaviour.targetObject = transform.gameObject;
        enemy.transform.position = new Vector3(roomCenter, background_y, 0);

        currentRooms.Insert(0, room);
    }

    private void GenerateRoomIfRequired() {
        List<GameObject> roomsToRemove = new List<GameObject>();
        bool addRooms = true;
        float playerX = transform.position.x;
        float addRoomX = playerX + screenWidthInPoints;
        float farthestRoomEndX = 0;
        int roomCounter = 0;
        foreach (var room in currentRooms) {
            roomCounter++;
            float roomWidth = room.transform.Find("Floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;
            if (roomStartX > addRoomX) {
                addRooms = false;
            }

            if (roomCounter > maxNumberOfRooms) {
                roomsToRemove.Add(room);
            }
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }
        foreach (var room in roomsToRemove) {
            currentRooms.Remove(room);
            Destroy(room);
        }
        if (addRooms) {
            AddRoom(farthestRoomEndX);
        }

        if(currentRooms.Count >= maxNumberOfRooms) {
            GameObject currentEdge = currentRooms[currentRooms.Count - 2].transform.Find("LeftEdge").gameObject;
            currentEdge.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
        
    private IEnumerator GeneratorCheck() {
        while (true) {
            GenerateRoomIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
