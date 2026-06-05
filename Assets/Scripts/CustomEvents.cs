using System;
using UnityEngine;

public class CustomEvents : MonoBehaviour
{
    private static CustomEvents _instance;
    public Func<Square[,]> RetrieveGrid;
    public Action<Square[,]> UpdateGrid;
    private void Start()
    {
        if (_instance != null) _instance = this;
        else Destroy(this);
    }

    public Square[,] PublishRetrieveGrid()
    {
        return RetrieveGrid();
    }

    public void PublishUpdateGrid(Square[,] grid)
    {
        UpdateGrid.Invoke(grid);
    }
    
    
}
