using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CollectibleLoot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // apply script on all gameobjects that can be loot
    public bool isInWorld; // check if item is on the ground/in the world
    private bool collectible; // if player is hovering over item, become true otherwise false
    private InventoryUI inventoryUI;
    public GameObject lootToBeAdded = null;
    public Transform lootParent = null; // stores original parent of dragged object
    private Image image;

    public string lootType; // inv equipt names e.g. Gun, Trinket, Helmet, Chestplate // try create dropdown menu enum
    public bool isCursed; // if cursed, player shouldnt be able to move/drop the item
    public bool canBeUsed; // if item can be used while in inventory e.g. health potion 
    public bool autoCollect; // whether or not an item should be automatically collected, default is false

    private void Start()
    {
        image = GetComponent<Image>();
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // get player object
        inventoryUI = ObtainDefinitions.Instance.InventoryUI;

        if (autoCollect)
        {
            autoCollect = false;
            lootToBeAdded = gameObject;
            PickUp();
            
        }
    }

    private void Update()
    {
        if (isInWorld && collectible && Input.GetKey(KeyCode.E)) 
        // check if e is pressed, is in the physical world and is collectible 
        {   
            PickUp();
        }
        if (autoCollect)
        {
            PickUp();
            autoCollect = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) // check if collision is an item
        {
            lootToBeAdded = transform.gameObject; // save object in an variable to save
            lootToBeAdded.name = lootToBeAdded.name.Replace("(Clone)", "").Trim(); // remove all (clone) in name and any leading/following spaces
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

    public void PickUp()
    {   
        inventoryUI = ObtainDefinitions.Instance.InventoryUI;
        if (inventoryUI.inventoryCount > 20) // if inventory is full
        {
            Debug.Log(inventoryUI.inventoryCount);
            if (inventoryUI.stackedLoot.ContainsKey(lootToBeAdded.name)) // check if item in player inventory
            {
                inventoryUI.AddLoot(lootToBeAdded); // call method from playerinv class to place into inv
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
            inventoryUI.AddLoot(lootToBeAdded); // call method from playerinv class to place into inv
            if (ObtainDefinitions.Instance.isStackable[lootToBeAdded.name]) // only destory item is is stackable and is already in inventory
            {
                if (inventoryUI.stackedLoot[lootToBeAdded.name] > 2) // needs to have atleast one in the world before deleting
                {
                    Debug.Log("destory 2"+ inventoryUI.stackedLoot[lootToBeAdded.name]);
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
        if (!isCursed)
        {
            lootParent = transform.parent; // gets parent of object which is the slot container and stores it as parent
            transform.SetParent(transform.root); // set parent as the highest point in the heirarchy
            transform.SetAsLastSibling(); // places it at the top of the hierarchy for dragging layer
            image.raycastTarget = false;
        } 
    }
    public void OnDrag(PointerEventData eventData) // activates when actively dragging
    {
        if (!isCursed)
        {
            transform.position = Input.mousePosition; // translates object to the same position as the player mouse
        }
        
    }
    public void OnEndDrag(PointerEventData eventData) // activates when dropping dragged item
    {
        if (!isCursed)
        {
            transform.SetParent(lootParent); // set parent to old slot 
            image.raycastTarget = true;
        }
    }
}
