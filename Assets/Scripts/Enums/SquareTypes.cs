using System;

public enum SquareTypes
{
    // Det är viktigt att de squares som anses vara viktigas anges längst ned eftersom vi ska göra bit manipulering
    WallSquare = 0b0, // 0
    RegularSquare = 0b1, // 1
    NeighbourSquare = 0b10, // 2
    FoundPathSquare = 0b100, // 4
    EndNodeSquare = 0b1000, // 8
    StartNodeSquare = 0b10000_0, // 16
    
}
