using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private Transform dungeonRoom;
    private Transform playerDetectorContainer;

    private void Start()
    {
        playerDetectorContainer = transform.parent; // get container which holds all player detectors
        dungeonRoom = transform.parent.parent; // get dungeon room parent
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) // filters out other collisions
        {
            dungeonRoom.GetComponent<DungeonRoom>().EnterRoom(); // call enterroom method
            Destroy(playerDetectorContainer.gameObject); // destory all player detectors in that single room
        }
    }
}
