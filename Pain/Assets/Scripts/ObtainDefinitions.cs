using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainDefinitions : MonoBehaviour
{
    // store all dictionary/sprites required 
    public static ObtainDefinitions Instance { get; private set; }
    

    public Dictionary<string, float> dropChance = new Dictionary<string, float>
    {
        {"Coin", 100f},
        {"gun0", 100f},
        {"TestLoot0", 100f}
    };

    public Dictionary<string, bool> isStackable = new Dictionary<string, bool> // pre defined dictionary of all stackable items
    {
        {"gun0", false},
        {"TestLoot0", true},
        {"cheese", true}
    };

    public GameObject gun0;
    public GameObject testLoot0;

    private void Awake()
    {
        Instance = this;
    }
}
