using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject inventoryMenuUI; // store menu UI object
    public GameObject pauseMenuUI; // store menu UI object
    public GameObject HUD; // store hud menu reference
    public GameObject skillTreeUI; // store skill tree reference
    public bool isInInventory; // boolean to check is player has inventory open or not
    public bool isPaused; // boolean to check if the game is paused
    public bool inMenu; // if player is in ANY menu e.g. inv, shop, skilltree etc
    public bool inSkillTree; // check if player is in skilltree

    private void Awake()
    {
        Debug.Log("AWAKE");
        inventoryMenuUI.SetActive(true); // initialise the menu while game is loading
        inventoryMenuUI.SetActive(false); // deactivate to make sure the game doesnt start with the inventory present
    }
    private void Update()
    {
        OpenCloseMenu();
        manageSkillTree();
    }
    private void OpenCloseMenu()
    {
        if ((Input.GetKeyDown(KeyCode.Tab) && !isPaused) || (Input.GetKeyDown(KeyCode.Escape) && !isPaused && isInInventory)) // check if player input is equal to the tab key and prevents interference 
        {
            if (isInInventory) // close inventory
            {
                inMenu = false;
                inventoryMenuUI.SetActive(false); // set object to inactive which hides it from view 
                Time.timeScale = 1f; // unfreeze game by setting timescale back to 1
                isInInventory = false; // change to match current state which is closing inventory
            }
            else if (!isInInventory && !inMenu) // open inventory if not in a menu
            {
                inMenu = true;
                inventoryMenuUI.SetActive(true); // set object to active so player can view and use the inventory
                Time.timeScale = 0f; // freeze time by setting speed time is passing to 0
                isInInventory = true; // change to show that inventory has been opened 
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isInInventory) // check if input is equal to the escape key and prevents interference 
        {
            if (isPaused) // resume game
            {
                inMenu = false;
                pauseMenuUI.SetActive(false); // hide pause menu
                HUD.SetActive(true); // show HUD
                Time.timeScale = 1f; // unfreeze game by setting timescale back to 1
                isPaused = false;
            }
            else if (!isPaused && !inMenu) // pause game
            {
                inMenu = true;
                pauseMenuUI.SetActive(true); // show pause menu
                HUD.SetActive(false); // hide HUD
                Time.timeScale = 0f; // freeze time by setting speed time is passing to 0
                isPaused = true;
            }
        }
    }

    public void manageSkillTree()
    {
       if ((Input.GetKeyDown(KeyCode.P) && !inMenu) || (Input.GetKeyDown(KeyCode.P) && inMenu && inSkillTree)) // if E key is pressed
       {
        Debug.Log("skillTree");
            if (inSkillTree) // close skill tree
            {
                skillTreeUI.SetActive(false); // set UI element to inactive
                inSkillTree = false;
                inMenu = false;
                Time.timeScale = 1f; // resume time
            }
            else if (!inSkillTree) // open skill tree
            {
                skillTreeUI.SetActive(true); // set UI element to active
                inSkillTree = true;
                inMenu = true;
                Time.timeScale = 0f; // pause time
            }
       }
    }
}
