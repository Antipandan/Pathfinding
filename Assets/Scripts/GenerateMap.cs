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
    [Tooltip("Position is 0-indexed. Max value is rows or columns - 1")]
    [SerializeField] private Vector2Int startingSquarePosition;
    [Tooltip("Position is 0-indexed. Max value is rows or columns - 1")]
    [SerializeField] private Vector2Int goalSquarePosition;
    [SerializeField] [Range(0, 500)] private int maxWeight = 15;
    [SerializeField] private string seed = "";

    private Random rand = new Random();
    private Square[,] squares;
    
    public Random GetRandom
    {
        get => rand;
    }
    public Square[,] GetSquares
    {
        get => squares;
        set =>  squares = value;
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

    public ushort Rows
    {
        get => rows;
    }

    public ushort Columns
    {
        get => columns;
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
        squares = new Square[rows, columns];
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
        rows = clampDimensions(rows, 2, ushort.MaxValue);
        columns = clampDimensions(columns, 2, ushort.MaxValue);
        Vector2Int defaultStartingPosition = new Vector2Int(0, 0);
        Vector2Int dimensions = new Vector2Int(rows, columns);
        Vector2Int defaultGoalPosition = new Vector2Int(rows - 1, columns - 1);
        ResetCorrectly(ref startingSquarePosition, dimensions);
        ResetCorrectly(ref goalSquarePosition, dimensions);
        if (startingSquarePosition == goalSquarePosition)
        {
            startingSquarePosition = defaultStartingPosition;
            goalSquarePosition = defaultGoalPosition;
        }
    }
    
    private ushort clampDimensions(ushort value, ushort min, ushort max)
    {
        if (value < min) return min;
        if (value > max)
        {
            Debug.Log($"returning max: {max}");
            return max;
        }
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResetCorrectly(ref Vector2Int position, Vector2Int dimensions)
    {
        Vector2Int newPosition = new Vector2Int(position.x % dimensions.x, position.y % dimensions.y);
        if (newPosition.x < 0) newPosition.x += dimensions.x; 
        if (newPosition.y < 0) newPosition.y += dimensions.y;
        position = newPosition;
    }
    
    private void GenerateGrid()
    {
        UtilityFunctions.InitializeSquares(ref squares, rows, columns);
        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                squares[x, y].ModifySquare(new Vector2Int(x, y), UtilityFunctions.RandomizeWeight(rand, 0, maxWeight), SquareTypes.RegularSquare);
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
        CheckValuesAreCorrect();
        // förhindra felmeddelande
        UtilityFunctions.PreventFunctionRunningInEditor(GenerateGrid);
    }
}
