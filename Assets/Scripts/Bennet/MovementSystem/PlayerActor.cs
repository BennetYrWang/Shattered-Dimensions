using System.Collections.Generic;
using UnityEngine;

namespace Bennet.MovementSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerActor : MonoBehaviour
    {
        public float gravityScale;
        public int dimensionIndex = 1;


        private Rigidbody2D rb;
        [SerializeField] private GravityType gravityDirection;
        private GravityType _prevGravityType;
        public PlayerMovementController controller;
        
        private List<RaycastHit2D> raycastHits = new(5);
        
        // Jumping
        private bool inAir;
        private bool doubleJumped;
        

        protected void Start()
        {
            controller = transform.parent.GetComponent<PlayerMovementController>();
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; //I'm lazy, just use this
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
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                print("Test for flipping. Triggered with Backspace.");
                //FlipCharacter();
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
            
            //Check for jumping
            raycastHits.Clear();
            rb.Cast(GetGravityDirection(), raycastHits, 0.01f);
            inAir = true;
            foreach (RaycastHit2D hit in raycastHits)
            {
                if (hit.collider.CompareTag("LandScape"))
                    inAir = false;
            }
            doubleJumped = inAir && doubleJumped;
        }

        public bool CanMoveHorizontally(float forwardSpeed)
        {
            raycastHits.Clear();
            rb.Cast(GetForwardDirection(), raycastHits, forwardSpeed);
        
            foreach (RaycastHit2D hit in raycastHits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (!hit.collider.GetComponent<PlayerActor>().controller.TryMovePlayerHorizontally(forwardSpeed))
                    {
                        return false;
                    }
                }
                else if (hit.collider.CompareTag("LandScape")) //|| hit.collider.CompareTag("Player"))
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
            if (inAir && doubleJumped)
                return;
            rb.velocity = GetGravityDirection() * -jumpVelocity;
            doubleJumped = inAir;
            inAir = true;
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

        public void FlipCharacter(bool flip)
        {
            //Vector2 forward = GetForwardDirection();
            var scale = transform.localScale;

            if (flip)
            {
                scale = new Vector3(Mathf.Abs(scale.x) * -1, scale.y, scale.z);
            }
            else
            {
                scale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
            }
            //scale.x = forward.x == 0 ? scale.x : -scale.x;
            //scale.y = forward.y == 0 ? scale.y : -scale.y;


            transform.localScale = scale;
        }

        public void setPosition(Vector3 pos)
        {
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}