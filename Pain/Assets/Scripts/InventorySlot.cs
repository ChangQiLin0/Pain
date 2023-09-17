using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) // should only have one item in slot if 0, its empty 
        {
            Debug.Log("dropped onto slot");
            GameObject droppedObject = eventData.pointerDrag; // gets and stores dropped object on slot
            CollectibleLoot lootComponent = droppedObject.GetComponent<CollectibleLoot>(); // get collecible loot script 
            lootComponent.lootParent = transform; // set parent current slot
        }
        else if (transform.childCount == 1) // if item/child is present 
        {
            Debug.Log("swapping items");
            
            GameObject droppedObject = eventData.pointerDrag; // gets and stores dropped object on slot 
            CollectibleLoot lootComponent = droppedObject.GetComponent<CollectibleLoot>(); // get collecible loot script
            Transform originalParent = lootComponent.lootParent; // store original parent as temp variable for later use 

            Transform itemInSlot = transform.GetChild(0); // get child object
            lootComponent.lootParent = transform; // set transform in original script so it doesnt swap back
            droppedObject.transform.SetParent(transform); // swap with current slot
            itemInSlot.SetParent(originalParent); // swap with original parent
        }
    }
}
