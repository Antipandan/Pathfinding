using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity;
using Utility;
using Random = System.Random;

public class Square
{
    private uint weight;
    private Vector2Int squarePosition = Vector2Int.zero;
    private List<Square> neighbourSquares = new List<Square>();

    public Square(Vector2Int squarePosition)
    {
        this.squarePosition = squarePosition;
    }

    public Vector2Int SquarePosition => squarePosition;
    public void RandomizeWeight(Random random, int minVal = 0, int maxVal = int.MaxValue)
    {
        if (maxVal < 0) throw new ArgumentOutOfRangeException(nameof(maxVal));
        weight = (uint) random.Next(minVal, maxVal);
    }
}
