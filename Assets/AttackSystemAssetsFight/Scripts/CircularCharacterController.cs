using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularCharacterController : MonoBehaviour
{
    public Transform circleCenter;  // Center of the circle
    public float circleRadius = 5f; // Radius of the circle
    public float moveSpeed = 5f;    // Speed of movement
    public float jumpForce = 10f;   // Jumping force
    public float gravityForce = 9.8f; // Gravity pulling away from the center

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveCharacter();
        ApplyCircularGravity();
        HandleJump();
    }

    // Calculate and apply movement tangential to the circle
    void MoveCharacter()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Get input (left/right)
        Vector2 toCenter = (circleCenter.position - transform.position).normalized; // Vector to the center
        Vector2 tangent = new Vector2(-toCenter.y, toCenter.x); // Perpendicular to the center vector (tangent)

        // Move along the tangent (sideways movement)
        rb.velocity = tangent * moveInput * moveSpeed;
    }

    // Apply radial gravity pulling away from the center of the circle
    void ApplyCircularGravity()
    {
        Vector2 toCenter = (circleCenter.position - transform.position).normalized;
        rb.AddForce(-toCenter * gravityForce); // Pull away from the center
    }

    // Handle jumping
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && IsNearCircleWall())
        {
            Vector2 jumpDirection = (transform.position - circleCenter.position).normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse); // Jump in the outward direction
        }
    }

    // Check if the character is near the wall of the circle (within a small threshold)
    bool IsNearCircleWall()
    {
        float distanceToCenter = Vector2.Distance(transform.position, circleCenter.position);
        return Mathf.Abs(distanceToCenter - circleRadius) < 0.1f; // Adjust threshold as needed
    }
}
