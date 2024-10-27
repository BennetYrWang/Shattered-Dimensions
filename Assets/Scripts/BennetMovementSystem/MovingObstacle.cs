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

        private void FixedUpdate()
        {
            
        }
    }
}