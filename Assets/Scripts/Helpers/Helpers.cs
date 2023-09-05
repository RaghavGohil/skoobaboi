using UnityEngine;

namespace Game.Helpers
{
    public class Helpers
    {

        public static bool canDrawDebugRays;
        public static bool canDrawDebugSpheres;

        public static void De_Ray(Vector3 startPos, Vector3 endPos, Color color)
        {
            if(canDrawDebugRays)
                Debug.DrawRay(startPos, endPos, color);
        }

        public static void De_Sphere(Vector3 position, Vector3 position1, Color color) // for now
        {
            if(canDrawDebugSpheres)
            {
                Debug.DrawRay(position, position1, color); 
            }
        }

    }   
}

