using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public interface IGenerateMap
{
    public Square GetStartingSquare { get; }
    
    public Square GetGoalSquare { get; }
    
    public void UpdateGrid(Square[,] grid);

    public Square[,] RetrieveGrid();

}
