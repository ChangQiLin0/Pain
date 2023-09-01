using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private Animator playerAnimation;

    private bool facingRight = true;
    private Vector2 moveVector;

    private void Start()
    {
        // get componets
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInputs();
        Animation();
    }

    private void HandleInputs()
    {
        // gets horizontal and vertical inputs e.g. wasd or arrow keys
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        // gets direction of where the cursor is
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        // checks for what direction the cursor is to check if flip on y axis for sprites is required
        // TEMP - dir -> moveVector
        if (moveVector.x > 0 && !facingRight || moveVector.x < 0 && facingRight)
        {
            Flip();
        }

    }


    private void Flip()
    {
        facingRight = !facingRight;
        // flips/mirrors sprites on the y axis
        transform.Rotate(0f, 180f, 0f);
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


    private void FixedUpdate() 
    {
        // velocitytemp just a random variable 
        // normalized used to make sure that move speed is constant
        Vector2 velocitytemp = moveVector.normalized * moveSpeed;
        rb.velocity = velocitytemp;
    }
}
