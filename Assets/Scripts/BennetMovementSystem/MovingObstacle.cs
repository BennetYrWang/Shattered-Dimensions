using System;
using UnityEngine;

namespace BennetMovementSystem
{
    public class MovingObstacle : MonoBehaviour
    {
        private Vector2 prevLocation;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<PlayerActor>();
            if (!player)
                return;
            var playerGravityDirection = player.GetGravityDirection();
            float amount = playerGravityDirection.x == 0 ? rb.velocity.x : rb.velocity.y;
            amount *= Time.fixedDeltaTime;
            player.controller.TryMovePlayerHorizontally(amount);
        }
        private bool TryApplyMovementToPlayer(Collision2D other)
        {
            var player = other.gameObject.GetComponent<PlayerActor>();
            if (!player)
                return false;
            var playerGravityDirection = player.GetGravityDirection();
            float amount = playerGravityDirection.x == 0 ? rb.velocity.x : rb.velocity.y;
            amount *= Time.fixedDeltaTime;
            return player.controller.TryMovePlayerHorizontally(amount);
        }
    }
}