using UnityEngine;

namespace BennetMovementSystem
{
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

            TryMovePlayerHorizontally(movement);

            if (Input.GetKeyDown(jump))
            {
                body.Jump(jumpVelocity);
                illusion.Jump(jumpVelocity);
            }
        }

        public bool TryMovePlayerHorizontally(float amount)
        {
            if (amount == 0)
                return true;

            if (!body.CanMoveHorizontally(amount) || !illusion.CanMoveHorizontally(amount))
                return false;

            body.ApplyHorizontalMove(amount);
            illusion.ApplyHorizontalMove(amount);
        
            return true;
        }
    }
}