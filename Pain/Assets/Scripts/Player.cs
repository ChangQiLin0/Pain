using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator playerAnimation;
    private bool facingRight = true;
    private Vector2 moveVector; // store player movement input e.g. WASD
    public float totalMoveSpeed = 5f;
    public float maxHealth = 200f;
    public float curHealth = 200f;
    public float totalCoins = 0f;
    public float coinMultiplier = 1f; 
    public float totalExp = 0f; // temp
    public float nextReqExp = 24f; // base level exp req is 24 (20 * 1.2^1 = 24)
    public float expMultiplier = 1f; // temp
    public float playerLevel = 1f; // temp
    public int skillPoints = 0;
    public float bonusDamage = 0f;
    public float damageMultiplier = 1f; // higher is better default = 1 but can be > 0
    public float bulletSpreadAngle; // in degrees
    public int bonusAmmo; // bonus ammo for player
    public float defence; // higher = better
    public float defenceMultiplier = 1; // lower = better
    public int leechChance = 0; // chance to leech health from enemy 0-100
    public int leechPercent = 0; // percentage of health leeched 0-100
    public float regenRate = 0f; // nature regeneration of hp for player
     
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // get component from player
        playerAnimation = GetComponent<Animator>(); // get animation
        curHealth = maxHealth; // initiates the player character
    }

    private void FixedUpdate()
    {
        HandleInputs();
        PlayerAnimation();
        HealthRegen();
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

    private void PlayerAnimation()
    {
        if (moveVector.normalized == new Vector2(0,0)) // if move vector = 0, player is not moving
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

    public void Heal(float healValue)
    {
        curHealth += healValue; // add healvalue to total health
        if (curHealth > maxHealth) // check if healed amount is greater than max health
        {
            curHealth = maxHealth; // set curhealth to maxhealth if surpassed
        }
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
        if (totalExp >= nextReqExp)
        {
            totalExp -= nextReqExp; // keep any extra exp and transfer to next level
            playerLevel += 1; // add one to player level
            skillPoints ++; // add 1 skill point to total
            nextReqExp = 20 * Mathf.Pow(1.2f, playerLevel); // 20 x 1.2^playerLevel
        }
    }

    public void HealthRegen()
    {
        if (regenRate > 0 && curHealth < maxHealth) // if regen is greater than 0 and curhealth is less than max health
        {
            Heal(regenRate * Time.fixedDeltaTime);
        }
    }
}
