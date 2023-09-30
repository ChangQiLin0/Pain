using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventory: MonoBehaviour
{
    public Dictionary<string, int> stackedLoot = new Dictionary<string, int>(); // holds all items that have been stacked + number
    public int inventoryCount; // used to check if inventory is full or not/ number of items in inv
    public InventoryUI inventoryUI;

    public void AddLoot(GameObject loot)
    {
        loot.name = loot.name.Replace("(Clone)", "").Trim();
        if (ObtainDefinitions.Instance.isStackable[loot.name]) // check if is stackable 
        {
            if (stackedLoot.ContainsKey(loot.name)) // check if is in inv and stacked
            {
                if (stackedLoot[loot.name] > 0)
                {
                    stackedLoot[loot.name] ++; // add to dictionary count 
                }
                else
                {
                    inventoryCount ++; // add 1 to overall inv count
                    stackedLoot[loot.name] = 1; // append to dictionary and set number to 1
                    inventoryUI.UpdateAddInv(loot); // update inventory UI
                }
            }
            else
            {
                inventoryCount ++; // add 1 to overall inv count
                stackedLoot[loot.name] = 1; // append to dictionary and set number to 1
                inventoryUI.UpdateAddInv(loot); // update inventory UI
            }
        }
        else
        {
            inventoryCount ++; // add 1 to overall inv count
            inventoryUI.UpdateAddInv(loot); //update inventory UI 
        }
    }
}
