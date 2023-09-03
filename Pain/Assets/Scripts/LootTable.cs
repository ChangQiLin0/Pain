using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    public GameObject itemPrefab;
    public List<Loot> lootList = new List<Loot>();


    Loot GetDroppedItem()
    {
        // generate random number to 2dp
        int randomInt = Random.Range(0, 10001); // 0 to 10000
        float roundedNumber = (float)randomInt / 100; // 0 to 100 to 2dp

        List<Loot> possibleItems = new List<Loot>();

        foreach (Loot item in lootList)
        {
            if (roundedNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }

        if (possibleItems.Count > 0)
        {
            // only returns one item by index
            // random index by 0 to length of list
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        // no items dropped
        return null;
    }

    public void InstantiateLoot(Vector3 spawnLocation)
    {
        Loot droppedItem = GetDroppedItem();

        if (droppedItem != null)
        {
            // item prefab, location, rotation
            GameObject lootGameObject = Instantiate(itemPrefab, spawnLocation, Quaternion.identity);
            // updates sprite of dropped item
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;
        }
    }

}


