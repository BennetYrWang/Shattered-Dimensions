using System;
using UnityEditor;
using UnityEngine;

namespace Bennet.Obstacle
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(MovingObstacle))]
    public class MovingObstacleEditor : Editor
    {
        private SerializedProperty pos1Property;
        private SerializedProperty pos2Property;
        private SerializedProperty lerpProperty;
        private Tool prevTool;

        private void OnEnable() 
        {
            pos1Property = serializedObject.FindProperty("Pos1");
            pos2Property = serializedObject.FindProperty("Pos2");
            lerpProperty = serializedObject.FindProperty("lerpPosition");
            prevTool = Tools.current;
            Tools.current = Tool.None;
        }
        
        private void OnDisable()
        {
            Tools.current = prevTool;
        }
        
        [Obsolete("Obsolete")]
        private void OnSceneGUI()
        {
            MovingObstacle obstacle = (MovingObstacle)target;

            Handles.color = Color.green;
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            // Handle 1
            Vector2 newPos1 = Handles.FreeMoveHandle(pos1Property.vector2Value, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
            Handles.DrawLine(newPos1 + Vector2.up * .2f, newPos1 + Vector2.down * .2f);
            Handles.DrawLine(newPos1 + Vector2.left * .2f, newPos1 + Vector2.right * .2f);

            // Handle 2
            Vector2 newPos2 = Handles.FreeMoveHandle(pos2Property.vector2Value, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
            Handles.DrawLine(newPos2 + Vector2.up * .2f, newPos2 + Vector2.down * .2f);
            Handles.DrawLine(newPos2 + Vector2.left * .2f, newPos2 + Vector2.right * .2f);
            
            // Line
            Handles.color = Color.cyan;
            Handles.DrawLine(newPos1, newPos2);
            
            
            // Interpolate
            Handles.color = Color.yellow;
            Vector2 lerpPosition = Vector2.Lerp(newPos1, newPos2, lerpProperty.floatValue);
            Vector2 newLerpPosition = Handles.FreeMoveHandle(lerpPosition, Quaternion.identity, 0.2f, Vector3.zero, Handles.SphereHandleCap);

            // Projecting handles onto the line from Pos1 to Pos2
            Vector2 direction = (newPos2 - newPos1).normalized;
            float projection = Vector2.Dot(newLerpPosition - newPos1, direction);
            projection = Mathf.Clamp(projection, 0, Vector2.Distance(newPos1, newPos2));
            float lerpValue = projection / Vector2.Distance(newPos1, newPos2);
            
            // Transform
            obstacle.transform.position = Vector2.Lerp(newPos1, newPos2, lerpValue);
            

            if (EditorGUI.EndChangeCheck())
            {
                // Apply modification
                pos1Property.vector2Value = newPos1;
                pos2Property.vector2Value = newPos2;
                lerpProperty.floatValue = lerpValue;
                
                // Trigger OnValidate
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
    #endif
}