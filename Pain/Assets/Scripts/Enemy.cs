using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // organise later
    [SerializeField] private Transform barrel;
    [SerializeField] private GameObject bulletPrefab;
    public float health;
    public float enemyDamage;
    public float bulletSpeed;
    public float enemySpeed;

    public float bulletCount;
    public float fireRate;
    public float lookRadius;
    public float attackRadius;
    public float bulletSpreadAngle;
    public LayerMask whatIsPlayer;
    private Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Vector3 dir;

    private bool inLookRange;
    private bool inAttackRange = true;
    private float fireTimer;
    public bool hasShotgun;
    private bool attackPlayer;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // sets the player as the target
        target = GameObject.FindWithTag("Player").transform;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        // checks if collision object is a player by check tags
        {
            // stops enemy from drifting when touched by the player
            rb.velocity = Vector2.zero;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            lookRadius = 1000;
            attackRadius += 0;
        }
        // stops enemy from drifting
        rb.velocity = Vector2.zero;
        // gets playerstats inorder to be able to call TakeDamage function
        PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(enemyDamage);
        }

        
    }

    void FixedUpdate()
    {
        fireTimer += Time.fixedDeltaTime;
        
        if (fireTimer >= fireRate && attackPlayer)
        {
            if (hasShotgun)
                // creates 3 bullets one in centre and 2 off axis
                {

                GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);
                Rigidbody2D rb1 = bullet.GetComponent<Rigidbody2D>();
                GameObject leftBullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation * (Quaternion.Euler(0f, 0f, -30f)));
                Rigidbody2D rb2 = leftBullet.GetComponent<Rigidbody2D>();
                GameObject rightBullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation* (Quaternion.Euler(0f, 0f, 30f)));
                Rigidbody2D rb3 = rightBullet.GetComponent<Rigidbody2D>();
                
                }
            else
            {
                // creates a bullet at the position of the barrel 
                GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);
                Rigidbody2D rb4 = bullet.GetComponent<Rigidbody2D>();
            }

            fireTimer = 0f;
        }
    }

    private void Update()
    {
        // creates a circle at centre with radius of look/attack and if player is in radius 
        inLookRange = Physics2D.OverlapCircle(transform.position, lookRadius, whatIsPlayer);
        inAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

        float distance = Vector2.Distance(transform.position, target.position);

        // checks if player is in range so it can move
        if (distance <= lookRadius && distance >= attackRadius)
        {
            // if in range get direction of player then normlize to prevent excess speed
            Vector2 dir = target.position - transform.position;
            dir.Normalize();
            // moves enemy towards player 
            transform.Translate(dir * enemySpeed * Time.deltaTime);
            attackPlayer = true;
        }
        else
        {
            // prevents drifting of enemy
            rb.velocity = Vector2.zero;
        }
        // creates a circle with half radius of look 
        // check if enemy is in area and has been attack
        Collider2D[] surroundingEnemys = Physics2D.OverlapCircleAll(transform.position, (lookRadius/2), LayerMask.GetMask("Enemy"));
        // checks all items in list and runs the following code
        foreach (Collider2D surroundingEnemy in surroundingEnemys)
        {
            // gets the Enemy script from each enemy
            Enemy enemyData = surroundingEnemy.gameObject.GetComponent<Enemy>();

            if (enemyData != null)
            {
                if (enemyData.attackPlayer)
                {
                    // set radius to infinite to prevent players from running away
                    lookRadius = 1000;
                }
            }
        }
        
        
    }



    public void TakeDamage(int damage)
    {
        // subtracts health by damage
        health -= damage;
        // checks if health is below 0
        if (health <= 0)
        {
            // if below zero die
            Die();
        }
    }
    public void Die()
    {
        // when enemy dead check loottable and drop loot if lucky
        GetComponent<LootTable>().InstantiateLoot(transform.position);

        // destroys gameobject which enemy when health <= 0
        Destroy(gameObject);
    }


}
