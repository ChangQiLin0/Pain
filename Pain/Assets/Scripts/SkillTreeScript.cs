using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeScript : MonoBehaviour
{
    private Player player;
    public Button[] skills;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // get player component
        int[] interactableSkill = {0, 2}; // get specific skill to set to interactable via index
        
        // set all buttons which are locked to non interactable
        for (int i = 0; i < skills.Length; i++) // loop through length of 
        {
            Debug.Log(skills[i]);
            if (!interactableSkill.Contains(i)) // if index not in interactable skill
            {
                skills[i].interactable = false; // set buttons to non interactable
            }
        }
    }

    public void Button0(Button clickedButton) // Leech1
    {
        Debug.Log(clickedButton.name);
        clickedButton.interactable = false; // set to non interactable
        clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated
        skills[2].interactable = true; // set follow up skill to active

        player.leechChance = 5; // 5% chance to leech
        player.leechPercent = 10; // gain 10% from leech
    }
    public void Button1(Button clickedButton) // Leech2
    {
        Debug.Log(skills[1].name);
    }
    public void Button2(Button clickedButton) // Regen1
    {
        Debug.Log(skills[2].name);
    }
    public void Button3(Button clickedButton) // Regen2
    {
        Debug.Log(skills[3].name);
    }
    
}
