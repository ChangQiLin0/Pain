using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private Transform invParent;

    public void SetInventory(PlayerInventory playerInventory)
    {
        this.playerInventory = playerInventory;
    }
    public void Awake()
    {
        invParent = transform.Find("Inventory"); // gets inventory main parent
    }

    public void UpdateAddInv(GameObject objectToAdd)
    {   
        bool foundEmpty = false; // makes sures that every time this is ran always start at false
        int i = 0; // always set index back to 0 time method is called
        while (!foundEmpty && i < 24) // when not found or not end of list keep running 
        {
            GridLayoutGroup gridLayoutGroup = invParent.GetComponent<GridLayoutGroup>(); // get layout grid 
            Transform invLootChildSlot = gridLayoutGroup.transform.GetChild(i);
            if (invLootChildSlot.childCount < 1) // if parent doesnt have a child
            {
                Debug.Log("Working on inv add update"+ objectToAdd.TryGetComponent(out Image abc));
                
                if (!objectToAdd.TryGetComponent(out Image objectImage)) // attemps to get component
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

                foundEmpty = true; // stop while loop to as object has been insantiated
            }
            i ++; // add one to counter 
        }
    }
}