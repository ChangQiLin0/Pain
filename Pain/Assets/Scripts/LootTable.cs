using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    // create list that takes in type Loot
    public List<string> lootTableList = new List<string>(); // initiate loot table as list
    private GameObject playerObject; // get player as an object
    private Player player; // get player script

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player"); // get player object
        player = playerObject.GetComponent<Player>();
    }

    public void GetDroppedLoot() // get loot when enemy dies
    {
        // generate random number to 2dp
        int randomInt = Random.Range(0, 10001); // 0 to 10000
        float roundedNumber = (float)randomInt / 100; // 0 to 100 to 2dp
        Debug.Log("number ="+roundedNumber);

        List<GameObject> possibleLoot = new List<GameObject>(); // list of all loot that could drop
        GameObject lootObject = null; // default to null (no loot dropped)

        foreach (string lootString in lootTableList) // loop through each item in lootList
        {   
            if (!ObtainDefinitions.Instance.dropChance.ContainsKey(lootString)) // if lootstring isnt in dropchance
            {
                switch(lootString)
                {
                    case "Coin": // 100% drop chance
                        float rngCoins = Random.Range(0,6); // 0-5 inclusive
                        player.totalCoins += rngCoins * player.coinMultiplier; // increase player coin balance
                        break; // does not add to possibleloot
                    case "Exp": // 100% drop chance
                        float rngExp = Random.Range(3,10); // 3-9 inclusive
                        player.totalExp += rngExp * player.expMultiplier; // increase player exp
                        player.LevelUpCalculation(); // update player exp
                        break;
                }
            }
            else if (roundedNumber <= ObtainDefinitions.Instance.dropChance[lootString]) // check if item should drop from dictionary
            {
                switch(lootString) // start switch statement passing in name of loot
                {
                    case "Gun0": // PROBABLY CAN MAKE THIS SO MULTIPLE CASES DIRECTS TO ADD ITEM CHECK DICTUONARY ---------------------------
                        possibleLoot.Add(ObtainDefinitions.Instance.gun0); // add to loot pool
                        Debug.Log("added loot gun0 to possible loot");
                        break;
                    case "TestLoot0":
                        possibleLoot.Add(ObtainDefinitions.Instance.testLoot0); // add to loot pool
                        Debug.Log("added loot TestLoot0 to possible loot");
                        break;
                    case "Desert Eagle":
                        possibleLoot.Add(ObtainDefinitions.Instance.desertEagle); // add to loot pool
                        Debug.Log("added loot Desert Eagle to possible loot");
                        break;
                    case "Shotgun":
                        possibleLoot.Add(ObtainDefinitions.Instance.shotgun); // add to loot pool
                        Debug.Log("added loot shotgun to possible loot");
                        break;
                    case "Speed Potion":
                        possibleLoot.Add(ObtainDefinitions.Instance.speedPotion); // add to loot pool
                        Debug.Log("added speed pot");
                        break;
                    case "Health Potion":
                        possibleLoot.Add(ObtainDefinitions.Instance.healthPotion); // add to loot pool
                        Debug.Log("added health pot");
                        break;
                    case "Steriods":
                        possibleLoot.Add(ObtainDefinitions.Instance.steriods); // add to loot pool
                        Debug.Log("added steriods");
                        break;
                    
                    default: 
                        Debug.Log("Something passed switchcase in LootTable"); 
                        break;
                }
            }
        }
        if (possibleLoot.Count > 0) // possible items will be added to list
        {
            lootObject = possibleLoot[Random.Range(0, possibleLoot.Count)]; // pick one drop list at random
        }
        else if (possibleLoot.Count == 1)
        {
            lootObject = possibleLoot[0]; // get first/only object in list
        }

        if (lootObject != null)
        {
            InstantiateLoot(lootObject); // only called if lootObject is not null
        }
    }

    public void InstantiateLoot(GameObject dropThisLoot)
    {   
        GunScript gunScript = dropThisLoot.GetComponent<GunScript>();
        if (gunScript) // if object contains gunscript
        {
            float lowerBound = gunScript.lowerBound + (player.playerLevel/5f); // every 5 player level add 1 to min
            float upperBound = gunScript.upperBound + 10f * (player.playerLevel/10f); // every 10 levels add 10 to max 

            GameObject createdObject = Instantiate(dropThisLoot, transform.position, Quaternion.identity); // create object FIRST
            createdObject.tag = "Loot"; // apply loot tag to dropped object


            // do this for all stats //
            GunScript modifyScript = createdObject.GetComponent<GunScript>(); // get script
            modifyScript.baseDamage = Mathf.Round(Random.Range(lowerBound, upperBound/10f)*100f)/100f; // round to 2dp


            // add rng to curse loot
            // dont allow stackable loot to be curseable
            // place variable in player component and set to global
            CollectibleLoot collectibleLoot = createdObject.GetComponent<CollectibleLoot>(); // get collectibleloot component
            collectibleLoot.isInWorld = true;
        }
        else
        {
            GameObject createdObject = Instantiate(dropThisLoot, transform.position, Quaternion.identity);
            CollectibleLoot collectibleLoot = createdObject.GetComponent<CollectibleLoot>(); // get collectibleloot component
            collectibleLoot.isInWorld = true;
        }
    }
}