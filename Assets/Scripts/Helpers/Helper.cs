using UnityEngine;

namespace Game.HelperFunctions
{
    public class Helper
    {

        public static bool canLog;
        public static bool canDrawDebugRays;
        public static bool canDrawDebugSpheres;

        public static void Log(string message, byte type = 0) // by default, just log the thing out.
        {
            if (canLog)
            {
                switch (type)
                {
                    case 0:
                        Debug.Log(message);
                        break;
                    case 1:
                        Debug.LogWarning(message);
                        break;
                    case 2:
                        Debug.LogError(message);
                        break;
                    default:
                        Debug.Log(message);
                        break;
                }
            }
        }
        public static void De_Ray(Vector3 startPos, Vector3 endPos, Color color)
        {
            if(canDrawDebugRays)
                Debug.DrawRay(startPos, endPos, color);
        }

        public static void De_Sphere(Vector3 position, Vector3 position1, Color color) // change this implementation
        {
            if(canDrawDebugSpheres)
            {
                Debug.DrawRay(position, position1, color); 
            }
        }

    }   
}

