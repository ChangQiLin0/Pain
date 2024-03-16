using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject deathMenu;
    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f; // pause game as player is already dead
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }

    public void StartGame()
    {
        
    }
}
