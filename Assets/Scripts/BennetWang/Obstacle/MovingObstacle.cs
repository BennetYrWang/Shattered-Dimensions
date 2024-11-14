using System;
using System.Collections.Generic;
using BennetWang.Module.Timer;
using BennetWang.MovementSystem;
using JetBrains.Annotations;
using UnityEngine;
using Timer = BennetWang.Module.Timer.Timer;

namespace BennetWang.Obstacle
{
    [RequireComponent(typeof(Collider2D),typeof(Rigidbody2D))]
    public class MovingObstacle : MonoBehaviour
    {
        [Header("AnchorPoint")]
        public Vector2 Pos1, Pos2;
        
        [Header("Values")]
        [SerializeField, Range(0f, 1f)] protected float lerpPosition = .5f;
        [SerializeField] private bool moveReversed;
        [SerializeField] float lerpDuration;
        [SerializeField, Tooltip("Time rest at the end")]
        float restTime;

        [Header("Status")]
        private bool resting = false;
        
        // Reference
        private Rigidbody2D rb;
        
        // Timers
        private Timer restTimer;
        
        // Cache
        [CanBeNull] private List<RaycastHit2D> hitCahce = new List<RaycastHit2D>(3);


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (resting)
                return;
            
            float deltaLerpValue = Time.fixedDeltaTime / lerpDuration;
            if (moveReversed)
                deltaLerpValue *= -1f;

            if (!TryApplyMove(deltaLerpValue))
                return;
            
            
            lerpPosition += deltaLerpValue;
            
            bool reachEnd = false;
            if (moveReversed)
                reachEnd = lerpPosition <= 0f;
            else
                reachEnd = lerpPosition >= 1f;
            
            if (reachEnd)
                Rest(restTime);
            
            UpdatePosition();
            
            
        }
        
        
        private bool TryApplyMove(float deltaLerp)
        {
            Vector2 deltaMove = (Pos2 - Pos1) * deltaLerp;
            hitCahce.Clear();
            rb.Cast(deltaMove, hitCahce, deltaMove.magnitude);
            
            foreach (RaycastHit2D hit in hitCahce)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerActor playerActor = hit.collider.GetComponent<PlayerActor>();
                    Vector2 playerForward = playerActor.GetForwardDirection();
                    float horizontalMove = 0f, verticalMove = 0f;
                    if (playerForward.x == 0)
                    {
                        horizontalMove = deltaMove.y * Mathf.Sign(playerForward.y);
                        verticalMove = deltaMove.x * Mathf.Sign(playerForward.y);
                    }
                    else
                    {
                        horizontalMove = deltaMove.x * Mathf.Sign(playerForward.x);
                        verticalMove = deltaMove.y * Mathf.Sign(playerForward.x);
                    }

                    tag = "Untagged";
                    
                    if (!playerActor.controller.TryMovePlayerHorizontally(horizontalMove))
                    {
                        Rest(restTime);
                        return false;
                    }

                    tag = "LandScape";
//
                    playerActor.transform.position += 
                        new Vector3(playerForward.x == 0? verticalMove : 0f,
                            playerForward.y == 0? verticalMove : 0f,0);
                }
            }

            return true;
        }


        private bool TryApplyMovementToPlayer(PlayerActor playerActor, float horizontalAmount)
        {
            bool result = playerActor.CanMoveHorizontally(horizontalAmount);
            if (result)
                playerActor.ApplyHorizontalMove(horizontalAmount);
            return result;
        }

        private void WithdrawMove()
        {
            lerpPosition -= 1 / lerpDuration * (moveReversed ? -1 : 1) * Time.fixedDeltaTime;
        }

        private void UpdatePosition()
        {
            rb.position = Vector2.Lerp(Pos1,Pos2,lerpPosition);
        }

        private void Rest(float restTimeSecond)
        {
        
            lerpPosition = Mathf.Clamp01(lerpPosition);
            resting = true;
            restTimer?.Dispose();
            restTimer = TimerModule.CreateTimer(restTimeSecond, Timer.UpdateType.Update, ReStart);
        }
        
        private void ReStart()
        {
            moveReversed = !moveReversed;
            resting = false;
            restTimer = null;

        }
    }
}