using Bennet.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bennet.MovementSystem
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Controller Input")]
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode jumpKey;
        public KeyCode downKey;
        private KeyCode pressedControllKey;

        [Header("Movement")]
        public bool inputAllowed = true;
        public float moveSpeed = 5f;
        public float jumpVelocity = 7f;
        public float dashSpeed = 10f;
        
        [Header("Dash State")]
        public bool isDashing = false;
        public bool dashRecovered = true;
        public bool canDashInterrupted = true;
        
        [Header("Dash Values")]
        public float dashDurationSecond = 0.3f;
        public float doubleClickIntervalSecond = 0.2f;
        public float dashRecoverySecond = 1f;
        private Vector2 dashDirection;
        
        //Counters
        private float doubleClickCounter = -1f;
        private float dashRecoveryCounterSecond = -1f;
        private float dashDurationCounterSecond = -1f;
        
        
        public PlayerActor body, illusion;
        
        private bool _dualExistence = true;

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
            UpdateCounters();
            if (inputAllowed)
                UpdateInputs();
            
            float movement = 0;
            if (isDashing)
            {
                movement = illusion.FacingRight ? dashSpeed : -dashSpeed;
            }
            else
            {
                if (Input.GetKeyDown(leftKey))
                    RegisterPressedKey(leftKey);

                if (Input.GetKeyDown(rightKey))
                    RegisterPressedKey(rightKey);

                if (!isDashing)
                {
                    if (Input.GetKey(leftKey))
                    {
                        movement -= moveSpeed;
                        if (_dualExistence)
                            illusion.FacingRight = false;
                    }

                    if (Input.GetKey(rightKey))
                    {
                        movement += moveSpeed;
                        if (_dualExistence)
                            illusion.FacingRight = true;
                    }
                }
            }

            bool moveSucceed = TryMovePlayerHorizontally(movement * Time.deltaTime);
            
            if (isDashing && !moveSucceed && canDashInterrupted)
                StopDashing();

            if (Input.GetKeyDown(jumpKey))
            {
                body.Jump(jumpVelocity);
                if (_dualExistence)
                    illusion.Jump(jumpVelocity);
            }

            void UpdateCounters()
            {
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
            }

            void UpdateInputs()
            {
                
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

        // Dashing
        private void RegisterPressedKey(KeyCode dashKey)
        {
            if (!dashRecovered)
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