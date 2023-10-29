using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
    public Tilemap tilemap; // tile grid everything will be based off of
    public GameObject tilePrefab;

    public Tile[] openDoorTiles; // store all open door tiles
    public Tile[] closedDoorTiles; // store all closed door tiles
    public int roomsRequired; // number of rooms that should spawn 
    public int roomsSpawned; // number of rooms currently spawned

    public GameObject[] spawnRooms; // store all rooms which are spawn rooms
    public GameObject[] rooms; // store all room prefabs
    public GameObject[] bricks;


    public string[,] occupiedGrid = new string[15,15]; // 2D array -21x21 grid which shows which cells have a room/type of room e.g. LR (left+right doors)
    // if contains "Req" + "L/R/U/D" it means its requires that

    public void Start()
    {

        for (int i = 0; i < 15; i++) // loop from 0-20 inclusive
        {
            occupiedGrid[i, 0] = "boarder"; // bottom horizontal
            occupiedGrid[0, i] = "boarder"; // left vertical 
            occupiedGrid[i, 14] = "boarder"; // top horizontal
            occupiedGrid[14, i] = "boarder"; // right vertical
        }

        GameObject room = Instantiate(tilePrefab, new Vector2(8*26, 8*26), quaternion.identity); // create spawn at 4,4 which is at (104,104)
        occupiedGrid[8,8] = "LRUD"; // indicate new spawned room has doors facing in all directions
        occupiedGrid[7,8] = "roomR";
        occupiedGrid[9,8] = "roomL";
        occupiedGrid[11,7] = "roomU";
        occupiedGrid[11,9] = "roomD";


        room.transform.SetParent(tilemap.transform);

        room.GetComponent<DungeonRoom>().gridPos = new Vector3Int(8,8); // set location on grid
        room.GetComponent<DungeonRoom>().CreateNextRoom(); // call create room method

        // create spawn room at centre
        // append occupiedgrid at centre (4,4)
    }
}
