using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private Transform dungeonRoom;
    private Transform dungeonManager;
    private Transform playerDetectorContainer;


    private void Start()
    {
        playerDetectorContainer = transform.parent; // get container which holds all player detectors
        dungeonRoom = transform.parent.parent; // get dungeon room parent object
        dungeonManager = dungeonRoom.parent; // get dungeon room manager object
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) // filters out other collisions
        {   
            DungeonRoom dungeonRoomScript = dungeonRoom.GetComponent<DungeonRoom>(); // call enterroom method
            DungeonManager dungeonManagerScript = dungeonManager.GetComponent<DungeonManager>();
            if (dungeonRoomScript.enemyCount == 0)
            {
                dungeonRoomScript.OpenDoors("LRUD"); // open all doors
                if (dungeonRoomScript.doorDirections.Contains("L")) // if room has a left door
                {
                    GameObject leftRoom = dungeonManagerScript.occupiedGridGameObject[dungeonRoomScript.gridPos.x -1, dungeonRoomScript.gridPos.y]; // get room left of current room
                    if (leftRoom.TryGetComponent(out DungeonRoom leftRoomScript))
                    {
                        leftRoomScript.OpenDoors("R"); // open right door
                    }
                }
                if (dungeonRoomScript.doorDirections.Contains("R")) // if room has a right door
                {
                    GameObject rightRoom = dungeonManagerScript.occupiedGridGameObject[dungeonRoomScript.gridPos.x +1, dungeonRoomScript.gridPos.y]; // get room right of current room
                    if (rightRoom.TryGetComponent(out DungeonRoom rightRoomScript))
                    {
                        rightRoomScript.OpenDoors("L"); // open left door
                    }
                }
                if (dungeonRoomScript.doorDirections.Contains("U")) // if room has a top door
                {
                    GameObject aboveRoom = dungeonManagerScript.occupiedGridGameObject[dungeonRoomScript.gridPos.x, dungeonRoomScript.gridPos.y +1]; // get room above of current room
                    if (aboveRoom.TryGetComponent(out DungeonRoom aboveRoomScript))
                    {
                        aboveRoomScript.OpenDoors("D"); // open bottom door
                    }
                }
                if (dungeonRoomScript.doorDirections.Contains("D")) // if room has a bottom door
                {
                    GameObject belowRoom = dungeonManagerScript.occupiedGridGameObject[dungeonRoomScript.gridPos.x, dungeonRoomScript.gridPos.y -1]; // get room below of current room
                    if (belowRoom.TryGetComponent(out DungeonRoom belowRoomScript))
                    {
                        belowRoomScript.OpenDoors("U"); // open top door
                    }
                    
                }
            }
            
            
            

            playerDetectorContainer.gameObject.SetActive(false); // destory all player detectors in that single room
        }
    }
}
