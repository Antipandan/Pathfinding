using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Screen = UnityEngine.Device.Screen;

public class GenerateMap : MonoBehaviour, IGenerateMap
{
    [SerializeField] private ushort rows;
    [SerializeField] private ushort columns;
    
    private Square[,] squares;

    public Square[,] GetSquares => squares;

    private void Awake()
    {
        squares = new Square[rows, columns];
        GetAllNecessaryStuff();
        GenerateGrid();
    }
    
    private void GenerateGrid()
    {
        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                squares[x, y] = new Square(new Vector2Int(x, y));
            }
        }
    }
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetAllNecessaryStuff()
    {
        GetScreenHeightAndWidth();
        GetMapWidthHeight();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetScreenHeightAndWidth()
    {
        return;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetMapWidthHeight()
    {
        return;
    }
    
}
