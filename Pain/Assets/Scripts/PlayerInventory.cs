using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventory: MonoBehaviour
{
    public List<GameObject> lootList;
    public Dictionary<string, int> stackedLoot = new Dictionary<string, int>(); // holds all items that have been stacked + number
    public bool fullInventory;
    public GameUI gameUI;

    public void Awake()
    {
        lootList = new List<GameObject>();
    }

    public void AddLoot(GameObject loot)
    {
        loot.name = loot.name.Replace("(Clone)", "").Trim();
        Debug.Log("loot added: " + loot.name);
        if (ObtainDefinitions.Instance.isStackable.ContainsKey(loot.name)) // check if is stackable 
            {
                if (stackedLoot.ContainsKey(loot.name)) // check if is in inv and stacked
                {
                    stackedLoot[loot.name] ++; // add to dictionary count 
                    // gui number +1 
                }
                else
                {
                    lootList.Add(loot); // add to inventory list if not already in inv 
                    stackedLoot[loot.name] = 1; // append to dictionary and set number to 1
                    gameUI.UpdateAddInv(loot); // update inventory UI
                }
            }
        else
        {
            lootList.Add(loot); // add to inventory list if not already in inv 
            gameUI.UpdateAddInv(loot); //update inventory UI 
        }

        if (lootList.Count > 24)
        {
            fullInventory = true;
        }
        else
        {
            fullInventory = false;
        }

    }
    public void DropLoot(GameObject loot) // drop loot into world
    {
        
    }
}
