using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot
{
    // PROBABLY CAN DELETE THIS SCRIPT/CLASS














    // holds all info about item
    //public Sprite lootSprite;
    public string lootName; // name of loot 
    public float lootDropChance; // chance of dropping 
    public float isStackable; // check if loot is stackable 
    // ----- might add luck

    //public string itemDescription; // mainly used in UI
    //public int amount;
    public float randomStat;// TEMP FOR TESTING

    // private float player level 
    // increase base roll for every x level e.g. Random.Range(0,10) becomes 1,11


    
    private float LootDropChance()
    {
        switch(lootName) // returns loop drop chance as float 
        {
            case "Health Potion": return 100f; // TEMP 100% drop chance
            default: return 0f;
        }
    }

    private GameObject LootObjectPrefab() // return object prefab 
    {
        switch(lootName)
        {
            case "Health Potion": return null;
            default: return null;
            // case "gunpart_0":
            //      // base level bonus = 10
            //      x = base + (level bonus)/z  // z to dilute bonus or increase using 0.5 etc 
            //                                  // round to 2dp
            //      gunpart.damage = Random.Range(x-y)
        }
    }


    public Loot()
    {
        
    }
    
    // brain storm

    // Loot(item name, player luck)
    // return dropchance 
    // switch(itemname) 
    // case "gunpart_0": return 
}
