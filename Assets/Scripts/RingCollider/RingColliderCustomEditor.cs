using System;
using UnityEditor;
using UnityEngine;

namespace RingCollider
{
    [CustomEditor(typeof(RingCollider))]
    public class RingColliderCustomEditor : Editor
    {
        private RingCollider ring;
        private SerializedProperty otterRadius;
        private SerializedProperty innerRadius;
        
        private void OnEnable()
        {
            ring = (RingCollider)target;
            otterRadius = serializedObject.FindProperty("otterRadius");
            innerRadius = serializedObject.FindProperty("innerRadius");
            ring.edgeCollider = ring.GetComponent<EdgeCollider2D>();
        }

        
    }
}