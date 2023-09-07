using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    public float gunDamage;
    public Rigidbody2D rb;
    

    void FixedUpdate() 
    {   
        rb.velocity = transform.right * bulletSpeed; // bullet moves right based on the speed
    }

        // if bullet collides with anything with a rigidbody bullet selfdestructs and deals damage
    private void OnCollisionEnter2D (Collision2D collision)
    {
        // checks if collision object has an enemy component so it can take damage
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            // if enemy component exists 
            if (enemy != null)
            {
                // damage taken can be changed in component - defaulted to 10
                enemy.TakeDamage(gunDamage);
            }
        }
        Destroy(gameObject);
    }
}
