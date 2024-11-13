using BennetWang.Util;
using BennetWang.Module.Timer;
using UnityEngine;
using UnityEngine.Serialization;

namespace BennetWang.MovementSystem
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Controller Input")]
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode jumpKey = KeyCode.W;
        private KeyCode pressedControllKey;

        [Header("Movement")]
        public bool inputAllowed = true;
        public float moveSpeed = 5f;
        public float jumpVelocity = 7f;
        public float dashSpeed = 10f;
        private bool prevIsMoving = false;
        
        [Header("Dash State")]
        public bool isDashing = false;
        public bool dashRecovered = true;
        public bool canDashInterrupted = true;
        
        [Header("Dash Values")]
        public float dashDurationSecond = 0.3f;
        public float doubleClickIntervalSecond = 0.2f;
        public float dashRecoverySecond = 1f;
        private bool dash2Right;
        
        [Header("References")]
        public PlayerActor body, illusion;
        [SerializeField] private TrailRenderer bodyTrail, illusionTrail;
        
        //Timers
        private Timer doubleClickTimer;
        private Timer dashDurationTimer;
        private Timer dashRecoveryTimer;
        
        
        public delegate void OnMoveStart();

        public event OnMoveStart onMoveStart;
        public delegate void OnMoveEnd();
        public event OnMoveEnd onMoveStop;
        
        
        
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
            if (!inputAllowed)
                return;
            
            if (!isDashing)
                CheckDoubleClick();

            if (isDashing)
                UpdateDashing();
            else
                UpdateMoveInputs();
        }

        private void CheckDoubleClick()
        {
            if (Input.GetKeyDown(leftKey))
                RegisterPressedKey(leftKey);

            if (Input.GetKeyDown(rightKey))
                RegisterPressedKey(rightKey);
        }
        
        private void UpdateMoveInputs()
        {
            float horizontalMove = 0;
            CalculateHorizontalMove();

            if (horizontalMove != 0 && body.CanMoveHorizontally(horizontalMove) && illusion.CanMoveHorizontally(horizontalMove))
            {
                body.ApplyHorizontalMove(horizontalMove);
                illusion.ApplyHorizontalMove(horizontalMove);
                if (!prevIsMoving)
                {
                    onMoveStart?.Invoke();
                    prevIsMoving = true;
                }
            }
            else
            {
                if (prevIsMoving)
                {
                    onMoveStop?.Invoke();
                    prevIsMoving = false;
                }
            }

            if (Input.GetKeyDown(jumpKey))
            {
                body.Jump(jumpVelocity);
                illusion.Jump(jumpVelocity);
            }
            
            void  CalculateHorizontalMove()
            {
                if (Input.GetKey(leftKey))
                {
                    horizontalMove -= moveSpeed;
                    if (_dualExistence)
                        illusion.FacingRight = false;
                }

                if (Input.GetKey(rightKey))
                {
                    horizontalMove += moveSpeed;
                    if (_dualExistence)
                        illusion.FacingRight = true;
                }

                horizontalMove *= Time.deltaTime;
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

            if (dashKey == leftKey)
                dash2Right = false;
            else if (dashKey == rightKey)
                dash2Right = true;
            
            StartDashing();
        }
        
        private void StartDashing()
        {
            print("Dash");
            isDashing = true;
            dashRecovered = false;

            bodyTrail.enabled = true;
            illusionTrail.enabled = true;
            
            // Set Timers
            dashDurationTimer?.Dispose();
            dashRecoveryTimer?.Dispose();
            dashDurationTimer = TimerModule.CreateTimer(dashDurationSecond,Timer.UpdateType.Update,() =>
            {
                isDashing = false;
                dashDurationTimer = null;
                
                bodyTrail.enabled = false;
                illusionTrail.enabled = false;
            });
            dashRecoveryTimer = TimerModule.CreateTimer(dashRecoverySecond, Timer.UpdateType.Update, () =>
            {
                dashRecovered = true;
                dashRecoveryTimer = null;
            });
            
            pressedControllKey = KeyCode.None;
        }

        private void UpdateDashing()
        {
            float move = dashSpeed * Time.deltaTime * (dash2Right ? 1f : -1f);
            bool dashWillSuccess = body.CanMoveHorizontally(move) &&
                                   illusion.CanMoveHorizontally(dashSpeed * Time.deltaTime * (dash2Right? 1f:-1f));
            if (!dashWillSuccess)
            {
                if (canDashInterrupted)
                    StopDashing();
                return;
            }
            
            body.ApplyHorizontalMove(move);
            illusion.ApplyHorizontalMove(move);
        }
        private void StopDashing()
        {
            isDashing = false;
            dashDurationTimer.Dispose();
        }
    }
}