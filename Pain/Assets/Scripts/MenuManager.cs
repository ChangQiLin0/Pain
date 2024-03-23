using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject inventoryMenuUI; // store menu UI object
    public GameObject pauseMenuUI; // store menu UI object
    public GameObject HUD; // store hud menu reference
    public GameObject skillTreeUI; // store skill tree reference
    public GameObject deathMenu; // store death menu ui
    public GameObject startMenu; // store start menu ui
    public GameObject tutorialMenu; // store tutorial ui
    public GameObject player;
    public bool isInInventory; // boolean to check is player has inventory open or not
    public bool isPaused; // boolean to check if the game is paused
    public bool inMenu; // if player is in ANY menu e.g. inv, shop, skilltree etc
    public bool inSkillTree; // check if player is in skilltree
    public bool inStartMenu = true;
    public bool inTutorial = false;

    private void Awake()
    {
        inventoryMenuUI.SetActive(true); // initialise the menu while game is loading
        inventoryMenuUI.SetActive(false); // deactivate to make sure the game doesnt start with the inventory present

        OpenStartMenu(); // pause game at start (start menu is active)
    }
    private void Update()
    {
        OpenCloseMenu();
        ManageSkillTree();
        OpenDeathMenu();
    }

    public void OpenTutorialMenu()
    {
        tutorialMenu.SetActive(true);
        inTutorial = true;
    }

    public void CloseTutorialMenu()
    {
        tutorialMenu.SetActive(false);
        inTutorial = false;
    }

    public void CloseStartMenu()
    {
        Time.timeScale = 1f;
        startMenu.SetActive(false);
        isPaused = false;
        inMenu = false;
        inStartMenu = false;
    }

    public void OpenStartMenu()
    {
        Time.timeScale = 0f;
        startMenu.SetActive(true);
        isPaused = true;
        inMenu = true;
        inStartMenu = true;
    }
    private void OpenDeathMenu()
    {
        if (player.GetComponent<Player>().curHealth <= 0) // if player health is less than or equal to 0
        {
            Time.timeScale = 0f; // pause game as player is already dead
            deathMenu.SetActive(true); // show death menu UI options
        }
    }

    public void UnpauseGame()
    {
        inMenu = false;
        pauseMenuUI.SetActive(false); // hide pause menu
        HUD.SetActive(true); // show HUD
        Time.timeScale = 1f; // unfreeze game by setting timescale back to 1
        isPaused = false;
    }

    private void OpenCloseMenu()
    {
        if (((Input.GetKeyDown(KeyCode.Tab) && !isPaused) || (Input.GetKeyDown(KeyCode.Escape) && !isPaused && isInInventory)) && !inTutorial) // check if player input is equal to the tab key and prevents interference 
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
        else if (Input.GetKeyDown(KeyCode.Escape) && !isInInventory && !inStartMenu && !inTutorial) // check if input is equal to the escape key and prevents interference 
        {
            if (isPaused) // resume game
            {
                UnpauseGame();
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

    public void ManageSkillTree()
    {
       if (((Input.GetKeyDown(KeyCode.T) && !inMenu) || (Input.GetKeyDown(KeyCode.T) && inMenu && inSkillTree)) && !inTutorial) // if E key is pressed
       {
        Debug.Log("skillTree");
            if (inSkillTree) // close skill tree
            {
                skillTreeUI.SetActive(false); // set UI element to inactive
                HUD.SetActive(true); // activate HUD
                inSkillTree = false;
                inMenu = false;
                Time.timeScale = 1f; // resume time
            }
            else if (!inSkillTree) // open skill tree
            {
                skillTreeUI.SetActive(true); // set UI element to active
                HUD.SetActive(false); // hide HUD
                inSkillTree = true;
                inMenu = true;
                Time.timeScale = 0f; // pause time
            }
       }
    }
}
