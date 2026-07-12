using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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
        /// Either converts string to int if possible. If not counts the ASCII char value in a string
        /// </summary>
        /// <param name="seedString">string to count value of it's chars</param>
        /// <returns>Combined ASCII value of all chars or the int representation of string</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ParseSeed(string seedString)
        {
            return int.TryParse(seedString, out int seedInt) ? seedInt : CountCharValue(seedString);
        }

        /// <summary>
        /// Counts the ASCII char value in a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Runs a function inside a double for loop
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        public static void DoubleForLoop(Vector2Int bounds, Action<Vector2Int> body)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    body.Invoke(new Vector2Int(x, y));
                }
            }
        }

        
        /// <summary>
        /// Runs a function inside a double for loop
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <param name="argument">Argument needed to run function</param>
        /// <typeparam name="T1">Type of the first argument</typeparam>
        public static void DoubleForLoop<T1>(Vector2Int bounds, Action<Vector2Int, T1> body, T1 argument)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    body.Invoke(new Vector2Int(x, y), argument);
                }
            }
        }

        /// <summary>
        /// Runs a function inside a double for loop
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <param name="argument1">First argument needed to run the function</param>
        /// <param name="argument2">Second argument needed to run the function</param>
        /// <typeparam name="T1">Type of the first argument</typeparam>
        /// <typeparam name="T2">Type of the second argument</typeparam>
        public static void DoubleForLoop<T1, T2>(Vector2Int bounds, Action<Vector2Int, T1, T2> body, T1 argument1, T2 argument2)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    body.Invoke(new Vector2Int(x, y), argument1, argument2);
                }
            }
        }

        /// <summary>
        /// Runs a function inside a double for loop
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <param name="argument1">First argument needed to run the function</param>
        /// <param name="argument2">Second argument needed to run the function</param>
        /// <param name="argument3">Third argument needed to run the function</param>
        /// <typeparam name="T1">Type of the first argument</typeparam>
        /// <typeparam name="T2">Type of the second argument</typeparam>
        /// <typeparam name="T3">Type of the third argument</typeparam>
        public static void DoubleForLoop<T1, T2, T3>(Vector2Int bounds, Action<Vector2Int, T1, T2, T3> body,
            T1 argument1, T2 argument2, T3 argument3)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    body.Invoke(new Vector2Int(x, y), argument1, argument2, argument3);
                }
            }
        }

        /// <summary>
        /// Runs a function inside a double for loop
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <param name="argument1">First argument needed to run the function</param>
        /// <param name="argument2">Second argument needed to run the function</param>
        /// <param name="argument3">Third argument needed to run the function</param>
        /// <param name="argument4">Fourth argument needed to run the function</param>
        /// <typeparam name="T1">Type of the first argument</typeparam>
        /// <typeparam name="T2">Type of the second argument</typeparam>
        /// <typeparam name="T3">Type of the third argument</typeparam>
        /// <typeparam name="T4">Type of the fourth argument</typeparam>
        public static void DoubleForLoop<T1, T2, T3, T4>(Vector2Int bounds, Action<Vector2Int, T1, T2, T3, T4> body,
            T1 argument1, T2 argument2, T3 argument3, T4 argument4)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    body.Invoke(new Vector2Int(x, y), argument1, argument2, argument3, argument4);
                }
            }
        }
        
        /// <summary>
        /// Runs a function inside a double for loop. Returns values iteratively
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <typeparam name="TResult">Type of the return value</typeparam>
        public static IEnumerable<TResult> DoubleForLoop<TResult>(Vector2Int bounds, Func<Vector2Int, TResult> body)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    yield return body.Invoke(new Vector2Int(x, y));
                }
            }
            yield return default;
        }
        
        /// <summary>
        /// Runs a function inside a double for loop. Returns values iteratively
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <param name="argument1">First argument needed to run the function</param>
        /// <typeparam name="T1">Type of the first argument</typeparam>
        /// <typeparam name="TResult">Type of the return value</typeparam>
        public static IEnumerable<TResult> DoubleForLoop<T1, TResult>(Vector2Int bounds, Func<Vector2Int, T1, TResult> body,
            T1 argument1)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                   yield return body.Invoke(new Vector2Int(x, y), argument1);
                }
            }
            yield return default;
        }
        
        /// <summary>
        /// Runs a function inside a double for loop. Returns values iteratively
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <param name="argument1">First argument needed to run the function</param>
        /// <param name="argument2">Second Argument needed to run the function</param>
        /// <typeparam name="T1">Type of the first argument</typeparam>
        /// <typeparam name="T2">Type of the second argument</typeparam>
        /// <typeparam name="TResult">Type of the return value</typeparam>
        public static IEnumerable<TResult> DoubleForLoop<T1, T2, TResult>(Vector2Int bounds, Func<Vector2Int, T1, T2, TResult> body,
            T1 argument1, T2 argument2)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    yield return body.Invoke(new Vector2Int(x, y), argument1, argument2);
                }
            }
            yield return default;
        }

        /// <summary>
        /// Runs a function inside a double for loop. Returns values iteratively
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <param name="argument1">First argument needed to run the function</param>
        /// <param name="argument2">Second Argument needed to run the function</param>
        /// <param name="argument3">Third argument needed to run the function</param>
        /// <typeparam name="T1">Type of the first argument</typeparam>
        /// <typeparam name="T2">Type of the second argument</typeparam>
        /// <typeparam name="T3">Type of the third argument</typeparam>
        /// <typeparam name="TResult">Type of the return value</typeparam>
        public static IEnumerable<TResult> DoubleForLoop<T1, T2, T3, TResult>(Vector2Int bounds,
            Func<Vector2Int, T1, T2, T3, TResult> body, T1 argument1, T2 argument2, T3 argument3)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    yield return body.Invoke(new Vector2Int(x, y), argument1, argument2, argument3);
                }
            }
            yield return default;
        }

        /// <summary>
        /// Runs a function inside a double for loop. Returns values iteratively
        /// </summary>
        /// <param name="bounds">Where should both loops end. x value represents where the outer for loop should end.
        /// Y value represents where the inner for loop should end</param>
        /// <param name="body">Function to run</param>
        /// <param name="argument1">First argument needed to run the function</param>
        /// <param name="argument2">Second Argument needed to run the function</param>
        /// <param name="argument3">Third argument needed to run the function</param>
        /// <param name="argument4"></param>
        /// <typeparam name="T1">Type of the first argument</typeparam>
        /// <typeparam name="T2">Type of the second argument</typeparam>
        /// <typeparam name="T3">Type of the third argument</typeparam>
        /// <typeparam name="T4">Type of the fourth argument</typeparam>
        /// <typeparam name="TResult">Type of the return value</typeparam>
        public static IEnumerable<TResult> DoubleForLoop<T1, T2, T3, T4, TResult>(Vector2Int bounds,
            Func<Vector2Int, T1, T2, T3, T4, TResult> body, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
        {
            for (int y = 0; y < bounds.x; y++)
            {
                for (int x = 0; x < bounds.y; x++)
                {
                    yield return body.Invoke(new Vector2Int(x, y), argument1, argument2, argument3, argument4);
                }
            }
            yield return default;
        }
    }
}