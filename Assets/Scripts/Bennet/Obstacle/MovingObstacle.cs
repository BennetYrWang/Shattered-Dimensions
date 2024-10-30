using System;
using Bennet.MovementSystem;
using UnityEditor;
using UnityEngine;

namespace Bennet.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    public class MovingObstacle : MonoBehaviour
    {
        protected Vector2 prevLocation;
        protected Rigidbody2D rb;

        public Vector2 Pos1, Pos2;
        [SerializeField, Range(0f,1f)] protected float interpolationPosition = .5f;
        [SerializeField] private bool reverseMovement;
        [SerializeField] float lerpDuration, restTimeAtEnd;
        private float prevLerpPosition;
        private float plannedMove;
        
        protected Collider2D triggerCollider;
        

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            prevLerpPosition = interpolationPosition;
            throw new NotImplementedException();
        }

        private float NewInterpolationPosition()
        {
            return interpolationPosition + 1f / lerpDuration * (reverseMovement? -1 : 1);
        }

        private float GetHorizontalMove()
        {
            throw new NotImplementedException();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            PlayerActor playerActor = other.gameObject.GetComponent<PlayerActor>();
            bool canMove = TryApplyMovementToPlayer(playerActor, GetHorizontalMove());
            if (!canMove)
            {
                throw new NotImplementedException();
            }
        }
        protected bool TryApplyMovementToPlayer(PlayerActor playerActor, float horizontalAmount)
        {
            bool result = playerActor.CanMoveHorizontally(horizontalAmount);
            if (result)
                playerActor.ApplyHorizontalMove(horizontalAmount);
            return result;
        }
    }
}