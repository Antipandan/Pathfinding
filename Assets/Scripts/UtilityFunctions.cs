using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity;
using UnityEngine;

namespace Utility
{
    public class UtilityFunctions
    {
        /// <summary>
        /// Calculates the distance between two points using the pythagorean theorem see: https://www.geeksforgeeks.org/maths/euclidean-distance/
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">End point</param>
        /// <returns>Distance between two points</returns>
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateEuclidieanDistance(Vector2 start, Vector2 end)
        {
            float xDiff = end.x - start.x;
            float yDiff = end.y - start.y;
            return Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }
        /// <summary>
        /// Calculates the distance between two points using the pythagorean theorem see: https://www.geeksforgeeks.org/maths/euclidean-distance/
        /// </summary>
        /// <param name="deltaDistanceX">Difference between X position</param>
        /// <param name="deltaDistanceY">Difference between Y position</param>
        /// <returns>Distance between two points</returns>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateEuclidieanDistance(float deltaDistanceX, float deltaDistanceY)
        {
            return Mathf.Sqrt(deltaDistanceX * deltaDistanceX + deltaDistanceY * deltaDistanceY);
        }

        /// <summary>
        /// Calculates the manhattan distance between two points see: https://www.geeksforgeeks.org/data-science/manhattan-distance/
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">End point</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateManhattanDistance(Vector2 start, Vector2 end)
        {
            return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
        }

        /// <summary>
        /// Calculates the manhattan distance between two points see: https://www.geeksforgeeks.org/data-science/manhattan-distance/
        /// </summary>
        /// <param name="deltaDistanceX">Difference between X position</param>
        /// <param name="deltaDistanceY">Difference between Y position</param>
        /// <returns></returns>
        public static float CalculateManhattanDistance(float deltaDistanceX, float deltaDistanceY)
        {
            return Mathf.Abs(deltaDistanceX) + Mathf.Abs(deltaDistanceY);
        }
        
    }
}