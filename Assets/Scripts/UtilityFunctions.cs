using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Utility
{
    public static class UtilityFunctions
    {

        /// <summary>
        /// Calculates the distance between two points using the pythagorean theorem see: https://www.geeksforgeeks.org/maths/euclidean-distance/
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">End point</param>
        /// <returns>Distance between two points</returns>
        public static int CalculateEuclidieanDistance(Vector2Int start, Vector2Int end)
        {
            int yDiff = end.y - start.y;
            int xDiff = end.x - start.x;
            // gånger 10 eftersom det ger oss "bra" värden tycker jag
            return (int)(Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff)) * 10;
        }
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateManhattanDistance(float deltaDistanceX, float deltaDistanceY)
        {
            return Mathf.Abs(deltaDistanceX) + Mathf.Abs(deltaDistanceY);
        }

        /// <summary>
        /// Calculates the manhattan distance between two points see: https://www.geeksforgeeks.org/data-science/manhattan-distance/
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">End point</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateManhattanDistance(Vector2Int start, Vector2Int end)
        {
            return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
        }
        
        
        public static int RandomizeWeight(System.Random random, int minVal = 0, int maxVal = int.MaxValue)
        {
            if (maxVal < 0) throw new ArgumentOutOfRangeException(nameof(maxVal));
            return random.Next(minVal, maxVal);
        }
        
        public static void PreventFunctionsRunningInEditor(params Action[] functionsToRun)
        {
            foreach (Action function in functionsToRun)
            {
                if (UnityEditor.EditorApplication.isPlaying) function();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetLengthOfInt(int value)
        {
            return (int)Mathf.Log(value, 10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ParseSeed(string seedString)
        {
            return int.TryParse(seedString, out var seedInt) ? seedInt : CountCharValue(seedString);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CountCharValue(string text)
        {
            int total = 0;
            foreach (char c in text)
            {
                // redundant cast men jag tycker att det förtydligar att vi använder ASCII värdet av char:en
                total += (int)c;
            }
            return total;
        }
        
    }
}