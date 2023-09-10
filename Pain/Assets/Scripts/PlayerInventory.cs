using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerInventory: MonoBehaviour
{
    public List<GameObject> lootList;
    public GameObject gun;

    public void Start()
    {
        lootList = new List<GameObject>();
        lootList.Add(gun);
    }

    public void AddLoot(GameObject loot)
    {
        
    }
    public void DropLoot(GameObject loot) // drop loot into world
    {

    }
}
