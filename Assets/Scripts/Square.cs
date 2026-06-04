using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity;
using Utility;
using Random = System.Random;

public class Square
{
    private uint g;
    private uint h;
    private uint weight;
    private SquareType type;
    private Vector2Int squarePosition = Vector2Int.zero;

    public Square(Vector2Int squarePosition, int maxWeight, params SquareTypes[] types)
    {
        ModifySquare(squarePosition, maxWeight, types);
    }

    public Square()
    {
        ModifySquare(Vector2Int.zero, 0, SquareTypes.RegularSquare);
    }
    
    public uint Weight
    {
        // liten work around men jag bryr mig inte längre direkt!
        get
        {
            if (type.HasSquareType(SquareTypes.StartNodeSquare) || 
                    type.HasSquareType(SquareTypes.EndNodeSquare)) return 0;
            return weight;
        }
    }
    
    public SquareType Type
    {
        get => type;
        set => type = value;
    }

    public Vector2Int SquarePosition
    {
        get => squarePosition;
    }

    public uint G
    {
        get => g;
        set => g = value;
    }

    public uint H
    {
        get => h;
        set => h = value;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ModifySquare(Vector2Int squarePosition, int maxWeight, params SquareTypes[] types)
    {
        this.squarePosition = squarePosition;
        this.type = new SquareType(types);
        this.weight = (uint) maxWeight;
        this.g = weight;
        this.h = 0;
    }
}
