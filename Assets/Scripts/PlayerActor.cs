using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerActor : MonoBehaviour
{
    public float gravityScale;
    public int dimensionIndex = 1;
    
    protected Rigidbody2D rb;
    public GravityDirection gravityDirection = GravityDirection.Downward;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        // Apply gravity only
        Vector2 direction = Vector2.zero;
        switch (gravityDirection)
        {
            case GravityDirection.Downward:
                direction = Vector2.down;
                rb.rotation = 0f;
                break;
            case GravityDirection.Leftward:
                direction = Vector2.left;
                rb.rotation = -90f;
                break;
            case GravityDirection.Rightward:
                direction = Vector2.right;
                rb.rotation = 90f;
                break;
            case GravityDirection.Upward:
                direction = Vector2.up;
                rb.rotation = 180f;
                break;
        }

        Vector2 gravityForce = direction * (gravityScale * Physics.gravity.magnitude * rb.mass);
        rb.AddForce(gravityForce, ForceMode2D.Force);
    }

    public void ApplyHorizontalMove(float forwardVelocity)
    {
        switch (gravityDirection)
        {
            case GravityDirection.Downward:
                rb.velocity = new Vector2(forwardVelocity,rb.velocity.y);
                break;
            case GravityDirection.Leftward:
                rb.velocity = new Vector2(rb.velocity.x,-forwardVelocity);
                break;
            case GravityDirection.Rightward:
                rb.velocity = new Vector2(rb.velocity.x, forwardVelocity);
                break;
            case GravityDirection.Upward:
                rb.velocity = new Vector2(-forwardVelocity,rb.velocity.y);
                break;
        }
    }
    public bool CanMoveHorizontally(float forwardSpeed)
    {
        Vector2 forwardDirection = Vector2.down;
        
        switch (gravityDirection)
        {
            case GravityDirection.Downward:
                forwardDirection = new Vector2(1, 0);
                break;
            case GravityDirection.Upward:
                forwardDirection = new Vector2(-1, 0);
                break;
            case GravityDirection.Leftward:
                forwardDirection = new Vector2(0, -1);
                break;
            case GravityDirection.Rightward:
                forwardDirection = new Vector2(0, 1);
                break;
        }

        forwardDirection *= Mathf.Sign(forwardSpeed);
        
        List<RaycastHit2D> hits = new (5);
        Debug.DrawLine(transform.position, forwardDirection + (Vector2)transform.position);
        rb.Cast(forwardDirection,hits,.1f);
        
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("LandScape"))
                return false;

        }
        
        return true;
    }

    public void StopMoveHorizontally()
    {
        switch (gravityDirection)
        {
            case GravityDirection.Downward:
            case GravityDirection.Upward:
                rb.velocity = new Vector2(0,rb.velocity.y);
                break;
            case GravityDirection.Leftward:
            case GravityDirection.Rightward:
                rb.velocity = new Vector2(rb.velocity.x,0);
                break;
        }
    }
    
    public void Jump(float jumpVelocity)
    {
        switch (gravityDirection)
        {
            case GravityDirection.Downward:
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                break;
            case GravityDirection.Upward:
                rb.velocity = new Vector2(rb.velocity.x, -jumpVelocity);
                break;
            case GravityDirection.Leftward:
                rb.velocity = new Vector2(jumpVelocity,rb.velocity.y);
                break;
            case GravityDirection.Rightward:
                rb.velocity = new Vector2(-jumpVelocity,rb.velocity.y);
                break;
        }
    }

    public enum GravityDirection
    {
        Downward, Upward, Leftward, Rightward
    }
}