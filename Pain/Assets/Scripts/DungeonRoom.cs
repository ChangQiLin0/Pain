using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoom : MonoBehaviour
{
    public string doorDirections; // LRUD (Left, Right, Up, Down)
    public Vector3Int gridPos; // store grid pos relative to occupied grid in dungeonManager script
    private Tile[] closedDoorTiles; // store all closed door tiles
    private Tile[] openDoorTiles; // store all open door tiles
    private DungeonManager dungeonManager;

    public bool spawnedEnemies = false; // if room is passive, set true by default in inspector
    public int enemyCount = 0; // number of enemies remaining 
    public bool closeDoors = false;


    private void Start()
    {
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get dungeonmanager script from parent
        openDoorTiles = dungeonManager.openDoorTiles; // get tile asset
        closedDoorTiles = dungeonManager.closedDoorTiles; // get tile asset
    }

    private void Update()
    {
        RoomCleared();
    }

    public void OpenDoors(string openDirection) // openCommand = "U" / "D" up/down
    {
        Tilemap aboveLayer = transform.GetChild(1).GetComponent<Tilemap>(); // get CollisionAbove child 
        Tilemap belowLayer = transform.GetChild(2).GetComponent<Tilemap>(); // get CollisionBelow child 

        BoundsInt bounds = aboveLayer.cellBounds; // gets 26x26 areas coordinates, min and max coords
        Vector3Int bounds1 = bounds.min; // get min point of bounds (bottom left)
        Vector3Int bounds2 = bounds.min;
        bounds1.x += 12;
        bounds2.y += 13;
        for (int i = 0; i < 26; i ++) // loops from 0 to 25 inclusive
        {
            bounds1.y += 1; // increment y position by 1 (move up)
            TileBase aboveTile = aboveLayer.GetTile(bounds1); // retrives tile at pos in above layer
            TileBase belowTile = belowLayer.GetTile(bounds1); // retrives tile at pos in below layer
            if (closedDoorTiles[0] == aboveTile && openDirection.Contains("U")) // if above tile is top left closed door tile
            {
                aboveLayer.SetTile(bounds1, null); // remove top left
                belowLayer.SetTile(bounds1, openDoorTiles[0]); // instantiate top left open door tile
                aboveLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y, 0), null); // top right
                belowLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y, 0), openDoorTiles[1]); // instantiate top left open door tile
                aboveLayer.SetTile(new Vector3Int(bounds1.x, bounds1.y - 1, 0), openDoorTiles[2]); // bottom left
                aboveLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y -1, 0), openDoorTiles[3]); // bottom right
            }
            if (closedDoorTiles[0] == belowTile && openDirection.Contains("D")) // if below tile is top left closed door tile
            {
                belowLayer.SetTile(bounds1, openDoorTiles[0]); // replace top left with new tile
                belowLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y, 0), openDoorTiles[1]); // top right
                belowLayer.SetTile(new Vector3Int(bounds1.x, bounds1.y - 1, 0), null); // bottom left
                aboveLayer.SetTile(new Vector3Int(bounds1.x, bounds1.y - 1, 0), openDoorTiles[2]);
                belowLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y -1, 0), null); // bottom right
                aboveLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y -1, 0), openDoorTiles[3]); // bottom right
            }
            bounds2.x += 1; // increment x position by 1 (move up)
            TileBase aboveTile2 = aboveLayer.GetTile(bounds2); // retrives tile at pos in above layer
            if (closedDoorTiles[4] == aboveTile2 && openDirection.Contains("L")) // if above tile is top left closed door tile
            {
                aboveLayer.SetTile(bounds2, openDoorTiles[4]); // replace closed door tile with open door tile
                aboveLayer.SetTile(new Vector3Int(bounds2.x, bounds2.y - 1, 0), openDoorTiles[5]); // bottom tile
            }
            if (closedDoorTiles[6] == aboveTile2 && openDirection.Contains("R"))
            {
                aboveLayer.SetTile(bounds2, openDoorTiles[6]); // remove top tile
                aboveLayer.SetTile(new Vector3Int(bounds2.x, bounds2.y - 1, 0), openDoorTiles[7]); // bottom tile
            }
        }
    }

    public void CloseDoors(string openDirection) // openCommand = "U" / "D" up/down
    {
        Tilemap aboveLayer = transform.GetChild(1).GetComponent<Tilemap>(); // get CollisionAbove child 
        Tilemap belowLayer = transform.GetChild(2).GetComponent<Tilemap>(); // get CollisionBelow child 

        BoundsInt bounds = aboveLayer.cellBounds; // gets 26x26 areas coordinates, min and max coords
        Vector3Int bounds1 = bounds.min; // get min point of bounds (bottom left)
        Vector3Int bounds2 = bounds.min;
        bounds1.x += 12;
        bounds2.y += 13;
        for (int i = 0; i < 26; i ++) // loops from 0 to 25 inclusive
        {
            bounds1.y += 1; // increment y position by 1 (move up)
            TileBase aboveTile = aboveLayer.GetTile(bounds1); // retrives tile at pos in above layer
            TileBase belowTile = belowLayer.GetTile(bounds1); // retrives tile at pos in below layer
            if (openDoorTiles[0] == belowTile && openDirection.Contains("U")) // if above tile is top left open door tile
            {
                belowLayer.SetTile(bounds1, null); // set previous below tile to nothing
                aboveLayer.SetTile(bounds1, closedDoorTiles[0]); // set new tile above that in above layer
                belowLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y, 0), null); // set top right tile to none
                aboveLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y, 0), closedDoorTiles[1]); // set top right with new tile
                aboveLayer.SetTile(new Vector3Int(bounds1.x, bounds1.y - 1, 0), closedDoorTiles[2]); // replace bottom left tile with new tile
                aboveLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y -1, 0), closedDoorTiles[3]); // replace bottom right tile with new tile
            }
            if (openDoorTiles[0] == aboveTile && openDirection.Contains("D")) // if below contains a open door tile
            {
                belowLayer.SetTile(bounds1, closedDoorTiles[0]); // replace top left tile
                belowLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y, 0), closedDoorTiles[1]); // replace top right tile 
                aboveLayer.SetTile(new Vector3Int(bounds1.x, bounds1.y - 1, 0), null); // set bottom left tile to none
                belowLayer.SetTile(new Vector3Int(bounds1.x, bounds1.y - 1, 0), closedDoorTiles[2]); // replace with open door tile
                aboveLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y -1, 0), null); //set bottom right tile to none
                belowLayer.SetTile(new Vector3Int(bounds1.x + 1, bounds1.y -1, 0), closedDoorTiles[3]); // replace with open door tile
            }
            bounds2.x += 1; // increment x position by 1 (move up)
            TileBase aboveTile2 = aboveLayer.GetTile(bounds2); 
            if (openDoorTiles[4] == aboveTile2 && openDirection.Contains("L")) // if left contains open door tiles
            {
                aboveLayer.SetTile(bounds2, closedDoorTiles[4]); 
                aboveLayer.SetTile(new Vector3Int(bounds2.x, bounds2.y - 1, 0), closedDoorTiles[5]); 
            }
            if (openDoorTiles[6] == aboveTile2 && openDirection.Contains("R")) // if right contains open door tiles
            {
                aboveLayer.SetTile(bounds2, closedDoorTiles[6]); 
                aboveLayer.SetTile(new Vector3Int(bounds2.x, bounds2.y - 1, 0), closedDoorTiles[7]); 
            }
        }
    }

    public void UpdateOccupiedGrid(Vector3Int newGridPos, GameObject room)
    {
        room.GetComponent<DungeonRoom>().gridPos = newGridPos; // set new location on grid
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
        dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y] = room.GetComponent<DungeonRoom>().doorDirections; // replace previous data with door direction data e.g. "RLD"
        dungeonManager.occupiedGridGameObject[newGridPos.x, newGridPos.y] = room;
        
        if (room.GetComponent<DungeonRoom>().doorDirections.Contains("L"))
        {
            if (dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y] == null)
            {
                dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y] = "roomR"; // use key word "room" and add right to its requirement
            }
            else if (doorDirections.Contains("L") && dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y].Contains("room"))
            {
                dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y] += "R"; // add right door to requirements
            }
        }
        if (room.GetComponent<DungeonRoom>().doorDirections.Contains("R"))
        {
            if (dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y] == null)
            {
                dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y] = "roomL"; // use key word "room" and add left to its requirement
            }
            else if (doorDirections.Contains("R") && dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y].Contains("room"))
            {
                dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y] += "L"; // add left door to requirements
            }
        }
        if (room.GetComponent<DungeonRoom>().doorDirections.Contains("U"))
        {
            if (dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1] == null)
            {
                dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1] = "roomD"; // use key word "room" and add left to its requirement
            }
            else if (doorDirections.Contains("U") && dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1].Contains("room"))
            {
                dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1] += "D"; // add left door to requirements
            }
        }
        if (room.GetComponent<DungeonRoom>().doorDirections.Contains("D"))
        {
            if (dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1] == null)
            {
                dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1] = "roomU"; // use key word "room" and add left to its requirement
            }
            else if (doorDirections.Contains("D") && dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1].Contains("room"))
            {
                dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1] += "U"; // add left door to requirements
            }
        }
    }
    private bool CorrectRoom(Vector3Int pos, string doorDirections)
    {
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
        int totalPasses = 0;
        int validPasses = 0;
        foreach (char letter in doorDirections)
        {
            if (doorDirections.Contains("L"))
            {
                if (dungeonManager.occupiedGrid[pos.x -1, pos.y] == null || dungeonManager.occupiedGrid[pos.x -1, pos.y].Contains("R")) // if cell is empty or has path pointing back
                {
                    validPasses += 1;
                }
                totalPasses += 1;
            }
            if (doorDirections.Contains("R"))
            {
                if (dungeonManager.occupiedGrid[pos.x +1, pos.y] == null || dungeonManager.occupiedGrid[pos.x +1, pos.y].Contains("L")) // if cell is empty or has path pointing back
                {
                    validPasses += 1;
                }
                totalPasses += 1;
            }
            if (doorDirections.Contains("U"))
            {
                if (dungeonManager.occupiedGrid[pos.x, pos.y +1] == null || dungeonManager.occupiedGrid[pos.x, pos.y +1].Contains("D")) // if cell is empty or has path pointing back
                {
                    validPasses += 1;
                }
                totalPasses += 1;
            }
            if (doorDirections.Contains("D"))
            {
                if (dungeonManager.occupiedGrid[pos.x, pos.y -1] == null || dungeonManager.occupiedGrid[pos.x, pos.y -1].Contains("U")) // if cell is empty or has path pointing back
                {
                    validPasses += 1;
                }
                totalPasses += 1;
            }
        }
        if (totalPasses == validPasses)
        {
            return true;
        }
        return false;
    }

    public void InstantiateRoom(Vector3Int newGridPos, string requiredDirections)
    {
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
        if (dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y] == null || dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y].Contains("room"))
        {
            bool validRoom = false;
            int iterationCounter = 0;
            while (!validRoom && iterationCounter <= 50)
            {
                int validCounter = 0; // always set validcounter to 0 on each iteration
                GameObject potentialRoom = dungeonManager.rooms[Random.Range(0, dungeonManager.rooms.Length)]; // get and store potential room 
                foreach (char letter in potentialRoom.GetComponent<DungeonRoom>().doorDirections) // loops through each letter in doordirection string
                {
                    if (requiredDirections.Contains(letter.ToString()) && letter.ToString() != "") // checks if given potential rooms direction matches required
                    {
                        validCounter += 1; // increment counter by 1
                    }
                }
                if (validCounter == requiredDirections.Length && CorrectRoom(newGridPos, potentialRoom.GetComponent<DungeonRoom>().doorDirections)) // if counter shows that room contains all required directions
                {
                    GameObject room = Instantiate(potentialRoom, new Vector3Int(newGridPos.x*26, newGridPos.y*26, 0), Quaternion.identity); // instantiate room in tilemap grid
                    room.transform.parent = dungeonManager.transform; // set parent
                    dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y] = room.GetComponent<DungeonRoom>().doorDirections; // replace previous data with door direction data e.g. "RLD"
                    UpdateOccupiedGrid(newGridPos, room);
                    if (room.GetComponent<DungeonRoom>().doorDirections.Length > 1)
                    {
                        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
                        room.GetComponent<DungeonRoom>().CreateNextRoom();
                    }
                    validRoom = true;
                }
                iterationCounter ++;
            }
            if (!validRoom)
            {
                Debug.Log("brick spawned");
                GameObject brick = Instantiate(dungeonManager.bricks[0], new Vector3Int(newGridPos.x*26, newGridPos.y*26, 0), Quaternion.identity);
                brick.transform.parent = dungeonManager.transform;
                UpdateOccupiedGrid(newGridPos, brick);
            }
        }
    }

    public void CreateNextRoom()
    {
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
        string requiredDirections; // stores which paths next spawned room must have
        if (doorDirections.Contains("L")) // spawn a room on the left
        {
            requiredDirections = CheckSurroundingRooms(gridPos, "L");
            
            Vector3Int newGridPos = new Vector3Int(gridPos.x -1, gridPos.y); // create new vector3 pos of where the room should spawn at
            InstantiateRoom(newGridPos, requiredDirections); // call method which will verify and instantiate rooms
        }
        if (doorDirections.Contains("R")) // spawn a room on the right
        {
            requiredDirections = CheckSurroundingRooms(gridPos, "R");

            Vector3Int newGridPos = new Vector3Int(gridPos.x +1, gridPos.y); // create new vector3 pos of where the room should spawn at
            InstantiateRoom(newGridPos, requiredDirections); // call method which will verify and instantiate rooms
        }
        if (doorDirections.Contains("U")) // spawn a room above
        {
            requiredDirections = CheckSurroundingRooms(gridPos, "U");

            Vector3Int newGridPos = new Vector3Int(gridPos.x, gridPos.y +1); // create new vector3 pos of where the room should spawn at
            InstantiateRoom(newGridPos, requiredDirections); // call method which will verify and instantiate rooms
        }
        if (doorDirections.Contains("D")) // spawn a room below
        {
            requiredDirections = CheckSurroundingRooms(gridPos, "D");

            Vector3Int newGridPos = new Vector3Int(gridPos.x, gridPos.y -1); // create new vector3 pos of where the room should spawn at
            InstantiateRoom(newGridPos, requiredDirections); // call method which will verify and instantiate rooms
        }
    }

    public string CheckSurroundingRooms(Vector3Int currentPos, string checkDirection) // returns what directions are required
    {
        string validDirections = ""; // contains all direction which is free
        
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
        if (checkDirection == "L")
        {
            Vector3Int leftPos = new Vector3Int(currentPos.x - 1, currentPos.y); // subtract 1 from x coord to get left position
            if (dungeonManager.occupiedGrid[leftPos.x, leftPos.y] != null && dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Contains("room"))
            {
                validDirections += dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Substring(4); // returns everything past Req e.g. "LD" (left + down)
            }
        }
        if (checkDirection == "R")
        {
            Vector3Int rightPos = new Vector3Int(currentPos.x + 1, currentPos.y); // add 1 to x coord to get right position
            if (dungeonManager.occupiedGrid[rightPos.x, rightPos.y] != null && dungeonManager.occupiedGrid[rightPos.x, rightPos.y].Contains("room"))
            {
                validDirections += dungeonManager.occupiedGrid[rightPos.x, rightPos.y].Substring(4); // returns everything past Req e.g. "LD" (left + down)
            }
        }
        if (checkDirection == "U")
        {
            Vector3Int upPos = new Vector3Int(currentPos.x, currentPos.y + 1); // add 1 to y coord to get up position
            if (dungeonManager.occupiedGrid[upPos.x, upPos.y] != null && dungeonManager.occupiedGrid[upPos.x, upPos.y].Contains("room"))
            {
                validDirections += dungeonManager.occupiedGrid[upPos.x, upPos.y].Substring(4); // returns everything past Req e.g. "LD" (left + down)
            }
        }
        if (checkDirection == "D")
        {
            Vector3Int downPos = new Vector3Int(currentPos.x, currentPos.y - 1); // subtract 1 from x coord to get down position
            if (dungeonManager.occupiedGrid[downPos.x, downPos.y] != null && dungeonManager.occupiedGrid[downPos.x, downPos.y].Contains("room"))
            {
                validDirections += dungeonManager.occupiedGrid[downPos.x, downPos.y].Substring(4); // returns everything past Req e.g. "LD" (left + down)
            }
        }

        if (validDirections == "")
        {
            switch (checkDirection) // return oposite of what checkDirection was
            {
                case "L":
                    validDirections += "R";
                    break;
                case "R":
                    validDirections += "L";
                    break;
                case "U":
                    validDirections += "D";
                    break;
                case "D":
                    validDirections += "U";
                    break;
            }
        }
        return validDirections;
    }

    private void RoomCleared()
    {
        if (enemyCount == 0 && spawnedEnemies == true) // if spawned enemies are all dead 
        {
            DungeonManager dungeonManagerScript = dungeonManager.GetComponent<DungeonManager>();
            OpenDoors("LRUD"); // open all doors
            if (doorDirections.Contains("L")) // if room has a left door
            {
                GameObject leftRoom = dungeonManagerScript.occupiedGridGameObject[gridPos.x -1, gridPos.y]; // get room left of current room
                if (leftRoom.TryGetComponent(out DungeonRoom leftRoomScript))
                {
                    leftRoomScript.OpenDoors("R"); // open right door
                }
            }
            if (doorDirections.Contains("R")) // if room has a right door
            {
                GameObject rightRoom = dungeonManagerScript.occupiedGridGameObject[gridPos.x +1, gridPos.y]; // get room right of current room
                if (rightRoom.TryGetComponent(out DungeonRoom rightRoomScript))
                {
                    rightRoomScript.OpenDoors("L"); // open left door
                }
            }
            if (doorDirections.Contains("U")) // if room has a top door
            {
                GameObject aboveRoom = dungeonManagerScript.occupiedGridGameObject[gridPos.x, gridPos.y +1]; // get room above of current room
                if (aboveRoom.TryGetComponent(out DungeonRoom aboveRoomScript))
                {
                    aboveRoomScript.OpenDoors("D"); // open bottom door
                }
            }
            if (doorDirections.Contains("D")) // if room has a bottom door
            {
                GameObject belowRoom = dungeonManagerScript.occupiedGridGameObject[gridPos.x, gridPos.y -1]; // get room below of current room
                if (belowRoom.TryGetComponent(out DungeonRoom belowRoomScript))
                {
                    belowRoomScript.OpenDoors("U"); // open top door
                }
            }
            enemyCount ++;
        }
    }
}