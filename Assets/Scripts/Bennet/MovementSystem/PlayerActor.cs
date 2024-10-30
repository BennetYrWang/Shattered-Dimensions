using System.Collections.Generic;
using UnityEngine;

namespace Bennet.MovementSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerActor : MonoBehaviour
    {
        [SerializeField] private int defaultCollisionTestCapacity = 5;
        public float gravityScale;
        public int dimensionIndex = 1;


        private Rigidbody2D rb;
        [SerializeField] private GravityType gravityDirection;
        private GravityType _prevGravityType;
        public PlayerMovementController controller;

        protected void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            _prevGravityType = gravityDirection;
            GravityDirection = gravityDirection;
        }


        public GravityType GravityDirection
        {
            get => gravityDirection;
            set
            {
                gravityDirection = value;

                rb.constraints = RigidbodyConstraints2D.FreezeRotation | gravityDirection switch
                {
                    GravityType.Downward => RigidbodyConstraints2D.FreezePositionX,
                    GravityType.Upward => RigidbodyConstraints2D.FreezePositionX,
                    GravityType.Leftward => RigidbodyConstraints2D.FreezePositionY,
                    GravityType.Rightward => RigidbodyConstraints2D.FreezePositionY,
                    _ => RigidbodyConstraints2D.FreezePositionX
                };
                
                rb.rotation = gravityDirection switch
                {
                    GravityType.Downward => 0f,
                    GravityType.Rightward => 90f,
                    GravityType.Upward => 180f,
                    GravityType.Leftward => 270f,
                    _ => 0
                };
            }
        }

       
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
            }
        }

        private void FixedUpdate()
        {
            Vector2 gravityMove = GetGravityDirection() *
                                  (gravityScale * Physics.gravity.magnitude * Time.fixedDeltaTime);
            rb.velocity += gravityMove;
            //rb.MovePosition(rb.position + gravityMove);
            if (_prevGravityType != gravityDirection)
            {
                GravityDirection = gravityDirection;
                _prevGravityType = gravityDirection;
            }
        }

        public bool CanMoveHorizontally(float forwardSpeed)
        {
            List<RaycastHit2D> hits = new(defaultCollisionTestCapacity);
            rb.Cast(GetForwardDirection(), hits, forwardSpeed);
        
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("LandScape") || hit.collider.CompareTag("Player"))
                    return false;
            }
            return true;
        }
        public void ApplyHorizontalMove(float amount)
        {
            rb.position += GetForwardDirection() * amount;
        }
    
        public void Jump(float jumpVelocity)
        {
            rb.velocity = GetGravityDirection() * -jumpVelocity;
        }

 
        public enum GravityType
        {
            Leftward = 0,
            Rightward = 1,
            Downward = 2,
            Upward = 3
        }

        public Vector2 GetGravityDirection()
        {
            int x, y;
            int dir = (int)gravityDirection;
            x = y = ((dir & 0b1) << 1) - 1;
            x *= (~dir & 0b10) >> 1;
            y *= (dir & 0b10) >> 1;
        
            return new Vector2(x, y);
        }

        public Vector2 GetForwardDirection()
        {
            int x, y;
            int dir = (int)gravityDirection;
            y = ((dir & 0b1) << 1) - 1;
            x = -y;
        
            x *= (dir & 0b10) >> 1;
            y *= (~dir & 0b10) >> 1;
        
            return new Vector2(x, y);
        }
    }
}