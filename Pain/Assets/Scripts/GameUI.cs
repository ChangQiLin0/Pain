using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private Transform itemContainer;
    private Transform itemTemplate;
    public Sprite sprite;

    public void SetInventory(PlayerInventory playerInventory)
    {
        this.playerInventory = playerInventory;
        RefreshInv();
    }

    private void Awake()
    {
        itemContainer = transform.Find("SlotContainer");
        //itemTemplate = itemContainer.Find("itemTemplate");
    }

    private void RefreshInv()
    {   
        Debug.Log(playerInventory.lootList.Count);
        int x = 0;
        int y = 0;
        float cellSize = 100f;
        foreach (GameObject loot in playerInventory.lootList)
        {
            Debug.Log("eeeeeeaaaa");
            //RectTransform itemRectTransform =
            Instantiate(sprite, itemContainer);//.GetComponent<RectTransform>();
            //itemRectTransform.gameObject.SetActive(true);
            //itemRectTransform.anchoredPosition = new Vector2(x * cellSize, y * cellSize);
            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
        
    }
}