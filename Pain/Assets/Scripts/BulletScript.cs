using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    public int damage = 10;
    public Rigidbody2D rb;
    

    void FixedUpdate() 
    {   
        // bullet moves based on the speed and will be the same on all fps
        rb.velocity = transform.right * bulletSpeed;
    }

        // if bullet collides with anything with a rigidbody bullet selfdestructs and deals damage
    private void OnCollisionEnter2D (Collision2D collision)
    {
        // checks if collision object has an enemy component so it can take damage
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        // if enemy component exists 
        if (enemy != null)
        {
            // damage taken can be changed in component - defaulted to 10
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
        
    
}
