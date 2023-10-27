using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
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
        
        if (dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y] != null && dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y].Contains("Room"))
        {
            dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y] += "R"; // requires a right door
        }
        else if (dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y] != null && dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y] == null)
        {
            dungeonManager.occupiedGrid[newGridPos.x -1, newGridPos.y] += "RoomR"; // add room keyword + direction required
        }

        if (dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y] != null && dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y].Contains("Room"))
        {
            dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y] += "L"; // requires a right door
        }
        else if (dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y] != null && dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y] == null)
        {
            dungeonManager.occupiedGrid[newGridPos.x +1, newGridPos.y] += "RoomL"; // add room keyword + direction required
        }

        if (dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1] != null && dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1].Contains("Room"))
        {
            dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1] += "U"; // requires a right door
        }
        else if (dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1] != null && dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1] == null)
        {
            dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y -1] += "RoomU"; // add room keyword + direction required
        }

        if (dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1] != null && dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1].Contains("Room"))
        {
            dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1] += "D"; // requires a right door
        }
        else if (dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1] != null && dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1] == null)
        {
            dungeonManager.occupiedGrid[newGridPos.x, newGridPos.y +1] += "RoomD"; // add room keyword + direction required 
        }

        room.GetComponent<DungeonRoom>().CreateNextRoom(); // continue making rooms if possible
    }


    public void CreateNextRoom()
    {
        string requiredDirections = ""; // stores which paths next spawned room must have
        if (doorDirections.Contains("L")) // spawn a room on the left
        {
            requiredDirections = CheckSurroundingRooms(gridPos, "L");
            dungeonManager.occupiedGrid[gridPos.x, gridPos.y] = doorDirections; // remove "Room" keyword from grid to prevent overlap

            Debug.Log(requiredDirections);
            Vector3Int newGridPos = new Vector3Int(gridPos.x - 1, gridPos.y); // create new vector3 pos of where the room should spawn at
            GameObject room = Instantiate(dungeonManager.roomR[0], new Vector3Int(newGridPos.x*26, newGridPos.y*26, 0), quaternion.identity); // instantiate room in tilemap grid
            room.transform.parent = dungeonManager.transform; // set parent
            
            UpdateOccupiedGrid(newGridPos, room);
            
        }
        if (doorDirections.Contains("R")) // spawn a room on the right
        {
            requiredDirections = CheckSurroundingRooms(gridPos, "R");
            Debug.Log(requiredDirections);
            GameObject room = Instantiate(dungeonManager.roomL[0], new Vector3Int((gridPos.x + 1)*26, gridPos.y*26, 0), quaternion.identity);
            room.transform.parent = dungeonManager.transform;
        }
        if (doorDirections.Contains("U")) // spawn a room above
        {
            requiredDirections = CheckSurroundingRooms(gridPos, "U");
            Debug.Log(requiredDirections);
            GameObject room = Instantiate(dungeonManager.roomD[0], new Vector3Int(gridPos.x*26, (gridPos.y + 1)*26, 0), quaternion.identity);
            room.transform.parent = dungeonManager.transform;
        }
        if (doorDirections.Contains("D")) // spawn a room below
        {
            requiredDirections = CheckSurroundingRooms(gridPos, "D");
            Debug.Log(requiredDirections);
            GameObject room = Instantiate(dungeonManager.roomU[0], new Vector3Int(gridPos.x*26,(gridPos.y - 1)*26, 0), quaternion.identity);
            room.transform.parent = dungeonManager.transform;
        }
        
    }

    public string CheckSurroundingRooms(Vector3Int currentPos, string checkDirection) // returns what directions are required
    {
        string returnString = "";
        dungeonManager = transform.parent.GetComponent<DungeonManager>(); // get updated instance of dungeon manager
        if (checkDirection == "L")
        {
            Vector3Int leftPos = new Vector3Int(currentPos.x -= 1, currentPos.y); // subtract 1 from x coord to get left position
            if (dungeonManager.occupiedGrid[leftPos.x, leftPos.y] != null && dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Contains("Room"))
            {
                returnString += dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Substring(4); // returns everything past Req e.g. "LD" (left + down)
            }
        }
        if (checkDirection == "R")
        {
            Vector3Int leftPos = new Vector3Int(currentPos.x += 1, currentPos.y); // add 1 to x coord to get right position
            if (dungeonManager.occupiedGrid[leftPos.x, leftPos.y] != null && dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Contains("Room"))
            {
                returnString += dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Substring(4); // returns everything past Req e.g. "LD" (left + down)
            }
        }
        if (checkDirection == "U")
        {
            Vector3Int leftPos = new Vector3Int(currentPos.x, currentPos.y += 1); // add 1 to y coord to get up position
            if (dungeonManager.occupiedGrid[leftPos.x, leftPos.y] != null && dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Contains("Room"))
            {
                returnString += dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Substring(4); // returns everything past Req e.g. "LD" (left + down)
            }
        }
        if (checkDirection == "D")
        {
            Vector3Int leftPos = new Vector3Int(currentPos.x, currentPos.y -= 1); // subtract 1 from x coord to get down position
            if (dungeonManager.occupiedGrid[leftPos.x, leftPos.y] != null && dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Contains("Room"))
            {
                returnString += dungeonManager.occupiedGrid[leftPos.x, leftPos.y].Substring(4); // returns everything past Req e.g. "LD" (left + down)
            }
        }

        switch (checkDirection) // return oposite of what checkDirection was
        {
            case "L":
                returnString += "R";
                break;
            case "R":
                returnString += "L";
                break;
            case "U":
                returnString += "D";
                break;
            case "D":
                returnString += "U";
                break;
        }
        
        return returnString;
    }
}
