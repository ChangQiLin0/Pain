using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    // create list that takes in type Loot
    public List<string> lootTableList = new List<string>();
    private GameObject playerObject; // get player
    Player player;

    public Dictionary<string, float> dropChance = new Dictionary<string, float>
    {
        {"Coin", 100f},
        {"gun0", 100f}
    };

    public Dictionary<string, bool> isStackable = new Dictionary<string, bool>
    {
        {"gun0", false}
    };

    public GameObject gun0;





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

        List<GameObject> possibleLoot = new List<GameObject>(); // list of all loot that could drop
        GameObject lootObject = null; // default to null (no loot dropped)

        foreach (string lootString in lootTableList) // loop through each item in lootList
        {
            if (roundedNumber <= dropChance[lootString]) // check if item should drop from dictionary
            {
                switch(lootString) // start switch statement passing in name of loot
                {
                    case "Coin": // 100% drop chance
                        float rngCoins = Random.Range(-1,6); // 1-5 inclusive
                        player.totalCoins += rngCoins * player.coinMultiplier; // increase player coin balance
                        break; // does not add to possibleloot
                    // add exp here later on

                    case "gun0": 
                        possibleLoot.Add(gun0); // add to loot pool
                        Debug.Log("added loot gun0 to possible loot");
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
        Debug.Log(lootObject);

        if (lootObject != null)
        {
            InstantiateLoot(lootObject); // only called if lootObject is not null
        }
        
    }



    // put in instantiate loot
    // Instantiate(gun0, transform.position, Quaternion.identity);
    
    // GunScript gunScriptComp = gun0.GetComponent<GunScript>();

    public void InstantiateLoot(GameObject dropThisLoot)
    {   
        float lowerBound = 1f + (player.playerLevel/5); // every 5 player level add 1 to min
        float upperBound = 101f + 10 * (player.playerLevel/10); // every 10 levels add 10 to max 
        if (dropThisLoot.GetComponent<GunScript>())
        {
            dropThisLoot.tag = "Item";
            GunScript modifyScript = dropThisLoot.GetComponent<GunScript>();
            


            modifyScript.baseDamage = Mathf.Round(Random.Range(lowerBound, upperBound/10)*100f)/100f; // round to 2dp


            Instantiate(dropThisLoot, transform.position, Quaternion.identity); // create object
        }
        // reminder to add loot drop for enemy script
        // replace this on enemy script with something else
    }
}