using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ObtainDefinitions : MonoBehaviour
{
    // store all dictionary/sprites required 
    private Player player;
    public static ObtainDefinitions Instance { get; private set; }
    public delegate string StringAction(string functionType, GameObject gameObject); // defines delegate that is a function that takes in a string as a parameter
    

    public Dictionary<string, float> dropChance = new Dictionary<string, float> // get drop chance for each item 
    {
        {"Coin", 100f},
        {"Gun0", 100f},
        {"TestLoot0", 100f}
    };

    public Dictionary<string, bool> isStackable = new Dictionary<string, bool> // pre defined dictionary of all stackable items
    {
        {"Gun0", false},
        {"TestLoot0", true},
    };

    private Dictionary<string, string> baseDesc = new Dictionary<string, string> // stores all basic description/base descriptions for all items that need it
    {
        {"Gun0", "A somewhat fast but inaccurate gun, always good for killing a few enemies while keeping your hands clean"},
        {"TestLoot0", "This is literally a test item nothing more."}
    };

    public Dictionary<string, bool> isUseable = new Dictionary<string, bool>
    {
        {"Gun0", false},
        {"TestLoot0", true},
    };

    public Dictionary<string, StringAction> lootDescription = new Dictionary<string, StringAction>(); // use key to get method with pre defined parameters as modes
    public GameObject gun0;
    public GameObject testLoot0;

    private void Awake()
    {
        Instance = this;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>(); // get player component so it can be accessed

        lootDescription.Add("gun", Gun);
        lootDescription.Add("TestLoot0", TestLoot0); // currently a place holder
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
