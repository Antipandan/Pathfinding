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

        /// <summary>
        /// Generic method used to check if object member variables are null. If so return true else false
        /// </summary>
        /// <param name="currentObject">Which object is null. Will not return several objects that may be null.
        /// Returns only the first object that is null</param>
        /// <param name="objects">Objects to check for null</param>
        /// <returns>true if there is a null object. false if there is none</returns>
        public static bool CheckIfObjectsAreNull(out object currentObject, params object[] objects)
        {
            foreach (object obj in objects)
            {
                currentObject = obj;
                if (obj == null) return true;
            }
            currentObject = null;
            return false;
        }
        
        /// <summary>
        /// Generic method used to check if object member variables are null. If so return true else false
        /// </summary>
        /// <param name="objects">Objects to check for null</param>
        /// <returns>true if there is a null object. false if there is none</returns>
        public static bool CheckIfObjectsAreNull(params object[] objects)
        {
            foreach (object obj in objects)
            {
                if (obj == null) return true;
            }

            return false;
        }

        /// <summary>
        /// Generic method used to check if object member variables are null. If so return true else false
        /// </summary>
        /// <param name="objects">Objects to check for null</param>
        /// <returns>true if there is a null object. false if there is none</returns>
        public static bool CheckIfObjectsAreNull(params GameObject[] objects)
        {
            foreach (GameObject obj in objects)
            {
                if (obj == null) return true;
            }
            return false;
        }
        
        /// <summary>
        /// Generic method used to check if object member variables are null. If so return true else false
        /// </summary>
        /// <param name="currentGameObject">Which object is null. Will not return several objects that may be null.
        /// Returns only the first object that is null</param>
        /// <param name="objects">Objects to check for null</param>
        /// <returns>true if there is a null object. false if there is none</returns>
        public static bool CheckIfObjectsAreNull(out GameObject currentGameObject, params GameObject[] objects)
        {
            foreach (GameObject obj in objects)
            {
                currentGameObject = obj;
                if (obj == null) return true;
            }

            currentGameObject = null;
            return false;
        }
        
        /// <summary>
        /// Reizes a grid with a precalculated Vector3
        /// </summary>
        /// <param name="grid">Grid to be modified</param>
        /// <param name="gridSize">Pre-calculated values</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResizeGrid(ref Grid grid, Vector3 gridSize)
        {
            grid.cellSize = gridSize;
        }
        
        /// <summary>
        /// Resizes a grid by defining the number of rows and columns the new grid shall have. Rounds down in case of
        /// decimal numbers
        /// </summary>
        /// <param name="grid">The grid to be modified</param>
        /// <param name="rows">Number of rows the newly modified grid shall have</param>
        /// <param name="columns">Number of column the newly modified grid shall have</param>
        public static void ResizeGrid(ref Grid grid, int rows, int columns)
        {
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;
            grid.cellSize = new Vector3((int) (screenWidth / (float) rows), (int) (screenHeight / (float) columns), 0f);
        }
        
        /// <summary>
        /// Resizes a grid by defining the number of rows and columns the new grid shall have. Rounds down in case of
        /// decimal numbers
        /// </summary>
        /// <param name="grid">The grid to be modified</param>
        /// <param name="screenDimensions">The dimensions of the screen represented as a Vector2Int.
        /// The X value represents the screen width and Y represents the screen height</param>
        /// <param name="rowColumnCount">Number of rows anc columns represented as a Vector2Int. The X value represents the row count and the Y value represents the column count</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResizeGrid(ref Grid grid, Vector2Int screenDimensions, Vector2Int rowColumnCount)
        {
            grid.cellSize = new Vector3(screenDimensions.x / (float)rowColumnCount.x,
                                        screenDimensions.y / (float)rowColumnCount.y,
                                        0f);
        }
        
        public static int RandomizeWeight(System.Random random, int minVal = 0, int maxVal = int.MaxValue)
        {
            if (maxVal < 0) throw new ArgumentOutOfRangeException(nameof(maxVal));
            return random.Next(minVal, maxVal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PreventFunctionRunningInEditor(Action functionToRun)
        {
            if (UnityEditor.EditorApplication.isPlaying) functionToRun();
        }

        public static void InitializeSquares(ref Square[,] squares)
        {
            InitializeSquares(ref squares, squares.GetLength(0), squares.GetLength(1));
        }

        public static void InitializeSquares(ref Square[,] squares, int rows, int columns)
        {
            squares = new Square[rows, columns];
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    squares[x, y] = new Square();
                }
            }
        }
        
    }
}