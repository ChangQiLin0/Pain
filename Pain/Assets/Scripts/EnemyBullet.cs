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
        rb.velocity = transform.right * bulletSpeed; // bullet moves based on the speed and will be the same on all fps
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        Destroy(gameObject);
    }
}