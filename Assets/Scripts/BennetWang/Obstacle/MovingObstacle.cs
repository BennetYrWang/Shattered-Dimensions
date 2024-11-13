using System;
using BennetWang.Module.Timer;
using BennetWang.MovementSystem;
using UnityEngine;
using Timer = BennetWang.Module.Timer.Timer;

namespace BennetWang.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    public class MovingObstacle : MonoBehaviour
    {

        [Header("Position")] public Vector2 Pos1, Pos2;
        [SerializeField, Range(0f, 1f)] protected float lerpPosition = .5f;
        [SerializeField] private bool moveReversed;
        public bool isMoving = true;
        [SerializeField] float lerpDuration;

        [SerializeField, Tooltip("Time rest at the end")]
        float restTime;

        private float horinzontalMoveAttempt;


        private Rigidbody2D rb;
        protected Collider2D triggerCollider;
        private Vector2 prevLocation;

        private bool reversedThisFrame = false;
        private bool resting = false;

        //Timers
        private Timer restTimer;


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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            PlayerActor playerActor = other.gameObject.GetComponent<PlayerActor>();
            bool canMovePlayer = TryApplyMovementToPlayer(playerActor, horinzontalMoveAttempt);
            if (!canMovePlayer)
            {
                throw new NotImplementedException();
            }
        }

        private void Rest(float restTimeSecond)
        {
            restTimer?.Dispose();
            restTimer = TimerModule.CreateTimer(restTimeSecond, Timer.UpdateType.Update, ReStart);
        }
        
        private void ReStart()
        {
            moveReversed = !moveReversed;
            resting = false;
        }
    }
}