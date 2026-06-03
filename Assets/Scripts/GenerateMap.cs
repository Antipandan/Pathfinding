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
    [SerializeField] private Vector2Int startingSquarePosition;
    [SerializeField] private Vector2Int goalSquarePosition;
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
        goalSquarePosition = new Vector2Int(rows - 1, columns - 1);
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
        SetSingleSquareType(startingSquarePosition, SquareTypes.StartNodeSquare);
        SetSingleSquareType(goalSquarePosition, SquareTypes.EndNodeSquare);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CheckIfStartGoalSame()
    {
        return startingSquarePosition == goalSquarePosition;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetSingleSquareType(Vector2Int index, SquareTypes type)
    {
        squares[index.x, index.y].Type = new SquareType(type);
    }
    
    
}
