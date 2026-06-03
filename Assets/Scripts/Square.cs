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
    private SquareType type;
    private Vector2Int squarePosition = Vector2Int.zero;
    private List<Square> neighbourSquares = new List<Square>();

    public Square(Vector2Int squarePosition, int maxWeight, params SquareTypes[] types)
    {
        this.squarePosition = squarePosition;
        this.type = new SquareType(types);
        this.weight = (uint) maxWeight;
    }
    
    public uint Weight
    {
        get => weight;
    }
    
    public SquareType Type
    {
        get => type;
        set => type = value;
    }

    public Vector2Int SquarePosition => squarePosition;

    
    public void AddNeighbours(params Square[] neighbouringSquares)
    {
        foreach (Square Square in neighbouringSquares)
        {
            neighbourSquares.Add(Square);
        }
    }
    
}
