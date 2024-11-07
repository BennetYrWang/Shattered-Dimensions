using System;
using Bennet.MovementSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bennet.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    public class MovingObstacle : MonoBehaviour
    {

        [Header("Position")]
        public Vector2 Pos1, Pos2;
        [SerializeField, Range(0f,1f)] protected float lerpPosition = .5f;
        [SerializeField] private bool moveReversed;
        public bool reverseWhenCollide = false;
        [SerializeField] float lerpDuration;
        [SerializeField, Tooltip("Time rest at the end")] float restTime;
        
        
        private float prevLerpPosition;
        private float plannedMove;
        

        private float horinzontalMoveAttempt;
        
        private Rigidbody2D rb;
        protected Collider2D triggerCollider;
        private Vector2 prevLocation;

        private bool reversedThisFrame = false;
        private float restTimer = -1f;
        private bool resting = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (resting)
            {
                restTimer -= Time.fixedDeltaTime;
                if (restTimer <= 0f)
                {
                    resting = false;
                    restTimer = -1f;
                }
                return;
            }
            
            reversedThisFrame = false;
            prevLerpPosition = lerpPosition;
            lerpPosition += 1 / lerpDuration * (moveReversed ? -1 : 1);
            if (Mathf.Abs(lerpPosition - (moveReversed? 0:1)) < float.Epsilon)
                ReachEnd();
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
            if (reversedThisFrame)
                moveReversed = !moveReversed;
            lerpPosition = prevLerpPosition;
        }

        private void ReachEnd()
        {
            moveReversed = !moveReversed;
            reversedThisFrame = true;
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
    }
}