using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool isInInventory;
    public GameObject inventoryMenuUI;

    private void Awake()
    {
        inventoryMenuUI.SetActive(true);
        inventoryMenuUI.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Pressed");
            if (isInInventory)
            {
                Debug.Log("close");
                CloseInventory();
                inventoryMenuUI.SetActive(false);
                isInInventory = false;
            }
            else
            {
                Debug.Log("Open");
                AccessInventory();
                inventoryMenuUI.SetActive(true);
                isInInventory = true;
            }
        }
    }

    private void AccessInventory()
    {

    }
    private void CloseInventory()
    {
        
    }

}
