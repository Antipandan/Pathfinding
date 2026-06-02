using System;

public enum SquareTypes
{
    // Det är viktigt att de squares som anses vara viktigas anges längst ned eftersom vi ska göra bit manipulering
    WallSquare = 0b0, // 0
    RegularColor = 0b1, // 1
    NeighbourColor = 0b10, // 2
    FoundPathColor = 0b100, // 4
    EndNodeColor = 0b1000, // 8
    StartNodeColor = 0b10000_0, // 16
    
}
