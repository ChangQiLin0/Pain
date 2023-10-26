using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Transform invParent; // holds inventory parent position 
    public Dictionary<string, int> stackedLoot = new Dictionary<string, int>(); // holds all items that have been stacked + number
    public int inventoryCount; // used to check if inventory is full or not/ number of items in inv
    public bool fullInventory;

    public void Awake()
    {
        invParent = transform.Find("Inventory"); // gets inventory main parent
    }

    public void UpdateAddInv(GameObject objectToAdd)
    {   
        bool foundEmpty = false; // makes sures that every time this is ran always start at false
        int i = 0; // counter for indexing, always set index back to 0 time method is called
        while (!foundEmpty && i < 21) // when not found or not end of list keep running 
        {
            GridLayoutGroup gridLayoutGroup = invParent.GetComponent<GridLayoutGroup>(); // get layout grid 
            Transform invLootChildSlot = gridLayoutGroup.transform.GetChild(i); // gets child based on index using i from the counter
            if (invLootChildSlot.childCount < 2) // if parent doesnt have a child
            {   
                if (!objectToAdd.TryGetComponent(out Image objectImage)) // attemps to get image component and stores any results in objectImage
                {
                    if (objectImage == null)
                    {
                        objectImage = objectToAdd.AddComponent<Image>(); // add image component
                    }
                }
                if (!objectToAdd.TryGetComponent(out SpriteRenderer objectSpriteRenderer)) // attemps to get component
                {
                    if (objectSpriteRenderer == null) // check if spriterender component is on parent object if not check child
                    {
                        objectSpriteRenderer = objectToAdd.GetComponentInChildren<SpriteRenderer>(); // get spriterender from children
                    }
                }
                objectImage.preserveAspect = true; // make sure that objects arent stretched 
                objectImage.sprite = objectSpriteRenderer.sprite; // set image sprite
                RectTransform objectSlotInstantiate = Instantiate(objectToAdd.transform, invLootChildSlot).GetComponent<RectTransform>(); // instantiate object
                objectSlotInstantiate.gameObject.SetActive(true); // allow object to be visible
                objectSlotInstantiate.SetAsFirstSibling();

                foundEmpty = true; // stop while loop to as object has been insantiated
            }
            i ++; // add one to counter 
        }
    }

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
                    UpdateAddInv(loot); // update inventory UI
                }
            }
            else
            {
                inventoryCount ++; // add 1 to overall inv count
                stackedLoot[loot.name] = 1; // append to dictionary and set number to 1
                UpdateAddInv(loot); // update inventory UI
            }
        }
        else
        {
            inventoryCount ++; // add 1 to overall inv count
            UpdateAddInv(loot); //update inventory UI 
        }
    }
}