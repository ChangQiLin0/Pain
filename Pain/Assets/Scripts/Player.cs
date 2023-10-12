using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator playerAnimation;
    private bool facingRight = true;
    private Vector2 moveVector; // store player movement input e.g. WASD
    public float totalMoveSpeed = 5;
    public float maxHealth = 200;
    public float curHealth = 200;
    public float totalCoins = 0;
    public float coinMultiplier = 1; 
    public float totalExp = 0; // temp
    public float nextReqExp = 30; // base level exp req is 30
    public float expMultiplier = 1; // temp
    public float playerLevel = 1; // temp
    public int skillPoints = 0;
    public float bonusDamage;
    public float damageMultiplier = 1; // higher is better default = 1 but can be > 0
    public float bulletSpreadAngle; // DEGREES
    public int bonusAmmo; // bonus ammo for player
    public float defence; // higher = better
    public float defenceMultiplier = 1; // lower = better
    private PlayerInventory playerInventory;
     
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // get component from player
        playerAnimation = GetComponent<Animator>(); // get animation
        curHealth = maxHealth; // initiates the player character

        playerInventory = GetComponent<PlayerInventory>(); // create new inventory instance
    }

    private void FixedUpdate()
    {
        HandleInputs();
        Animation();
    }

    private void HandleInputs()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal"); // get horizonal inputs e.g. W/S and Up/Down
        moveVector.y = Input.GetAxisRaw("Vertical"); // get vertical inputs e.g. A/D and Left/Right
        rb.velocity = moveVector.normalized * totalMoveSpeed; // apply movement to player

        Vector2 mouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position); // get player cursor position as vector
        if (mouseDir.x > 0 && !facingRight || mouseDir.x < 0 && facingRight) // checks if flipping sprite is required
        {
            facingRight = !facingRight; // set facing right to false (facing left)
            transform.Rotate(0f, 180f, 0f); // flips/mirrors sprites on the y axis
        }
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

    public void LevelUpCalculation()
    {
        Debug.Log("total exp" + totalExp);
        if (totalExp >= nextReqExp)
        {
            Debug.Log(playerLevel + " req: "+ nextReqExp);
            totalExp -= nextReqExp; // keep any extra exp and transfer to next level
            playerLevel += 1; // add one to player level
            skillPoints += Random.Range(1,4);; // add between 1-3 skill points
            nextReqExp = 25 * Mathf.Pow(1.2f, playerLevel); // 25 x 1.2^playerLevel
            Debug.Log(playerLevel + " req: "+ nextReqExp);
        }
    }
}
