using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameCode;
using Unity;
using UnityEditor.SearchService;
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
            return (int)(Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff));
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
        
        /// <summary>
        /// Prevents certain functions from running whilst unity is in editor mode. Intended to not run functions that for example Instantiate gameObjects and other
        /// </summary>
        /// <param name="functionsToRun">takes in a variable amount of function pointers of type 'Action'</param>
        public static void PreventFunctionsRunningInEditor(params Action[] functionsToRun)
        {
            foreach (Action function in functionsToRun)
            {
                if (UnityEditor.EditorApplication.isPlaying) function();
            }
        }

        /// <summary>
        /// Calculates the number of numbers inside a number.
        /// </summary>
        /// <param name="value">The number to count the number of numbers</param>
        /// <returns>number of numbers</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetLengthOfNumber(int value)
        {
            return (int)Mathf.Log(value, 10);
        }
        
        /// <summary>
        /// Calculates the number of numbers inside a number.
        /// </summary>
        /// <param name="value">The number to count the number of numbers</param>
        /// <returns>number of numbers</returns>
        public static int GetLengthOfNumber(float value)
        {
            return (int)Mathf.Log(value, 10);
        }

        /// <summary>
        /// Iterates over every single char in a given string. Returns the combinded ASCII value of said chars.
        /// if string can be directly converted to an int, that will be returned
        /// </summary>
        /// <param name="seedString">string to count value of it's chars</param>
        /// <returns>Combined ASCII value of all chars or the int representation of string</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ParseSeed(string seedString)
        {
            return int.TryParse(seedString, out int seedInt) ? seedInt : CountCharValue(seedString);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CountCharValue(string text)
        {
            int total = 0;
            foreach (char c in text)
            {
                total += (int)c;
            }
            return total;
        }

        public static void DoubleForLoop(Vector2Int bounds, Action body)
        {
            for (int i = 0; i < bounds.x; i++)
            {
                for (int j = 0; j < bounds.y; j++)
                {
                    body.Invoke();
                }
            }
        }

        public static void DoubleForLoop<T>(Vector2Int bounds, Action<T> body, T argument)
        {
            for (int i = 0; i < bounds.x; i++)
            {
                for (int j = 0; j < bounds.y; j++)
                {
                    body.Invoke(argument);
                }
            } 
        }

        public static void DoubleForLoop<T1, T2>(Vector2Int bounds, Action<T1, T2> body, T1 argument1, T2 argument2)
        {
            for (int i = 0; i < bounds.x; i++)
            {
                for (int j = 0; j < bounds.y; j++)
                {
                    body.Invoke(argument1, argument2);
                }
            }
        }

        public static void DoubleForLoop<T1, T2, T3>(Vector2Int bounds, Action<T1, T2, T3> body, T1 argument1,
            T2 argument2, T3 argument3)
        {
            for (int i = 0; i < bounds.x; i++)
            {
                for (int j = 0; j < bounds.y; j++)
                {
                    body.Invoke(argument1, argument2, argument3);
                }
            }  
        }

        public static void DoubleForLoop<T1, T2, T3, T4>(Vector2Int bounds, Action<T1, T2, T3, T4> body, T1 argument1,
            T2 argument2, T3 argument3, T4 argument4)
        {
            for (int i = 0; i < bounds.x; i++)
            {
                for (int j = 0; j < bounds.y; j++)
                {
                    body.Invoke(argument1, argument2, argument3, argument4);
                }
            }
        }
        
    }
}