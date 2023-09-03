using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 200;
    public int totalCoins = 0;
    public float curHealth = 200;
    public float damageMulti = 1;
    public float bulletSpreadAngle = 0;
    public float reloadSpeed = 1;
    public int bonusAmmo = 0;
    public float bonusDamage = 0;
    public float damageMultiplier = 1;
    // higher is better defence default = 1 but can be > 0
    public float defence = 0;
    // higher is better 
    public float defenceMultiplier = 1;
    // lower is better defence | shouldnt be effected that much most by 20%/0.2

    
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
        // checks if collision object has an enemy component so it can take damage
        float enemyBulletDamage = collision.gameObject.GetComponent<EnemyBullet>().damage;
        // if enemy component exists 

    
        // calculates total damage player should be dealt
        float totalDealtDamage = ((enemyBulletDamage - defence) * defenceMultiplier);
            
        // checks if damage is negative as it would heal not damage
        if (totalDealtDamage <= 0)
        {
            TakeDamage(0);
        }
        else
        {
            TakeDamage(totalDealtDamage);
        }
        Debug.Log(collision);
        Debug.Log(totalDealtDamage);
            
        
    }

    


}
