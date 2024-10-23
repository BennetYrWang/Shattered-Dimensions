using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CentrifugeGravity
{
    public class CentrifugeGravitySource2D : MonoBehaviour
    {
        public static CentrifugeGravitySource2D Instance { get; private set; }
        public bool decayWithDistance = true;
        public SimulationMethod simulationMethod = SimulationMethod.Velocity;
        public float gravityScale;
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position,"d_BuildSettings.Web");
        }

        public enum SimulationMethod
        {
            Acceleration,
            Velocity,
        }
    }
}