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
    public Vector3Int gridPos; // store grid pos relative to occupied grid
    private Tile[] closedDoorTiles;
    private Tile[] openDoorTiles; // store all open door tiles
    private DungeonManager dungeonManager;


    private void Start()
    {
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get dungeonmanager script from parent
        openDoorTiles = dungeonManager.openDoorTiles; // get tile asset
        closedDoorTiles = dungeonManager.closedDoorTiles; // get tile asset
    }

    private void OpenVerticalDoors() // locate all vertical doors (above/below doors)
    {
        Tilemap aboveLayer = transform.GetChild(1).GetComponent<Tilemap>(); // get CollisionAbove child 
        Tilemap belowLayer = transform.GetChild(2).GetComponent<Tilemap>(); // get CollisionBelow child 

        BoundsInt bounds = aboveLayer.cellBounds; // gets 26x26 areas coordinates, min and max coords
        Vector3Int boundsMin = bounds.min; // get min point of bounds (bottom left)
        boundsMin.x += 12;
        for (int i = 0; i < 26; i ++) // loops from 0 to 25 inclusive
        {
            boundsMin.y += 1; // increment y position by 1 (move up)
            TileBase aboveTile = aboveLayer.GetTile(boundsMin); // retrives tile at pos in above layer
            TileBase belowTile = belowLayer.GetTile(boundsMin); // retrives tile at pos in below layer
            if (closedDoorTiles[0] == aboveTile) // if above tile is top left closed door tile
            {
                aboveLayer.SetTile(boundsMin, null); // remove top left
                aboveLayer.SetTile(new Vector3Int(boundsMin.x, boundsMin.y - 1, 0), null); // bottom left
                aboveLayer.SetTile(new Vector3Int(boundsMin.x + 1, boundsMin.y, 0), null); // top right
                aboveLayer.SetTile(new Vector3Int(boundsMin.x + 1, boundsMin.y -1, 0), null); // bottom right
                Debug.Log("all door peices removed -above");
            }
            if (closedDoorTiles[0] == belowTile) // if below tile is top left closed door tile
            {
                belowLayer.SetTile(boundsMin, null); // remove top left
                belowLayer.SetTile(new Vector3Int(boundsMin.x, boundsMin.y - 1, 0), null); // bottom left
                belowLayer.SetTile(new Vector3Int(boundsMin.x + 1, boundsMin.y, 0), null); // top right
                belowLayer.SetTile(new Vector3Int(boundsMin.x + 1, boundsMin.y -1, 0), null); // bottom right
                Debug.Log("all door peices removed -below");
            }
        }
    }

    private void OpenHorizontalDoors() // locate all horizontal doors (left/right doors)
    {
        Tilemap aboveLayer = transform.GetChild(1).GetComponent<Tilemap>(); // get CollisionAbove child 
        BoundsInt bounds = aboveLayer.cellBounds; // gets 26x26 areas coordinates, min and max coords
        Vector3Int boundsMin = bounds.min; // get min point of bounds (bottom left)
        boundsMin.y += 13;

        for (int i = 0; i < 26; i ++) // loops from 0 to 25 inclusive
        {
            boundsMin.x += 1; // increment x position by 1 (move up)
            TileBase aboveTile = aboveLayer.GetTile(boundsMin); // retrives tile at pos in above layer
            if (closedDoorTiles[4] == aboveTile) // if above tile is top left closed door tile
            {
                aboveLayer.SetTile(boundsMin, openDoorTiles[4]); // replace closed door tile with open door tile
                aboveLayer.SetTile(new Vector3Int(boundsMin.x, boundsMin.y - 1, 0), openDoorTiles[5]); // bottom tile
                Debug.Log("all door peices removed -left");
            }
            else if (closedDoorTiles[6] == aboveTile)
            {
                aboveLayer.SetTile(boundsMin, openDoorTiles[6]); // remove top tile
                aboveLayer.SetTile(new Vector3Int(boundsMin.x, boundsMin.y - 1, 0), openDoorTiles[7]); // bottom tile
                Debug.Log("all door peices removed -right");
            }
        }
    }

    public void EnterRoom()
    {
        Debug.Log("Entered a room");
        // add doors behind player
    }

    public void UpdateOccupiedGrid(Vector3Int newGridPos, GameObject room)
    {
        room.GetComponent<DungeonRoom>().gridPos = newGridPos; // set new location on grid
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
        dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y] = room.GetComponent<DungeonRoom>().doorDirections; // replace previous data with door direction data e.g. "RLD"
        
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
        bool returnBool = false;
        foreach (char letter in doorDirections)
        {
            if (doorDirections.Contains("L"))
            {
                if (dungeonManager.occupiedGrid[pos.x -1, pos.y] == null || dungeonManager.occupiedGrid[pos.x -1, pos.y].Contains("R")) // if cell is empty or has path pointing back
                {
                    returnBool = true;
                }
                else
                {
                    returnBool = false;
                }
            }
            if (doorDirections.Contains("R"))
            {
                if (dungeonManager.occupiedGrid[pos.x +1, pos.y] == null || dungeonManager.occupiedGrid[pos.x +1, pos.y].Contains("L")) // if cell is empty or has path pointing back
                {
                    returnBool = true;
                }
                else
                {
                    returnBool = false;
                }
            }
            if (doorDirections.Contains("U"))
            {
                if (dungeonManager.occupiedGrid[pos.x, pos.y +1] == null || dungeonManager.occupiedGrid[pos.x, pos.y +1].Contains("D")) // if cell is empty or has path pointing back
                {
                    returnBool = true;
                }
                else
                {
                    returnBool = false;
                }
            }
            if (doorDirections.Contains("D"))
            {
                if (dungeonManager.occupiedGrid[pos.x, pos.y -1] == null || dungeonManager.occupiedGrid[pos.x, pos.y -1].Contains("U")) // if cell is empty or has path pointing back
                {
                    returnBool = true;
                }
                else
                {
                    returnBool = false;
                }
            }
        }
        return returnBool;
    }

    public void InstantiateRoom(Vector3Int newGridPos, string requiredDirections)
    {
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
        if (dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y] != null && dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y].Contains("room"))
        {
            bool validRoom = false;
            while (!validRoom)
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
}