using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor.Animations;
using Unity.VisualScripting;
using System.Diagnostics.Tracing;
using UnityEngine.UIElements;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI header;
    public TextMeshProUGUI description;
    public LayoutElement layoutElement;
    private int characterWrapLimit = 50;
    private PlayerInventory playerInventory;
    private bool canDrop;
    private GameObject selectedObject;

    private void Awake()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>(); // get player inventory component
    }
    private void Update()
    {
        TooltipUpdate();
        StackValueUpdate();     
        if (Input.GetKeyDown(KeyCode.Q) && canDrop && selectedObject != null)
        {
            dropLoot();
        }
    } 

    private void OnDisable()
    {
        OnPointerExit(null);
    }

    public void OnDrop(PointerEventData eventData) // when item has been dropped onto inv slot
    {
        if (transform.childCount == 1) // if it has one item it means its empty
        {
            GameObject droppedObject = eventData.pointerDrag; // gets and stores dropped object on slot
            CollectibleLoot lootComponent = droppedObject.GetComponent<CollectibleLoot>(); // get collecible loot script 
            lootComponent.lootParent = transform; // set parent current slot
        }
        else if (transform.childCount == 2 && !eventData.pointerDrag.GetComponent<CollectibleLoot>().isCursed && !transform.GetChild(0).GetComponent<CollectibleLoot>().isCursed) // if item/child is present and both items are not cursed/is able to swap
        {
            GameObject droppedObject = eventData.pointerDrag; // gets and stores dropped object on slot 
            CollectibleLoot lootComponent = droppedObject.GetComponent<CollectibleLoot>(); // get collecible loot script
            Transform originalParent = lootComponent.lootParent; // store original parent as temp variable for later use 
            Transform itemInSlot = transform.GetChild(0); // get child object
            
            lootComponent.lootParent = transform; // set transform in original script so it doesnt swap back
            droppedObject.transform.SetParent(transform); // swap with current slot
            itemInSlot.SetParent(originalParent); // swap with original parent
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount == 2) // only pass if child exists
        {
            layoutElement.gameObject.SetActive(true); // set object to visible
            TooltipContent(eventData);

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
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // get player object
        Instantiate(selectedObject, player.transform.position, Quaternion.identity); // instantiate object into world at player position
        if (playerInventory.stackedLoot.ContainsKey(selectedObject.name))
        {   
            if (playerInventory.stackedLoot[selectedObject.name] == 1) // if there is only one in the stack of items
            {
                Debug.Log("1.5");
                playerInventory.stackedLoot[selectedObject.name] --; // subtract from stack
                OnPointerExit(null); // call onpointerexit to hide tooltip
                Destroy(transform.GetChild(0).gameObject); // delete gameobject from UI
                playerInventory.inventoryCount --; // minus 1 from inventory count
            }
            else
            {
                playerInventory.stackedLoot[selectedObject.name] --; // subtract from stack
            }
        }
        else
        {
            Debug.Log("2");
            OnPointerExit(null); // call onpointerexit to hide tooltip
            Destroy(transform.GetChild(0).gameObject); // delete gameobject from UI
            playerInventory.inventoryCount --; // minus 1 from inventory count
        }
    }

    private void TooltipContent(PointerEventData eventData)
    {
        GameObject getGameObject = transform.GetChild(0).gameObject; // get child component as a gameobject
        string getLootType = getGameObject.GetComponent<CollectibleLoot>().lootType; // get loot type which was predefined on the item
        string getLootName = getGameObject.GetComponent<CollectibleLoot>().lootToBeAdded.name.Replace("(Clone)", "").Trim(); // get loot name/name of gameobject

        header.text = getLootName;
        description.text = ObtainDefinitions.Instance.lootDescription[getLootType]("desc", getGameObject); // create description by calling method in obtain def

        description.text += "<br><br>Press Q to drop";
        if (ObtainDefinitions.Instance.isUseable[getLootName])
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
        Vector2 mousePos = Input.mousePosition; // divide mouse pos by canvas size 
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
        
        int updatedSlotValue = 0; // default all stacks to 0
        transform.GetChild(0).name = transform.GetChild(0).name.Replace("(Clone)", "").Trim();
        string inventorySlotValue = "0";
        if (transform.childCount == 1) // if item slot is empty 
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0"; // set to 0 as there are no items
        }

        if (playerInventory.stackedLoot.ContainsKey(transform.GetChild(0).name))
        {
            updatedSlotValue = playerInventory.stackedLoot[transform.GetChild(0).name]; // get to be update stack size
            inventorySlotValue = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text; // get text for current stack size
        }

        if (transform.childCount == 2 && !playerInventory.stackedLoot.ContainsKey(transform.GetChild(0).name)) // if child isnt a stackable object
        {
            updatedSlotValue = 1; // stack size must be 1 as it is not stackable
            inventorySlotValue = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text; // get text for stack size
            if (inventorySlotValue == null)
            {
                Debug.Log("dkjsbfkhibshidfgbhabsgi");
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
