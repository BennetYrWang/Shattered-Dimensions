using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RingCollider
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class RingCollider : MonoBehaviour
    {
        /*
        public int segments = 36;
        public float innerRadius = 0.5f;
        public float otterRadius = 1.0f;

        public EdgeCollider2D edgeCollider;

        void Start()
        {
            edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
            UpdateRingCollider();
        }

        public void UpdateRingCollider()
        {
            Vector2[] innerPoints = new Vector2[segments + 1];

            float angleStep = 360f / segments;

            float ringRadius = (otterRadius + innerRadius)/2;
            edgeCollider.edgeRadius = otterRadius - innerRadius;
            
            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Deg2Rad * i * angleStep;
                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);

                innerPoints[i] = new Vector2(x, y) * ringRadius;
            }
            edgeCollider.points = innerPoints;
        }

        // Gizmos 显示顶点连接
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            if (edgeCollider != null && edgeCollider.points.Length > 0)
            {
                DrawEdgeGizmos(edgeCollider);
            }
        }
        private void OnValidate()
        {
            UpdateRingCollider();
        }
        void DrawEdgeGizmos(EdgeCollider2D edge)
        {
            Vector2[] points = edge.points;
            Gizmos.color = Color.green;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawLine(transform.TransformPoint(points[i]), transform.TransformPoint(points[i + 1]));
            }

            Gizmos.color = Color.green * 1.1f;
            if (innerRadius != otterRadius)
            {
                for (int i = 0; i < points.Length - 1; i++)
                {
                    Handles.DrawWireDisc(transform.TransformPoint(points[i]), Vector3.forward, otterRadius - innerRadius);
                }
            }
        }
        */
    }
}
