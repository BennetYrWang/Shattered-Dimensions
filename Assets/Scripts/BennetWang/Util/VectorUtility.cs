using UnityEngine;

namespace BennetWang.Util
{
    public static class VectorUtility
    {
        public static float GetDirectionDeg(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }
        public static Vector3 GetDirection2Euler(this Vector3 vector, EulerOrder eulerOrder = EulerOrder.ZYX)
        {
            if (vector.sqrMagnitude == 0f)
                return Vector3.zero;

            var normalized = vector.normalized;
            float pitch, yaw, roll;

            switch (eulerOrder)
            {
                case EulerOrder.ZYX:
                    // ZYX: First rotate around Z (Yaw), then Y (Pitch), then X (Roll)
                    yaw = Mathf.Atan2(normalized.x, normalized.z);
                    pitch = Mathf.Asin(-normalized.y);
                    roll = 0f;  // No roll considered here, since it's ZYX
                    break;

                case EulerOrder.XYZ:
                    // XYZ: First rotate around X (Roll), then Y (Pitch), then Z (Yaw)
                    pitch = Mathf.Atan2(normalized.y, normalized.z);
                    yaw = Mathf.Atan2(normalized.x, normalized.z);
                    roll = Mathf.Asin(normalized.x);
                    break;

                // Add more cases for different Euler orders
                case EulerOrder.YXZ:
                    // YXZ: First rotate around Y (Pitch), then X (Roll), then Z (Yaw)
                    pitch = Mathf.Atan2(normalized.z, normalized.y);
                    roll = Mathf.Atan2(normalized.x, normalized.y);
                    yaw = Mathf.Asin(-normalized.z);
                    break;

                default:
                    // Default to ZYX if not recognized
                    yaw = Mathf.Atan2(normalized.x, normalized.z);
                    pitch = Mathf.Asin(-normalized.y);
                    roll = 0f;
                    break;
            }
            
            return new Vector3(roll, yaw, pitch);
        }
        public enum EulerOrder
        {
            XYZ,
            YXZ,
            ZYX
        }

        public static float Project(this Vector2 vector, Vector2 onVector)
        {
            return Vector2.Dot(vector, onVector) / onVector.magnitude;
        }

        public enum Vector2Direction
        {
            None = 0,
            Up = 1,
            Down = 2,
            Right = 4,
            Left = 8,
            UpRight = 5,
            UpLeft = 9,
            DownRight = 6,
            DownLeft = 10,
        }

        /// <summary>
        /// Rotate a vector 2 counter-clockwise by angle of radians. The result will apply to the original vector
        /// </summary>
        /// <param name="vector2">The vector to modify</param>
        /// <param name="counterclockwiseRadians">The angle in radians</param>
        /// <returns></returns>
        public static ref Vector2 Rotate(this ref Vector2 vector2, float counterclockwiseRadians)
        {
            float x = vector2.x;
            float y = vector2.y;
            float cos = Mathf.Cos(counterclockwiseRadians);
            float sin = Mathf.Sin(counterclockwiseRadians);
            vector2.x = x * cos - y * sin;
            vector2.y = x * sin + y * cos;
            return ref vector2;
        }

        public static Vector2 AngleToVector(float angleRadiance)
        {
            return new Vector2(Mathf.Cos(angleRadiance), Mathf.Sin(angleRadiance));
        }
    }
    
}