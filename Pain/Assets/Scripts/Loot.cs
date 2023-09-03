using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public float dropChance;
    // 0-100 

    public Loot(string lootName, float dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
