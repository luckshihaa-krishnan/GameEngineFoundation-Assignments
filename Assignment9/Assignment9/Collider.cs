/*
* Name: Luckshihaa Krishnan 
* Student ID: 186418216
* Section: GAM 531 NSA 
*/

using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Assignment9
{
    public static class Collision
    {
        // Check collision between a point and a GameObject
        public static bool CheckCollision(Vector3 point, GameObject obj)
        {
            // min and max points
            Vector3 min = obj._position - obj._size / 2;
            Vector3 max = obj._position + obj._size / 2;

            // collision x-axis?
            bool collisionX = point.X >= min.X && point.X <= max.X;

            // collision y-axis?
            bool collisionY = point.Y >= min.Y && point.Y <= max.Y;

            // collision z-axis?
            bool collisionZ = point.Z >= min.Z && point.Z <= max.Z;

            // collision only if on all axes
            return collisionX && collisionY && collisionZ;
        }

    }
}
