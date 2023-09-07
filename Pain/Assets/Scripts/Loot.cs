using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour // apply to object and make prefab
{
    // holds all info about item
    public GameObject lootObject;
    public string lootName;
    public float dropChance;
    public string itemDescription; // mainly used in UI
    
}
