using System;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const uint InfiniteWeight = int.MaxValue;
    private static readonly Dictionary<SquareTypes, Color> squareColors = new Dictionary<SquareTypes, Color>
        {
            {SquareTypes.WallSquare, Color.black},
            {SquareTypes.RegularColor, Color.white},
            {SquareTypes.NeighbourColor, Color.yellow},
            {SquareTypes.FoundPathColor, Color.red},
            {SquareTypes.EndNodeColor, Color.magenta},
            {SquareTypes.StartNodeColor, Color.green},
        };
    public static Dictionary<SquareTypes, Color> GetSquareColors
    {
        get => squareColors;
    }
}
