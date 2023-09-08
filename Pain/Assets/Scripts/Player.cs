using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator playerAnimation;
    private bool facingRight = true;
    public Vector2 moveVector; // store player movement vector 
    public float totalMoveSpeed; // additional will be done later
    public float maxHealth = 200;
    public float curHealth = 200;
    public int totalCoins = 0;
    public float coinMultiplier = 1; 
    public float bonusDamage;
    public float damageMultiplier = 1; // higher is better default = 1 but can be > 0
    public float bulletSpreadAngle; // DEGREES
    public int bonusAmmo; // bonus ammo for player
    public float defence; // higher = better
    public float defenceMultiplier = 1; // lower = better
    
    private Vector3 mousePos;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // get component from player
        playerAnimation = GetComponent<Animator>(); // get animation
        curHealth = maxHealth; // initiates the player character 
    }

    private void FixedUpdate()
    {
        Animation();
        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context) // public as it needs to be found in menu
    {
        moveVector = context.ReadValue<Vector2>(); // get input from player as vector2 
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>(); // can only be vector2 not 3
    }

    private void MovePlayer() // seperate method to be called in fixedupdate
    {
        Vector2 mouseDir = mousePos - Camera.main.WorldToScreenPoint(transform.position); // get mouse direction

        // checks for what direction the movement is to check if flip on y axis for sprites is required
        if (mouseDir.x > 0 && !facingRight || mouseDir.x < 0 && facingRight)
        {
            facingRight = !facingRight; // notify that it is facing left 
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
}
