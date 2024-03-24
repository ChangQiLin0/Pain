using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private Transform dungeonRoom;
    private Transform dungeonManager;
    private Transform playerDetectorContainer;
    private DungeonRoom dungeonRoomScript; 
    private DungeonManager dungeonManagerScript;
    private Player player;

    private void Start()
    {
        playerDetectorContainer = transform.parent; // get container which holds all player detectors
        dungeonRoom = transform.parent.parent; // get dungeon room parent object
        dungeonManager = dungeonRoom.parent; // get dungeon room manager object
        dungeonRoomScript = dungeonRoom.GetComponent<DungeonRoom>(); // get rooms dungeon room script
        dungeonManagerScript = dungeonManager.GetComponent<DungeonManager>(); // add one to total
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // define player object
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (dungeonRoomScript.spawnedEnemies)
        {
            playerDetectorContainer.gameObject.SetActive(false); // hide all detectors
        }
        else if (collision.gameObject.CompareTag("Player")) // filters out other collisions to only get player
        {   
            dungeonManagerScript.totalVisitedRooms ++;
            EnemySpawnLocation();
        }
    }

    private void EnemySpawnLocation()
    {
        playerDetectorContainer.gameObject.SetActive(false); // hide all detectors
        dungeonRoomScript.CloseDoors("LRUD"); // close all doors, to trap player in room
        dungeonRoomScript.closeDoors = true;
        
        for (int i = 0; i < Random.Range(2, dungeonManagerScript.floor*6); i++) // random loop between 2 and (5*current floor)
        {
            dungeonRoomScript.enemyCount += 1; // increment enemy count by one for each enemy spawned
            Invoke("InstantiateEnemy", 0.5f); // invoke method with time delay
        }
        dungeonRoomScript.spawnedEnemies = true; // place last to prevent dungeon room triggering as count starts at 0
        player.inActiveRoom = true;
    }

    private void InstantiateEnemy()
    {
        Transform spawnLocation = dungeonRoom.Find("SpawnLocationContainer"); // get all possible spawn locations for each room
        Vector3 randomChildPos = spawnLocation.GetChild(Random.Range(0, spawnLocation.childCount)).position; // get random child of spawn location
        Vector3 spawnPos = new Vector3(randomChildPos.x + Random.Range(-1f, 1f), randomChildPos.y + Random.Range(-1f, 1f), 0);

        GameObject enemySpawned = Instantiate(ObtainDefinitions.Instance.enemies[0], spawnPos, Quaternion.identity);
        enemySpawned.transform.SetParent(dungeonRoom);

        Enemy enemyComponent = enemySpawned.GetComponent<Enemy>();

        float healthExponent = 1+((dungeonManagerScript.floor/5f) - 0.2f); // calculate exponent based on floors base exponent is 1, increases by 0.2 per floor
        float damageExponent = 1+((dungeonManagerScript.floor/10f) - 0.1f); // calculate exponent based on floors base exponent is 1, increases by 0.1 per floor
        enemyComponent.enemyHealth = Mathf.Pow(5 + (4 * dungeonManagerScript.totalVisitedRooms), healthExponent); // rounds down to nearest int
        enemyComponent.enemyDamage = Mathf.Pow(4 * dungeonManagerScript.totalVisitedRooms, damageExponent); // rounds down to nearest int


        // TEMP REMOVE LATER AND REPLACE WITH RANDOM SELECTED ENEMY FROM LIST
        if (Random.Range(0,4) == 0) // 25% chance
        {
            enemySpawned.GetComponent<Enemy>().hasShotgun = true;
        }
    }
}
