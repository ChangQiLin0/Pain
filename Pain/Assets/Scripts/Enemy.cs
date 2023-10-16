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

    public float fireRate;
    public float lookRadius;
    public float attackRadius;
    public float bulletSpreadAngle;
    // shotgun only
    public LayerMask whatIsPlayer;
    private Player player;
    private Transform target;
    private Rigidbody2D rb;
    public Vector3 dir;
    private float fireTimer;
    public bool hasShotgun;
    private bool attackPlayer;
    private bool isMoving;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform; // sets the player as the target
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) // seperate if statements do not combine
        {
            lookRadius = 1000; // sets look radius to infinite
        }
    }

    void FixedUpdate()
    {
        rb.velocity = Vector2.zero; // prevents enemies from drifting when collided 

        fireTimer += Time.fixedDeltaTime; 
        if (fireTimer >= fireRate && attackPlayer && !isMoving)
        {
            if (hasShotgun) // boolean - checks if is it a shotgunner
            {
                for (float i = -30; i < 31; i+=30) // i = -30, 0 and 30
                    {
                    GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation * Quaternion.Euler(0f, 0f, i)); // create bullet at barrel with rotation i
                    bullet.GetComponent<EnemyBullet>().damage = enemyDamage; // get bullet component to sets damage and speed
                    bullet.GetComponent<EnemyBullet>().bulletSpeed = bulletSpeed;
                    }
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation); // creates a bullet at the position of the barrel 
                bullet.GetComponent<EnemyBullet>().damage = enemyDamage;
                bullet.GetComponent<EnemyBullet>().bulletSpeed = bulletSpeed;
            }

            fireTimer = 0f; // always set timer back to 0
        }
    }

    private void Update()
    {
        EnemyMovement();
        EnemyAiming();
    }
    private void EnemyMovement()
    {
        float distance = Vector2.Distance(transform.position, target.position); // calculated distance from player to enemy
        
        if (distance <= lookRadius && distance >= attackRadius) // checks if player is in lookrange and not in attack range
        {
            Vector2 dir = target.position - transform.position; // calculate direct of player in the form of a vector 
            transform.Translate(dir.normalized * enemySpeed * Time.deltaTime); // moves enemy towards player 
            isMoving = true; // set both bool to true
            attackPlayer = true;
        }
        else if (distance <= attackRadius) // if player is within attack radius
        {
            isMoving = false; // set both bool to false
            attackPlayer = true;
        }
        
        Collider2D[] surroundingEnemys = Physics2D.OverlapCircleAll(transform.position, lookRadius/2, LayerMask.GetMask("Enemy")); // creates a circle with half radius of look radius and place all nearby enemies in a list
        foreach (Collider2D surroundingEnemy in surroundingEnemys) // checks all items in list and runs the following code
        {
            Enemy enemyData = surroundingEnemy.gameObject.GetComponent<Enemy>(); // gets the Enemy script from each enemy
            if (enemyData != null)
            {
                if (enemyData.attackPlayer) // bool to check is enemy should attack player 
                {
                    lookRadius = 1000; // set radius to infinite to prevent players from running away
                }
            }
        }
    }

    void EnemyAiming() // handle enemy aiming
    {   
        Transform gunHolder = transform.Find("EnemyWeaponHolder"); // find weapon container/holder
        Vector2 dir = target.position - gunHolder.position; // get direction of player 
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // calculates angle and converts to degrees using mathf.rad2deg
        gunHolder.eulerAngles = new Vector3(0, 0, angle); // apply angle to object
        
        Vector3 localScale = Vector3.one;// flip gun when past y axis
        if (angle > 89 || angle < -89) // use 89 to prevent both flips interfering 
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }
        // flips based on localScale
        gunHolder.localScale = localScale; // apply flip to object

    }

    public void TakeDamage(float damage)
    {
        health -= damage; // subtracts health by damage
        if (player.leechChance > 0) // if leech chance is greater than 0
        {
            
            int rng = Random.Range(0, 101); // random number generator from 0 to 100 inclusive
            if (player.leechChance > rng) // if random number is less than leechchance
            {
                player.Heal(damage * (player.leechPercent/100)); // healValue = damage dealt by player / leech percent
            }
        }
        if (health <= 0) // checks if health is below 0
        {
            
            Die(); // if below zero die
        }
    }
    public void Die()
    {
        GetComponent<LootTable>().GetDroppedLoot(); // check if enemy is lootable/has LootTable script
        Destroy(gameObject); // remove enemy from existence
    }
}
