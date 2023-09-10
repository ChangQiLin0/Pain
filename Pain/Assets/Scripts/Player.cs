using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator playerAnimation;
    private bool facingRight = true;
    private Vector2 moveVector; // store player movement input e.g. WASD
    public float totalMoveSpeed; // additional will be done later
    public float maxHealth = 200;
    public float curHealth = 200;
    public float totalCoins = 0;
    public float coinMultiplier = 1; 
    public float playerLevel; // temp
    public float totalExp; // temp
    public float expMultiplier; // temp
    public float bonusDamage;
    public float damageMultiplier = 1; // higher is better default = 1 but can be > 0
    public float bulletSpreadAngle; // DEGREES
    public int bonusAmmo; // bonus ammo for player
    public float defence; // higher = better
    public float defenceMultiplier = 1; // lower = better
    private PlayerInventory playerInventory;
    public GameUI gameUI;
    private GameObject collidedLoot;

     
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // get component from player
        playerAnimation = GetComponent<Animator>(); // get animation
        curHealth = maxHealth; // initiates the player character

        playerInventory = GetComponent<PlayerInventory>(); // create new inventory instance
        Debug.Log("jkdbsafh");
        gameUI.SetInventory(playerInventory); // pass playerinventory into ui script
    }

    private void FixedUpdate()
    {
        HandleInputs();
        Animation();
    }

    private void HandleInputs()
    {
        // gets horizontal and vertical inputs e.g. wasd or arrow keys
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        // Follow cursor
        Vector2 mouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        // checks for what direction the movement is to check if flip on y axis for sprites is required
        // Change moveVector to dir to follow cursor
        
        if (mouseDir.x > 0 && !facingRight || mouseDir.x < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f); // flips/mirrors sprites on the y axis
        }
        rb.velocity = moveVector.normalized * totalMoveSpeed; // apply movement to player
    }

    private void Animation()
    {
        if (moveVector.x == 0)
        {
            playerAnimation.SetBool("walking", false);
        }
        else 
        {
            playerAnimation.SetBool("walking", true);
        }
           
    }

    public void TakeDamage(float damageAmount)
    {
        curHealth -= damageAmount; // deal damage to current health not total
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet")) // check if collision is a enemy bullet by tag
        {
            float enemyBulletDamage = collision.gameObject.GetComponent<EnemyBullet>().damage; // get EnemyBullet script for damage
            float totalDealtDamage = (enemyBulletDamage - defence) * defenceMultiplier; // calculates total damage player should be dealt
            if (totalDealtDamage <= 0) // damage must be above 0 or it will heal the player
            {
                TakeDamage(1); // deal 1 damage as minimum
            }
            else
            {
                TakeDamage(totalDealtDamage); // calls takedamage function with damage as parameter 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Item"))
        {
            collidedLoot = collider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == collidedLoot)
        {
            collidedLoot = collider.gameObject;
        }
        
    }
}
