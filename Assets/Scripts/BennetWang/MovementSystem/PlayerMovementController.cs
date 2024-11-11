using BennetWang.Util;
using BennetWang.Module.Timer;
using UnityEngine;
using UnityEngine.Serialization;

namespace BennetWang.MovementSystem
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
        
        //Timers
        private Timer doubleClickTimer;
        private Timer dashDurationTimer;
        private Timer dashRecoveryTimer;
        
        
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
        }

        private void UpdateInputs()
        {
            if (Input.GetKeyDown(leftKey))
                RegisterPressedKey(leftKey);

            if (Input.GetKeyDown(rightKey))
                RegisterPressedKey(rightKey);
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
                
                if (doubleClickTimer == null)
                {
                    doubleClickTimer = TimerModule.CreateTimer(
                        doubleClickIntervalSecond,
                        Timer.UpdateType.Update,
                        () =>
                        {
                            pressedControllKey = KeyCode.None;
                            doubleClickTimer = null;
                        });
                }
                else
                {
                    doubleClickTimer.TimeLimit = doubleClickIntervalSecond;
                    doubleClickTimer.Reset();
                    doubleClickTimer.Paused = false;
                }
                return;
            }
            
            // If double pressed, pause the timer and dash
            doubleClickTimer.Paused = true;
            StartDashing();
        }
        
        private void StartDashing()
        {
            print("Dash");
            isDashing = true;
            dashRecovered = false;
            
            dashDurationTimer?.Dispose();
            dashRecoveryTimer?.Dispose();
            
            dashDurationTimer = TimerModule.CreateTimer(dashDurationSecond,Timer.UpdateType.Update,() => isDashing = false);
            dashRecoveryTimer = TimerModule.CreateTimer(dashRecoverySecond, Timer.UpdateType.Update, () => dashRecovered = true);
            
            pressedControllKey = KeyCode.None;
        }

        private void StopDashing()
        {
            isDashing = false;
            dashDurationTimer.Dispose();
        }
    }
}