using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public interface IGenerateMap
{
    public Square[,] GetSquares { get; }
    
    public Square GetStartingSquare { get; }
    
    public Square GetGoalSquare { get; }
    
    public List<Square> GetNeighbours(Square square);    
}
