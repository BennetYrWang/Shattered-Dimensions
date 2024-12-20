﻿using System;
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
        [SerializeField] private ParticleSystem illusionCollisionParticle;
        
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

        private void Start()
        {
            illusionCollisionParticle.Stop();
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
            if (Input.GetKeyDown(jumpKey))
            {
                body.Jump(jumpVelocity);
                illusion.Jump(jumpVelocity);
            }
            
            float horizontalMove = 0;
            CalculateHorizontalMove();

            if (horizontalMove == 0)
            {
                if (prevIsMoving)
                {
                    onMoveStop?.Invoke();
                    prevIsMoving = false;
                }
            }
            else
            {
                bool illusionCanMove = illusion.CanMoveHorizontally(horizontalMove);
                if (!illusionCanMove)
                {
                    illusionCollisionParticle.Play();
                    
                    if (prevIsMoving)
                    {
                        onMoveStop?.Invoke();
                        prevIsMoving = false;
                    }
                    return;
                }
                bool bodyCanMove = body.CanMoveHorizontally(horizontalMove);
                if (!bodyCanMove)
                {
                    if (prevIsMoving)
                    {
                        onMoveStop?.Invoke();
                        prevIsMoving = false;
                    }
                    return;
                }
                body.ApplyHorizontalMove(horizontalMove);
                illusion.ApplyHorizontalMove(horizontalMove);
                if (!prevIsMoving)
                {
                    onMoveStart?.Invoke();
                    prevIsMoving = true;
                }
            }
            
            void  CalculateHorizontalMove()
            {
                if (Input.GetKey(leftKey))
                {
                    horizontalMove -= moveSpeed;
                    body.FacingRight = false;
                    if (_dualExistence)
                        illusion.FacingRight = false;
                }

                if (Input.GetKey(rightKey))
                {
                    horizontalMove += moveSpeed;
                    body.FacingRight = true;
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

            bodyTrail.emitting = true;
            illusionTrail.emitting = true;
            
            // Set Timers
            dashDurationTimer?.Dispose();
            dashRecoveryTimer?.Dispose();
            dashDurationTimer = TimerModule.CreateTimer(dashDurationSecond,Timer.UpdateType.Update,() =>
            {
                isDashing = false;
                dashDurationTimer = null;

                bodyTrail.emitting = false;
                illusionTrail.emitting = false;

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