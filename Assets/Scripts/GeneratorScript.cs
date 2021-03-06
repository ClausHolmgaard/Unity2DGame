﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour {

    [SerializeField]
    private GameObject[] availableRooms;

    [SerializeField]
    private List<GameObject> currentRooms;

    [SerializeField]
    private GameObject[] availableDecos;

    [SerializeField]
    private int maxNumberOfRooms = 3;

    [SerializeField]
    private int minDecoPerRoom = 0;

    [SerializeField]
    private int maxDecoPerRoom = 4;

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

    void AddRoom(float farthestRoomEndX) {
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);
        float roomWidth = room.transform.Find("Floor").localScale.x;
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        float background_y = -room.transform.Find("Background").localPosition.y;
        room.transform.position = new Vector3(roomCenter, background_y, 0);

        int numDecos = Random.Range(minDecoPerRoom, maxDecoPerRoom + 1);
        int decoIndex;
        float roomStart = roomCenter - roomWidth * 0.5f;
        float roomEnd = roomStart + roomWidth;
        for (int i = 0; i < numDecos; i++) {
            decoIndex = Random.Range(0, availableDecos.Length);
            GameObject deco = (GameObject)Instantiate(availableDecos[decoIndex]);
            float decoX = Random.Range(roomStart, roomEnd);
            Vector2 decoPos = new Vector2(decoX, -5.0f);
            deco.transform.position = decoPos;
        }


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
