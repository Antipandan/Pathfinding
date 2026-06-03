using System;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const uint InfiniteWeight = int.MaxValue;
    private static readonly Dictionary<SquareTypes, Color> squareColors = new Dictionary<SquareTypes, Color>
        {
            {SquareTypes.WallSquare, Color.black},
            {SquareTypes.RegularSquare, Color.white},
            {SquareTypes.NeighbourSquare, Color.yellow},
            {SquareTypes.FoundPathSquare, Color.red},
            {SquareTypes.EndNodeSquare, Color.magenta},
            {SquareTypes.StartNodeSquare, Color.green},
        };
    public static Dictionary<SquareTypes, Color> GetSquareColors
    {
        get => squareColors;
    }
}
