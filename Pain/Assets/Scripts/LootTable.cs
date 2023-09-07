using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    // create list that takes in gameobjects 
    public List<GameObject> lootList = new List<GameObject>();
    List<GameObject> GetDroppedItem()
    {
        // generate random number to 2dp
        int randomInt = Random.Range(0, 10001); // 0 to 10000
        float roundedNumber = (float)randomInt / 100; // 0 to 100 to 2dp

        List<GameObject> dropItem = new List<GameObject>(); // list for all items that will be dropped
        List<GameObject> possibleItems = new List<GameObject>(); // list for all possible items that can be dropped

        List<string> guaranteeItems = new List<string> {"Coin"}; // holds the name of all items that should always drop
        
        foreach (GameObject item in lootList) // loops through each item in list 
        {   
            string itemName = item.GetComponent<Loot>().lootName; // gets name of object and checks if it is in guaranteeitem list
            if (guaranteeItems.Contains(itemName))
            {
                dropItem.Add(item); // add item to list
            }
            else if (roundedNumber <= item.GetComponent<Loot>().dropChance) // checks if drop chance is greater then random number 
            {
                possibleItems.Add(item); // add item to list 
            }
        }
        
        if (possibleItems.Count > 0) // possible items will be added to list
        {
            dropItem.Add(possibleItems[Random.Range(0, possibleItems.Count)]);
        }
        if (dropItem.Count > 0) // seperate if statement do not combine
        {
            // only returns one item by index
            // random index by 0 to length of list
            return dropItem;
        }
        
        return null; // no items dropped
    }

    public void InstantiateLoot(Vector3 spawnLocation)
    {
        List<GameObject> droppedItems = GetDroppedItem(); // calls function and stores in list
        
        if (droppedItems != null)
        {
            // loops through each item in list
            foreach (GameObject dropItems in droppedItems)
            {
                Instantiate(dropItems, spawnLocation, Quaternion.identity); // create object
            }
        }
    }
}


