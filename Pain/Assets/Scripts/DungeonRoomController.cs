using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoomController : MonoBehaviour
{
    public Tilemap tilemap; // tile grid everything will be based off of
    public GameObject tilePrefab;
    public void Start()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        GameObject a = Instantiate(tilePrefab, pos, Quaternion.identity);
        a.transform.SetParent(tilemap.transform);

        Vector3 pos2 = new Vector3(0, 26, 0);
        GameObject b = Instantiate(tilePrefab, pos2, Quaternion.identity);
        b.transform.SetParent(tilemap.transform);

        Vector3Int pos3 = new Vector3Int(13, 23, 0);
        Vector3Int pos4 = new Vector3Int(12, 23, 0);
        Vector3Int pos5 = new Vector3Int(13, 22, 0);
        Vector3Int pos6 = new Vector3Int(12, 22, 0);
        a.transform.GetChild(1).GetComponent<Tilemap>().SetTile(pos3, null);
        a.transform.GetChild(1).GetComponent<Tilemap>().SetTile(pos4, null);
        a.transform.GetChild(1).GetComponent<Tilemap>().SetTile(pos5, null);
        a.transform.GetChild(1).GetComponent<Tilemap>().SetTile(pos6, null);
    }
}
