using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject inventoryMenuUI; // store menu UI object
    public GameObject pauseMenuUI; // store menu UI object
    public bool isInInventory; // boolean to check is player has inventory open or not
    public bool isPaused; // boolean to check if the game is paused

    private void Awake()
    {
        Debug.Log("AWAKE");
        inventoryMenuUI.SetActive(true); // initialise the menu while game is loading
        inventoryMenuUI.SetActive(false); // deactivate to make sure the game doesnt start with the inventory present
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Tab) && !isPaused) || (Input.GetKeyDown(KeyCode.Escape) && !isPaused && isInInventory)) // check if player input is equal to the tab key and prevents interference 
        {
            if (isInInventory) // close inventory
            {
                inventoryMenuUI.SetActive(false); // set object to inactive which hides it from view 
                Time.timeScale = 1f; // unfreeze game by setting timescale back to 1
                isInInventory = false; // change to match current state which is closing inventory
            }
            else if (!isInInventory) // open inventory
            {
                inventoryMenuUI.SetActive(true); // set object to active so player can view and use the inventory
                Time.timeScale = 0f; // freeze time by setting speed time is passing to 0
                isInInventory = true; // change to show that inventory has been opened 
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isInInventory) // check if input is equal to the escape key and prevents interference 
        {
            if (isPaused) // resume game
            {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f; // unfreeze game by setting timescale back to 1
                isPaused = false;
            }
            else if (!isPaused) // pause game
            {
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f; // freeze time by setting speed time is passing to 0
                isPaused = true;
            }
        }
    }
}
