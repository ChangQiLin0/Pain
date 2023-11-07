using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI header;
    public TextMeshProUGUI description;
    public LayoutElement layoutElement;
    private int characterWrapLimit = 50;
    private GameObject player;
    private InventoryUI inventoryUI;
    private bool canDrop;
    private GameObject selectedObject;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // get player object
        inventoryUI = ObtainDefinitions.Instance.InventoryUI;
    }
    private void Update()
    {
        TooltipUpdate();
        StackValueUpdate();
        //GunSlot();     
        if (Input.GetKeyDown(KeyCode.Q) && canDrop && selectedObject != null)
        {
            dropLoot();
        }
    } 

    private void OnDisable()
    {
        OnPointerExit(null); // close/hides all ui elements
    }

    public void OnDrop(PointerEventData eventData) // when item has been dropped onto inv slot
    {
        GameObject droppedObject = eventData.pointerDrag; // gets and stores dropped object on slot
        CollectibleLoot lootComponent = droppedObject.GetComponent<CollectibleLoot>(); // get collecible loot script 
        Transform originalParent = lootComponent.lootParent; // store original parent as temp variable for later use 
        Transform itemInSlot = transform.GetChild(0); // get child object
        
        if (droppedObject != null && (transform.name == "ItemSlot" || transform.name == lootComponent.lootType)) // if dropped place is item  slot or matches loot slot
        {
            if (transform.childCount == 1) // if it has one item it means its empty
            { 
                Debug.Log("dropped onto slot");
                if (transform.name != "ItemSlot")
                {
                    inventoryUI.inventoryCount --; // subtract 1 from inv counter
                }
                else if (originalParent.name != "ItemSlot")
                {
                    inventoryUI.inventoryCount ++; // add 1 to inv counter
                }

                lootComponent.lootParent = transform; // set parent current slot
            }
            else if (transform.childCount == 2 && !lootComponent.isCursed && !transform.GetChild(0).GetComponent<CollectibleLoot>().isCursed) // if item/child is present and both items are not cursed
            {   
                Debug.Log("swapping items");
                string itemInSlotLootType = itemInSlot.GetComponent<CollectibleLoot>().lootType;
                if (itemInSlotLootType == transform.name || itemInSlotLootType == originalParent.name || transform.name == "ItemSlot" || originalParent.name == "ItemSlot") // both items are the same loot type as inv
                {
                    lootComponent.lootParent = transform; // set transform in original script so it doesnt swap back
                    droppedObject.transform.SetParent(transform); // swap with current slot
                    itemInSlot.SetParent(originalParent); // swap with original parent
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount == 2) // only pass if child exists
        {
            layoutElement.gameObject.SetActive(true); // set object to visible
            TooltipContent();

            canDrop = true; // bool set to true when able to drop item
            selectedObject = transform.GetChild(0).gameObject; // set to child object of inventory slot 
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount == 2) // only pass if child exists
        {
            layoutElement.gameObject.SetActive(false); // set object to not visible
            canDrop = false; // set to false, unable to drop item
            selectedObject = null; // set to null to prevent any issues
        }
    }

    private void dropLoot()
    {
        if (transform.name == "ItemSlot" && transform.childCount == 2 && !transform.GetChild(0).GetComponent<CollectibleLoot>().isCursed)
        {
            Instantiate(selectedObject, player.transform.position, Quaternion.identity); // instantiate object into world at player position
            if (inventoryUI.stackedLoot.ContainsKey(selectedObject.name))
            {   
                if (inventoryUI.stackedLoot[selectedObject.name] == 1) // if there is only one in the stack of items
                {
                    inventoryUI.stackedLoot[selectedObject.name] --; // subtract from stack
                    inventoryUI.inventoryCount --; // minus 1 from inventory count
                    inventoryUI.stackedLoot.Remove(selectedObject.name);
                    OnPointerExit(null); // call onpointerexit to hide tooltip
                    Destroy(transform.GetChild(0).gameObject); // delete gameobject from UI
                }
                else
                {
                    inventoryUI.stackedLoot[selectedObject.name] --; // subtract from stack
                }
            }
            else
            {
                OnPointerExit(null); // call onpointerexit to hide tooltip
                Destroy(transform.GetChild(0).gameObject); // delete gameobject from UI
                inventoryUI.inventoryCount --; // minus 1 from inventory count
            }
        }
    }

    private void TooltipContent()
    {
        GameObject getGameObject = transform.GetChild(0).gameObject; // get child component as a gameobject
        CollectibleLoot collectibleComponent = getGameObject.GetComponent<CollectibleLoot>();
        string getLootType = collectibleComponent.lootType; // get loot type which was predefined on the item
        string getLootName = collectibleComponent.lootToBeAdded.name.Replace("(Clone)", "").Trim(); // get loot name/name of gameobject

        header.text = getLootName;
        description.text = ObtainDefinitions.Instance.lootDescription[getLootType]("desc", getGameObject); // create description by calling method in obtain def

        if (transform.name == "ItemSlot" && !collectibleComponent.isCursed) // check if cursed is false
        {
            description.text += "<br><br>Press Q to drop";
        }

        if (collectibleComponent.isCursed) // if cursed is true
        {
            description.text += "<br><br>Cursed";
        }

        if (ObtainDefinitions.Instance.isUseable[getLootName]) // check defenitions to see if loot is useable
        {
            description.text += "<br>Press E to use";
        }

        int headerLength = header.text.Length; // get number of letters/length of header
        int descriptionLength = description.text.Length; // get length of description
        
        if (headerLength > characterWrapLimit || descriptionLength > characterWrapLimit) // if length of either header of description is longer than max width enable text wrapping
        {
            layoutElement.enabled = true; // set layoutelement which controls text wrapping to true
        }
        else
        {
            layoutElement.enabled = false; // disable text wrapping is not applicable 
        }
    }
    private void TooltipUpdate()
    {
        RectTransform tooltipRect = layoutElement.gameObject.transform.GetComponent<RectTransform>(); // get tooltip container
        Vector2 mousePos = Input.mousePosition; // get mouse position
        tooltipRect.position = mousePos; // set tooltips location to where mouse is

        float tooltipHeight = tooltipRect.sizeDelta.y; // get height of tooltip box 
        tooltipRect.position = mousePos; // set tooltip rec to mouse pos
        
        if (mousePos.y - tooltipHeight/1.5 < 0) // check if bottom of tooltip is within the screen or not
        {
            tooltipRect.position = new Vector2(mousePos.x, mousePos.y + tooltipHeight/2); // shift tooltip container up by half to move it not off screen
        }
        
        if (transform.GetChild(0).name == "StackValue" && transform.childCount > 1) // check if sorting order is correct
        {
            transform.GetChild(0).SetAsLastSibling(); // set stackvalue object to last in hierarchy
        }
    }

    private void StackValueUpdate()
    {
        
        int updatedSlotValue = 0; // stores value to be updated with default all stacks to 0
        string inventorySlotValue = "0"; 
        transform.GetChild(0).name = transform.GetChild(0).name.Replace("(Clone)", "").Trim();
        
        if (transform.childCount == 1) // if item slot is empty 
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0"; // set to 0 as there are no items
        }

        if (inventoryUI.stackedLoot.ContainsKey(transform.GetChild(0).name)) // if child is a stackable object
        {
            updatedSlotValue = inventoryUI.stackedLoot[transform.GetChild(0).name]; // get to be update stack size
            inventorySlotValue = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text; // get text for current stack size
        }

        if (transform.childCount == 2 && !inventoryUI.stackedLoot.ContainsKey(transform.GetChild(0).name)) // if child isnt a stackable object
        {
            updatedSlotValue = 1; // stack size must be 1 as it is not stackable
            inventorySlotValue = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text; // get text for stack size
            if (inventorySlotValue == null)
            {
                inventorySlotValue = "0";
            }
        }

        if (inventorySlotValue != updatedSlotValue.ToString() && transform.childCount == 2) // check if stack value is correct
        {
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = updatedSlotValue.ToString();
            if (updatedSlotValue > 1) // compare to check if stack value is greater than 1 
            {
                Debug.Log(transform.GetChild(0).name);
                transform.GetChild(1).gameObject.SetActive(true); // set to visible so values can be seen
            }
        }
        else if (inventorySlotValue == "1") // if stack == 1 set visible to false
        {
            transform.GetChild(1).gameObject.SetActive(false); // set to visible so values can be seen
        }
        else if (transform.childCount == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false); // set visibility to false
        }
    }
}
