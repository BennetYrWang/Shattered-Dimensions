using UnityEngine;
using UnityEngine.Serialization;

namespace Bennet.MovementSystem
{
    public class PlayerMovementController : MonoBehaviour
    {
        [FormerlySerializedAs("toLeft")] public KeyCode toLeftKey;
        [FormerlySerializedAs("toRight")] public KeyCode toRightKey;
        public KeyCode jump;
        public float moveSpeed = 1f;
        public float jumpVelocity = 3f;


        public bool isDashing = false;
        public bool dashRecovered = true;
        public bool canDash => dashRecovered;
        
        public bool canDashInterrupted = false;
        
        public float dashSpeed = 3f;
        public float dashDurationSecond = 0.2f;
        public float doubleClickIntervalSecond = 0.15f;
        private float doubleClickCounter = -1f;
        private float dashRecoveryCounterSecond = -1f;
        private float dashDurationCounterSecond = -1f;
        [FormerlySerializedAs("dashRecoveryTimeSecond")] public float dashRecoverySecond = 1f;
        
        
        public PlayerActor body, illusion;
        
        private bool _dualExistence = true;

        private KeyCode pressedControllKey;

        public bool DualExistence
        {
            get => _dualExistence;
            set
            {
                _dualExistence = value;
                illusion.gameObject.SetActive(value);
            }
        }

        private void Update()
        {
            print(pressedControllKey);
            // Double Click Stuff
            if (dashRecoveryCounterSecond > 0f)
            {
                dashRecoveryCounterSecond -= Time.deltaTime;
                if (dashRecoveryCounterSecond < 0f)
                {
                    dashRecoveryCounterSecond = -1f;
                    dashRecovered = true;
                }
            }

            if (doubleClickCounter > 0f)
            {
                doubleClickCounter -= Time.deltaTime;
                if (doubleClickCounter < 0f)
                {
                    pressedControllKey = KeyCode.None;
                    doubleClickCounter = -1f;
                }
            }

            if (dashDurationCounterSecond > 0f)
            {
                dashDurationCounterSecond -= Time.deltaTime;
                if (dashDurationCounterSecond < 0f)
                {
                    StopDashing();
                }
            }
            
      
            float movement = 0;
            if (isDashing)
            {
                movement = illusion.FacingRight ? dashSpeed : -dashSpeed;
            }
            else
            {
                if (Input.GetKeyDown(toLeftKey))
                    RegisterPressedKey(toLeftKey);

                if (Input.GetKeyDown(toRightKey))
                    RegisterPressedKey(toRightKey);

                if (!isDashing)
                {
                    if (Input.GetKey(toLeftKey))
                    {
                        movement -= moveSpeed;
                        if (_dualExistence)
                            illusion.FacingRight = false;
                    }

                    if (Input.GetKey(toRightKey))
                    {
                        movement += moveSpeed;
                        if (_dualExistence)
                            illusion.FacingRight = true;
                    }
                }
            }

            bool moveSucceed = TryMovePlayerHorizontally(movement * Time.deltaTime);
            if (isDashing && moveSucceed && canDashInterrupted)
                StopDashing();

            if (Input.GetKeyDown(jump))
            {
                body.Jump(jumpVelocity);
                if (_dualExistence)
                    illusion.Jump(jumpVelocity);
            }

        }

        public bool TryMovePlayerHorizontally(float amount)
        {
            if (amount == 0)
                return true;

            if (!body.CanMoveHorizontally(amount) || (_dualExistence && !illusion.CanMoveHorizontally(amount)))
                return false;

            body.ApplyHorizontalMove(amount);
            illusion.ApplyHorizontalMove(amount);

            return true;
        }

        private void RegisterPressedKey(KeyCode dashKey)
        {
            if (!canDash)
                return;
            if (pressedControllKey != dashKey)
            {
                pressedControllKey = dashKey;
                doubleClickCounter = doubleClickIntervalSecond;
                return;
            }

            doubleClickCounter = -1f;
            StartDashing();
        }
        private void StartDashing()
        {
            print("Dash");
            isDashing = true;
            dashRecovered = false;
            dashDurationCounterSecond = dashDurationSecond;
            dashRecoveryCounterSecond = dashRecoverySecond;
            pressedControllKey = KeyCode.None;
        }

        private void StopDashing()
        {
            isDashing = false;
            dashDurationCounterSecond = -1f;
        }
    }
}