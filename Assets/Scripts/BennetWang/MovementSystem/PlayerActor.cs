﻿using System.Collections.Generic;
using UnityEngine;

namespace BennetWang.MovementSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerActor : MonoBehaviour
    {
        // Movement
        private bool _isMoving;

        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (_isMoving == value)
                    return;
                _isMoving = value;
                if (_isMoving)
                    onMoveStart?.Invoke();
                else
                    onMoveEnd?.Invoke();
            }
        }

        private bool _facingRight;
        public bool FacingRight
        {
            get => _facingRight;
            set
            {
                if (_facingRight != value)
                    FlipCharacter();
                _facingRight = value;
            }

        }
        [SerializeField] private GravityType gravityDirection;
        public float gravityScale;
        
        // Jumping
        private bool inAir;
        private bool doubleJumped;
        
        // References
        private Rigidbody2D rb;
        public PlayerMovementController controller;
        
        // Private Variables
        private List<RaycastHit2D> raycastHitsCache = new(5);

        // Delegates
        public delegate void OnInAirBegin();
        public event OnInAirBegin onFallingBegin;
        public delegate void OnMoveStart();

        public event OnMoveStart onMoveStart;
        public delegate void OnMoveEnd();
        public event OnMoveEnd onMoveEnd;
        [SerializeField] private Animator spriteAnim;
        protected void Awake()
        {
            
            controller = transform.parent.GetComponent<PlayerMovementController>();
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; //I'm lazy, just use this
            rb.gravityScale = 0f;
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
            
            //Check for jumping
            raycastHitsCache.Clear();
            rb.Cast(GetGravityDirection(), raycastHitsCache, 0.01f);
            
            bool landed = false;
            foreach (RaycastHit2D hit in raycastHitsCache)
            {
                if (hit.collider.CompareTag("LandScape"))
                    landed = true;
            }

            if (!(landed || inAirs))
                onFallingBegin?.Invoke();

            inAir = !landed;
            doubleJumped = inAir && doubleJumped;
        }

        public bool CanMoveHorizontally(float forwardSpeed)
        {
            raycastHitsCache.Clear();
            rb.Cast(GetForwardDirection(), raycastHitsCache, forwardSpeed);
        
            foreach (RaycastHit2D hit in raycastHitsCache)
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

        public void FlipCharacter()
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
        

        public void setPosition(Vector3 pos)
        {
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
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