using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Random = System.Random;
using Screen = UnityEngine.Device.Screen;

public class GenerateMap : MonoBehaviour, IGenerateMap, ISeedParse
{
    [SerializeField] private ushort rows;
    [SerializeField] private ushort columns;
    [SerializeField] private Vector2Int startingSquarePosition;
    [SerializeField] private Vector2Int goalSquarePosition;
    [SerializeField] [Range(0, 500)] private int maxWeight = 15;
    [SerializeField] private string seed = "";

    private Random rand;
    private Square[,] squares;
    
    public Random GetRandom
    {
        get => rand;
    }
    public Square[,] GetSquares
    {
        get => squares;
    }

    public Square GetGoalSquare
    {
        get => squares[goalSquarePosition.x, goalSquarePosition.y];
    }

    public Square GetStartingSquare
    {
        get => squares[startingSquarePosition.x, startingSquarePosition.y];
    }

    public Random Rand
    {
        get => rand;
    }

    private void Awake()
    {
        SetupValues();
        CheckValuesAreCorrect();
        squares = new Square[rows, columns];
        GenerateGrid();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetupValues()
    {
        rand = new Random(ParseSeed(seed));
    }

    public int ParseSeed(string seedString)
    {
        int seedSum = 0;
        if (int.TryParse(seedString, out int parsedSeed)) return parsedSeed;
        foreach (char c in seedString)
        {
            seedSum += (int)c;
        }
        return seedSum;
    }

    public List<Square> GetNeighbours(Square square)
    {
        // much spaghett!
        List<Square> neighbours = new List<Square>();
        Vector2Int index = square.SquarePosition;
        if (index.x - 1 >= 0 && index.x + 1 <= rows - 1)
        {
            AddSingleNeighbour(squares[index.x - 1, index.y], neighbours);
            AddSingleNeighbour(squares[index.x + 1, index.y], neighbours);        
        }

        if (index.y - 1 >= 0 && index.y + 1 <= columns - 1)
        {
            AddSingleNeighbour(squares[index.x, index.y - 1], neighbours);
            AddSingleNeighbour(squares[index.x, index.y + 1], neighbours);
        }
        return neighbours;
    }

    private static void AddSingleNeighbour(Square square, List<Square> neighbours)
    {
        square.Type.TryAddMoreTypes(SquareTypes.NeighbourSquare);
        neighbours.Add(square);
    }

    private void CheckValuesAreCorrect()
    {
        Vector2Int defaultGoalPosition = new Vector2Int(rows - 1, columns - 1);
        Vector2Int defaultStartingPosition = new Vector2Int(0, 0);
        startingSquarePosition = new Vector2Int(Mathf.Max(startingSquarePosition.x, 0), Mathf.Max(startingSquarePosition.y, 0));
        goalSquarePosition = new Vector2Int(Mathf.Max(goalSquarePosition.x, 0), Mathf.Max(goalSquarePosition.y, 0));
        if (startingSquarePosition == goalSquarePosition)
        {
            startingSquarePosition = defaultStartingPosition;
            goalSquarePosition = defaultGoalPosition;
        }
        if (goalSquarePosition.x >= rows || goalSquarePosition.y >= columns) goalSquarePosition = defaultGoalPosition;
        if (startingSquarePosition.x >= rows || startingSquarePosition.y >= rows) startingSquarePosition = defaultStartingPosition;
    }
    
    
    private void GenerateGrid()
    {
        if (!UnityEditor.EditorApplication.isPlaying) return;
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
    private void SetSingleSquareType(Vector2Int index, SquareTypes type)
    {
        squares[index.x, index.y].Type = new SquareType(type);
    }

    private void OnValidate()
    {
        // förhindra felmeddelande
        UtilityFunctions.PreventFunctionRunningInEditor(GenerateGrid);
    }
}
