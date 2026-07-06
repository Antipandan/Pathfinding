using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCode
{
    public static class Constants
    {
        public const byte maxNumberLenght = 4;
        public static readonly Dictionary<SquareTypes, Color> squareColors = new Dictionary<SquareTypes, Color>
        {
            { SquareTypes.WallSquare, Color.black },
            { SquareTypes.RegularSquare, Color.white },
            { SquareTypes.NeighbourSquare, Color.yellow },
            { SquareTypes.FoundPathSquare, Color.red },
            { SquareTypes.EndNodeSquare, Color.magenta },
            { SquareTypes.StartNodeSquare, Color.green },
        };
    }
}
