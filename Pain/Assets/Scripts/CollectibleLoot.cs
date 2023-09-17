using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollectibleLoot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // apply script on all gameobjects that can be loot
    public bool isInWorld; // check if item is on the ground, set by whom ever drops it
    private bool collectible;
    private PlayerInventory playerInventory;
    private GameObject lootToBeAdded;
    public Transform lootParent; // stores original parent of dragged object
    private Image image;
    public bool isCursed; // if cursed, player shouldnt be able to move/drop the item

    private void Start()
    {
        image = GetComponent<Image>();
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // get player object
        playerInventory = player.GetComponent<PlayerInventory>(); // get player inventory
    }

    private void FixedUpdate()
    {
        if (isInWorld && collectible && Input.GetKey(KeyCode.E)) 
        // check if e is pressed, is in the physical world and is collectible 
        {   
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) // check if collision is an item
        {
            lootToBeAdded = transform.gameObject; // save object in an variable 
            collectible = true; // object is now collectible 

            // add some onscreen ui stat notif thing later
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) // check if collision is an item
        {
            lootToBeAdded = null; // clear variable of all objects
            collectible = false; // object is not not collectible 
        }
    }

    private void PickUp()
    {   
        lootToBeAdded.name = lootToBeAdded.name.Replace("(Clone)", "").Trim(); // remove all (clone) in name and any leading/following spaces

        if (playerInventory.fullInventory)
        {
            if (playerInventory.stackedLoot.ContainsKey(lootToBeAdded.name)) // check if item in player inventory
            {
                playerInventory.AddLoot(lootToBeAdded); // call method from playerinv class to place into inv
                Debug.Log("destory 1");
                Destroy(lootToBeAdded); // delete loot after collecting 
            }
            else
            {
                Debug.Log("Full inventory"); // add some screen text
            }
        }
        else
        {
            playerInventory.AddLoot(lootToBeAdded); // call method from playerinv class to place into inv
            if (ObtainDefinitions.Instance.isStackable.ContainsKey(lootToBeAdded.name)) // only destory item is is stackable and is already in inventory
            {
                if (playerInventory.stackedLoot[lootToBeAdded.name] > 2) // needs to have atleast one in the world before deleting
                {
                    Debug.Log("destory 2"+ playerInventory.stackedLoot[lootToBeAdded.name]);
                    Destroy(lootToBeAdded); 
                }
                else
                {
                    lootToBeAdded.SetActive(false); // hides gameobject but not deletes as it may need to be referenced
                }
            }
            else
            {
                lootToBeAdded.SetActive(false); // hides gameobject but not deletes as it may need to be referenced
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) // activates when dragging of item starts 
    {
        Debug.Log("A");
        lootParent = transform.parent; // gets parent of object which is the slot container and stores it as parent
        transform.SetParent(transform.root); // set parent as the highest point in the heirarchy
        transform.SetAsLastSibling(); // places it at the top of the hierarchy for dragging layer
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData) // activates when actively dragging
    {
        Debug.Log("B");
        transform.position = Input.mousePosition; // translates object to the same position as the player mouse
    }
    public void OnEndDrag(PointerEventData eventData) // activates when dropping dragged item
    {
        Debug.Log("C");
        transform.SetParent(lootParent); // set parent to old slot 
        image.raycastTarget = true;
    }
}
