using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DungeonManager : MonoBehaviour
{
    public GameObject tilemapSave; // save original tilemap used to go to next level or reset room
    public Tilemap tilemap; // tile grid everything will be based off of
    public Tile[] openDoorTiles; // store all open door tiles
    public Tile[] closedDoorTiles; // store all closed door tiles
    public GameObject[] allRooms; // store all room prefabs
    public List<GameObject> roomU = new List<GameObject>(); // store all room prefabs with Up facing door
    public List<GameObject> roomD = new List<GameObject>(); // store all room prefabs with Down facing door
    public List<GameObject> roomL = new List<GameObject>(); // store all room prefabs with Left facing door
    public List<GameObject> roomR = new List<GameObject>(); // store all room prefabs with Right facing Door
    public GameObject[] bricks; // place holder room for errors/unintended results

    public int floor = 1; // which floor the player is on, starts at 1
    public int totalVisitedRooms = 0; // total number of rooms player has gone to
    private static int dungeonSize = 9; // static variable, always the same

    public string[,] occupiedGrid = new string[dungeonSize, dungeonSize]; // 2D array -21x21 grid which shows which cells have a room/type of room e.g. LR (left+right doors)
    public GameObject[,] occupiedGridGameObject = new GameObject[dungeonSize, dungeonSize]; // store gameobject of what room is there 
    // if contains "Req" + "L/R/U/D" it means its requires that

    public void Start()
    {
        SortRoomPrefabs();
        tilemapSave = Instantiate(tilemap.gameObject);
        tilemapSave.SetActive(false);

        InstantiateDungeon(); // call instantiateDungeon method
        if (transform.childCount < 30)
        {
            tilemapSave.SetActive(true);
            Destroy(transform.gameObject); // destory self
        }
    }

    private void SortRoomPrefabs() // organise all room prefabs
    {
        if (roomU.Count == 0)
        {
            foreach (GameObject room in allRooms)
            {
                if (room.name.Contains("U"))
                {
                    roomU.Add(room);
                }
                if (room.name.Contains("D"))
                {
                    roomD.Add(room);
                }
                if (room.name.Contains("L"))
                {
                    roomL.Add(room);
                }
                if (room.name.Contains("R"))
                {
                    roomR.Add(room);
                }
            }
        }
    }

    private void InstantiateDungeon() // instantiates dungeon
    {
        for (int i = 0; i < dungeonSize; i++) // loop from 0-14 inclusive
        {
            occupiedGrid[i, 0] = "border"; // bottom horizontal
            occupiedGrid[0, i] = "border"; // left vertical 
            occupiedGrid[i, dungeonSize-1] = "border"; // top horizontal
            occupiedGrid[dungeonSize-1, i] = "border"; // right vertical
        }

        GameObject room = Instantiate(roomL[0], new Vector2((dungeonSize+1)/2*26, (dungeonSize+1)/2*26), quaternion.identity); // create spawn at centre
        occupiedGrid[(dungeonSize+1)/2,(dungeonSize+1)/2] = "L"; // indicate new spawned room has doors facing in all directions
        occupiedGrid[((dungeonSize+1)/2)-1,(dungeonSize+1)/2] = "roomR";

        room.GetComponent<DungeonRoom>().spawnedEnemies = true;
        occupiedGridGameObject[(dungeonSize+1)/2, (dungeonSize+1)/2] = transform.gameObject;

        room.transform.SetParent(tilemap.transform);
        room.GetComponent<DungeonRoom>().gridPos = new Vector3Int((dungeonSize+1)/2, (dungeonSize+1)/2); // set location on grid
        room.GetComponent<DungeonRoom>().CreateNextRoom(); // call create room method 
    }
}
