using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   
    public Rigidbody2D rb;
    public GameObject enemy;
    public float bulletSpeed;
    public float damage;
    
    private void FixedUpdate() 
    {   
        // bullet moves based on the speed and will be the same on all fps
        rb.velocity = transform.right * bulletSpeed;
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        Destroy(gameObject);
    }
}