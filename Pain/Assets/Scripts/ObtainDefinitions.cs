using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class ObtainDefinitions : MonoBehaviour
{
    // store all dictionary/sprites required 
    private Player player;
    public InventoryUI InventoryUI;
    public static ObtainDefinitions Instance { get; private set; }
    public delegate string StringAction(string functionType, GameObject gameObject); // defines delegate that is a function that takes in a string as a parameter

    public Dictionary<string, float> dropChance = new Dictionary<string, float> // get drop chance for each item 
    {
        {"Gun0", 0f},
        {"Starter Pistol", 0f},
        {"TestLoot0", 0f},
        {"Desert Eagle", 10f},
        {"Shotgun", 30f},
        {"Speed Potion", 20f},
        {"Health Potion", 20f},
        {"Steriods", 20f}
    };

    public Dictionary<string, bool> isStackable = new Dictionary<string, bool> // pre defined dictionary of all stackable items
    {
        {"Gun0", false},
        {"Starter Pistol", false},
        {"TestLoot0", true},
        {"Desert Eagle", false},
        {"Shotgun", false},
        {"Speed Potion", true},
        {"Health Potion", true},
        {"Steriods", true}
    };

    private Dictionary<string, string> baseDesc = new Dictionary<string, string> // stores all basic description/base descriptions for all items that need it
    {
        {"Gun0", "A somewhat fast but inaccurate gun, always good for killing a few enemies while keeping your hands clean"},
        {"Starter Pistol", "Cheap and useless gun better start killing for a new one"},
        {"TestLoot0", "This is literally a test item nothing more."},
        {"Desert Eagle", "Desert Eagle description placeholder"},
        {"Shotgun", "Literally just a shotgun"},
        {"Speed Potion", "On consumption permanently gain more speed"},
        {"Health Potion", "On consumption regain lost health"},
        {"Steriods", "On consumption permanenty gain more damage"}
    };

    public Dictionary<string, bool> isUseable = new Dictionary<string, bool>
    {
        {"Gun0", false},
        {"Starter Pistol", false},
        {"TestLoot0", true},
        {"Desert Eagle", false},
        {"Shotgun", false},
        {"Speed Potion", true},
        {"Health Potion", true},
        {"Steriods", true}
    };

    public Dictionary<string, StringAction> lootDescription = new Dictionary<string, StringAction>(); // use key to get method with pre defined parameters as modes
    public GameObject gun0; // store gun0 prefab
    public GameObject starterPistol; // starter pistol prefab
    public GameObject testLoot0; // store testloot0 prefab
    public GameObject desertEagle; // store desertEagle prefab
    public GameObject shotgun; // store Shotgun prefab
    public GameObject[] enemies; // store all enemies in an array

    public GameObject speedPotion; // store speedpotion prefab
    public GameObject healthPotion; // store speedpotion prefab
    public GameObject steriods; // store speedpotion prefab

    private void Awake()
    {
        Instance = this;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>(); // get player component so it can be accessed

        lootDescription.Add("Gun", Gun);
        lootDescription.Add("TestLoot0", TestLoot0); // currently a place holder
        lootDescription.Add("Helmet", TestLoot0); // currently a place holder
    }

    public string Gun(string functionType, GameObject gameObject) // function which returns a string and takes in what function it should do + gameobject 
    {
        if (functionType == "desc") // if a description is requested
        {
            GunScript gunScript = gameObject.GetComponent<GunScript>();
            string tooltipDescription = baseDesc[gameObject.name.Replace("(Clone)", "").Trim()]; // get base definition from dictionary 

            tooltipDescription += "<br><br>Attack Damage: " + gunScript.baseDamage; 
            tooltipDescription += "<br>Ammo: " + gunScript.baseAmmo;
            tooltipDescription += "<br>Fire rate: " + gunScript.baseFireRate;
            tooltipDescription += "<br>Reload speed: " + gunScript.baseReloadSpeed;
            // display all stats
            // include cursed or not 
            return tooltipDescription;
        }
        
        else if (functionType == "use")
        {
            return null; // cannot be used
        }
        Debug.Log("Error: invalid function type");
        return functionType;
    }

    public string TestLoot0(string functionType, GameObject gameObject) 
    {
        if (functionType == "desc")
        {
            string tooltipDescription = baseDesc[gameObject.name.Replace("(Clone)", "").Trim()];
            return tooltipDescription;
        }
        
        else if (functionType == "use")
        {
            return null;
        }
        Debug.Log("Error: invalid function type");
        return functionType;
    }
}
