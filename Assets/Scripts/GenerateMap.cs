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
        GenerateGrid();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssignCorrectValues()
    {
        goalSquareIndex = (rows * columns) - 1;
    }
    
    private void GenerateGrid()
    {
        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                squares[x, y] = new Square(new Vector2Int(x, y),
                    UtilityFunctions.RandomizeWeight(rand, 0, maxWeight),
                    SquareTypes.RegularSquare);
            }
        }
        SetSingleSquareType(0, 0, SquareTypes.RegularSquare);
        SetSingleSquareType(rows - 1, columns - 1, SquareTypes.EndNodeSquare);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CheckIfStartGoalSame()
    {
        return startingSquareIndex == goalSquareIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetSingleSquareType(int rowIndex, int columnIndex, SquareTypes type)
    {
        squares[rowIndex, columnIndex].Type = new SquareType(type);
    }
    
    
}
