using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeScript : MonoBehaviour
{
    private Player player;
    public TextMeshProUGUI skillPointCountText;
    public Button[] skills;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // get player component
        int[] interactableSkill = {0, 2, 4, 5}; // get specific skill to set to interactable via index
        
        // set all buttons which are locked to non interactable
        for (int i = 0; i < skills.Length; i++) // loop through length of 
        {
            if (!interactableSkill.Contains(i)) // if index not in interactable skill
            {
                skills[i].interactable = false; // set buttons to non interactable
            }
        }
    }
    private void Update()
    {
        skillPointCountText.text = player.skillPoints.ToString();
    }

    public void Button0(Button clickedButton) // Leech1
    {
        if (player.skillPoints >= 3) // this skill costs 3 skill points
        {
            clickedButton.interactable = false; // set to non interactable
            clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated
            skills[1].interactable = true; // set leech2 interactability to true
            skills[2].interactable = false; // set regen interactability to false
            skills[2].GetComponent<Image>().color = new Color(1, 0, 0); // color set to green, bought/activated
            skills[3].GetComponent<Image>().color = new Color(1, 0, 0); // color set to green, bought/activated

            player.leechChance = 10; // 10% chance to leech
            player.leechPercent = 10; // gain 10% from leech
            player.skillPoints -= 3; // subtract 3 skill points from player
        }
    }
    public void Button1(Button clickedButton) // Leech2
    {
        if (player.skillPoints >= 6) // this skill costs 6 skill points
        {
            clickedButton.interactable = false; // set to non interactable
            clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated

            player.leechChance = 15; // 15% chance to leech
            player.leechPercent = 20; // gain 20% from leech
            player.skillPoints -= 6; // subtract 6 skill points from player
        }
    }
    public void Button2(Button clickedButton) // Regen1
    {
        if (player.skillPoints >= 3) // this skill costs 2 skill points
        {
            clickedButton.interactable = false; // set to non interactable
            clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated
            skills[3].interactable = true; // set regen2 interactability to true
            skills[0].interactable = false; // set leech interactability to false
            skills[0].GetComponent<Image>().color = new Color(1, 0, 0); // color set to red, locked/unable to be bought
            skills[1].GetComponent<Image>().color = new Color(1, 0, 0); // color set to red, locked/unable to be bought

            player.regenRate = 0.2f; // regen 1 hp per 5 second
            player.skillPoints -= 3; // subtract 2 skill points from player
        }
    }
    public void Button3(Button clickedButton) // Regen2
    {
        if (player.skillPoints >= 4) // this skill costs 4 skill points
        {
            clickedButton.interactable = false; // set to non interactable
            clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated

            player.regenRate = 0.5f; // regen 1 hp per 2 second
            player.skillPoints -= 4; // subtract 4 skill points from player
        }
    }
    





    public void Button4(Button clickedButton) // extra coins
    {
        if (player.skillPoints >= 2) // costs 1 sp
        {
            clickedButton.interactable = false; // set to non interactable
            clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated
            skills[7].interactable = true; // set other one to false

            player.coinMultiplier += 0.25f; // add 25% to coin multi
            player.skillPoints -= 2; // subtract 1 skill points from player
        }
    }

    public void Button5(Button clickedButton) // extra xp
    {
        if (player.skillPoints >= 2) // costs 1 sp
        {
            clickedButton.interactable = false; // set to non interactable
            clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated
            skills[6].interactable = true; // set other one to false

            player.expMultiplier += 0.25f; // add 25% to exp multi
            player.skillPoints -= 2; // subtract 1 skill points from player
        }
    }

    public void Button6(Button clickedButton) // add 100% more exp

    {
        if (player.skillPoints >= 5) // costs 5 sp
        {
            clickedButton.interactable = false; // set to non interactable
            clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated
            skills[7].interactable = false; // set other one to false
            skills[7].GetComponent<Image>().color = new Color(1, 0, 0); // color set to red, locked/unable to be bought

            player.expMultiplier += 1.0f; // add 100% to exp multi
            player.skillPoints -= 5; // subtract 5 skill points from player
        }
    }

    public void Button7(Button clickedButton) // add 100% more coins
    {
        if (player.skillPoints >= 5) // costs 5 sp
        {
            clickedButton.interactable = false; // set to non interactable
            clickedButton.GetComponent<Image>().color = new Color(0, 1, 0); // color set to green, bought/activated
            skills[6].interactable = false; // set other one to false
            skills[6].GetComponent<Image>().color = new Color(1, 0, 0); // color set to red, locked/unable to be bought

            player.coinMultiplier += 1.0f; // add 100% to coin multi
            player.skillPoints -= 5; // subtract 5 skill points from player
        }
    }
}
