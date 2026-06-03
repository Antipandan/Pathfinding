using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Random = System.Random;
using Screen = UnityEngine.Device.Screen;

public class GenerateMap : MonoBehaviour, IGenerateMap
{
    [SerializeField] private ushort rows;
    [SerializeField] private ushort columns;
    [SerializeField] private int startingSquareIndex;
    [SerializeField] private int goalSquareIndex;
    [SerializeField] [Range(0, 500)] private int maxWeight = 15;

    private Random rand = new Random();
    private Square[,] squares;

    public Square[,] GetSquares
    {
        get => squares;
    }

    public Random Rand
    {
        get => rand;
    }

    private void Awake()
    {
        if (CheckIfStartGoalSame()) AssignCorrectValues();
        squares = new Square[rows, columns];
        GetAllNecessaryStuff();
        GenerateGrid();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssignCorrectValues()
    {
        goalSquareIndex = (rows * columns) - 1;
    }
    
    private void GenerateGrid()
    {
        SetStartingSquare();
        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                squares[x, y] = new Square(new Vector2Int(x, y), UtilityFunctions.RandomizeWeight(rand, 0, maxWeight));
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CheckIfStartGoalSame()
    {
        return startingSquareIndex == goalSquareIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetStartingSquare()
    {
        squares[0, 0].Type = new SquareType(SquareTypes.StartNodeColor);
    }
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetAllNecessaryStuff()
    {
        GetScreenHeightAndWidth();
        GetMapWidthHeight();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetScreenHeightAndWidth()
    {
        return;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetMapWidthHeight()
    {
        return;
    }
    
}
