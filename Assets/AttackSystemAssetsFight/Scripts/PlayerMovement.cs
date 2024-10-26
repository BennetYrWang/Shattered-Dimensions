using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool isPlayer1;
    public float moveSpeed = 5f;             // Player's horizontal speed
    public float jumpForce = 7f;             // Jump force applied on the y-axis
    public Transform groundCheck;            // Empty game object placed at the player's feet to detect the ground
    public LayerMask groundLayer;            // Define which layers are considered ground

    private Rigidbody2D rb;                  // Reference to the player's Rigidbody2D
    private bool isGrounded;                 // To check if the player is on the ground
    private float groundCheckRadius = 0.2f;

    // Radius to check ground proximity

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();    // Get the Rigidbody2D component attached to the player
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float horizontalInput;
   
         horizontalInput= Input.GetAxis("Horizontal");
       // Get input from "A/D" or left/right arrow keys
        Vector2 velocity = rb.velocity;                      // Get current velocity

        velocity.x = horizontalInput * moveSpeed;            // Set the x velocity according to input
        rb.velocity = velocity;                              // Apply the new velocity

        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);     // Face right
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);    // Face left
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);  // Check if the player is grounded

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)        // Check for spacebar press and if grounded
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Apply jump force on the y-axis
        }
    }

  
   
}
