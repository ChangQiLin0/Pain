using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private Player player;
    public Slider healthSlider;
    public Slider expSlider;
    public TextMeshProUGUI coinCount;
    public TextMeshProUGUI skillPointCount;
    public TextMeshProUGUI playerLevel;
    public Transform floorIndicator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // define player object
    }

    private void Update() 
    {
        HUDComponents();
        UpdatePColour();
    }

    private void HUDComponents()
    {
        healthSlider.maxValue = player.maxHealth; // update max health
        healthSlider.value = player.curHealth; // update current health

        expSlider.maxValue = player.nextReqExp; // update max exp value
        expSlider.value = player.totalExp; // set current exp
        playerLevel.text = "Level: " + player.playerLevel.ToString(); // set player level 
        
        coinCount.text = player.totalCoins.ToString(); // set player coin count
        skillPointCount.text = player.skillPoints.ToString(); // set player skillpoint count
        
    }

    private void UpdatePColour() // update p colors when avaliable
    {
        if (player.currentDungeonFloor != null)
        {
            DungeonManager dungeonManager = player.currentDungeonFloor.GetComponent<DungeonManager>();
            if (dungeonManager.floor >= 5)
            {
                floorIndicator.GetChild(0); // child 0 = cirle object
                floorIndicator.GetChild(1); // child 1 = text
            }
        }
    }
}

