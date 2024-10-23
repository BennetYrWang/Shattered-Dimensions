using System;
using UnityEngine;

namespace CentrifugeGravity
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CentrifugeGravityReceiver2D : MonoBehaviour
    {
        private Rigidbody2D rb;
        public bool updateRotation = true;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 gravitySourcePosition = CentrifugeGravitySource2D.Instance.transform.position;
            Vector2 gravityDirection = rb.position - gravitySourcePosition;
            Vector2 gravityValue = CentrifugeGravitySource2D.Instance.gravityScale * gravityDirection.normalized;
            
            switch (CentrifugeGravitySource2D.Instance.simulationMethod)
            {
                case CentrifugeGravitySource2D.SimulationMethod.Acceleration:
                    rb.AddForce(gravityValue * rb.mass, ForceMode2D.Force);
                    break;
                case CentrifugeGravitySource2D.SimulationMethod.Velocity:
                    Debug.LogWarning("No, I cannot figure out a way.");
                    break;
            }

            
        }
    }
}