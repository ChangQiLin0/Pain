using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 200;
    public float curHealth;
    public float damageMulti;
    public float bulletSpreadAngle;
    public float reloadSpeed;
    public float reloadMultiplier;
    public int bonusAmmo;

    
    private void Start()
    {
        curHealth = maxHealth;
    }
    public void TakeDamage(float damageAmount)
    {
        // current health subtracted by damage to
        curHealth -= damageAmount;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    


}
