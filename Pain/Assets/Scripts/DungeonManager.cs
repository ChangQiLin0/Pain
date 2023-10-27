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
    public bool hasUnconnectedRooms; // if a room has a door not connected to any other rooms

    public GameObject[] spawnRooms; // store all rooms which are spawn rooms
    public GameObject[] roomL; // rooms with left doors 
    public GameObject[] roomR; // rooms with right doors 
    public GameObject[] roomU; // rooms with doors going up
    public GameObject[] roomD; // rooms with doors going down

    public string[,] occupiedGrid = new string[9,9]; // 2D array -9x9 grid which shows which cells have a room/type of room e.g. LR (left+right doors)
    // if contains "Req" + "L/R/U/D" it means its requires that

    public void Start()
    {
        GameObject room = Instantiate(tilePrefab, new Vector2(4*26, 4*26), quaternion.identity); // create spawn at 4,4 which is at (104,104)
        occupiedGrid[4,4] = "LRUD"; // indicate new spawned room has doors facing in all directions
        room.transform.SetParent(tilemap.transform);

        occupiedGrid[3,4] = "RoomL"; // [temp for testing] indicate there is a room at 2,4 with a right door open

        room.GetComponent<DungeonRoom>().gridPos = new Vector3Int(4,4); // set location on grid
        room.GetComponent<DungeonRoom>().CreateNextRoom(); // call create room method

        // create spawn room at centre
        // append occupiedgrid at centre (4,4)
    }
}
