using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public interface IGenerateMap
{
    public Square[,] GetSquares { get; }
    
    public Square GetStartingSquare { get; }
    
    public Square GetGoalSquare { get; }
    
    public void FillTile(Vector3Int origin, Color color)
    {
        
    }
}
