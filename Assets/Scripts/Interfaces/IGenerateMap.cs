using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public interface IGenerateMap
{
    public Square[,] GetSquares { get; }
    
    public void FillTile(Vector3Int origin, Color color)
    {
        
    }
}
