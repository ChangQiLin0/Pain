using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   
    public Rigidbody2D rb;
    public GameObject enemy;
    private float bulletSpeed2;
    public float damage;
    

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
        Destroy(gameObject);
    }
}