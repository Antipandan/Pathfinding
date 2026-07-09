using System;

namespace GameCode
{
    public enum SquareTypes
    {
        // Det är viktigt att de squares som anses vara viktigas anges längst ned eftersom vi ska göra bit manipulering
        WallSquare = 0b1, // 1
        RegularSquare = 0b10, // 2
        NeighbourSquare = 0b100, // 4
        FoundPathSquare = 0b1000, // 8
        EndNodeSquare = 0b1000_0, // 16
        StartNodeSquare = 0b1000_00, // 32
        FinalPathSquare = 0b1000_000 // 64
    }
}
