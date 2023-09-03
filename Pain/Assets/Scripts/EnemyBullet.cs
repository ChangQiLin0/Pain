using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   
    public Rigidbody2D rb;
    public GameObject enemy;
    private float bulletSpeed2;
    private float damage;
    

    void Start()
    {
        // gets values from enemy which is an object
        if (enemy != null)
        {
            damage = enemy.GetComponent<Enemy>().enemyDamage;
            bulletSpeed2 = enemy.GetComponent<Enemy>().bulletSpeed;
        }
    }


    private void FixedUpdate() 
    {   
        // bullet moves based on the speed and will be the same on all fps
        rb.velocity = transform.right * bulletSpeed2;
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        // checks if collision object has an enemy component so it can take damage
        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        // if enemy component exists 
        if (player != null)
        {
            // damage taken can be changed in component - defaulted to 10
            player.TakeDamage(damage);
        }


        Destroy(gameObject);
    
        
    }
}