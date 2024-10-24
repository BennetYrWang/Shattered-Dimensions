using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public KeyCode toLeft;
    public KeyCode toRight;
    public KeyCode jump;
    public float moveSpeed = 1f;
    public float jumpVelocity = 3f;
    public PlayerActor body, illusion;

    private void Update()
    {
        float movement = 0;
        if (Input.GetKey(toLeft))
            movement -= moveSpeed;
        if (Input.GetKey(toRight))
            movement += moveSpeed;
        
        
        
        if (movement != 0)
        {
            if (body.CanMoveHorizontally(movement) && illusion.CanMoveHorizontally(movement))
            {
                body.ApplyHorizontalMove(movement);
                illusion.ApplyHorizontalMove(movement);
            }
            else
            {
                body.StopMoveHorizontally();
                illusion.StopMoveHorizontally();
            }
        }

        if (Input.GetKeyDown(jump))
        {
            body.Jump(jumpVelocity);
            illusion.Jump(jumpVelocity);
        }
    }
}