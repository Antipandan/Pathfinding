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
    [Tooltip("Measured in pixels")]
    [SerializeField] private ushort mapIndentationX;
    [Tooltip("Measured in pixels")]
    [SerializeField] private ushort mapIndentationY;

    private int coefficientScreenToWorldPosition;
    private int screenHeight;
    private int screenWidth;
    
    private int mapWidthWorldPos;
    private int mapHeightWorldPos;
    private int nrRows;
    private int nrColumns;
    
    private Square[,] squares;

    public Square[,] GetSquares => squares;

    private void Awake()
    {
        GetAllNecessaryStuff();
        GenerateGrid();
    }
    
    
    private void GenerateGrid()
    {
        return;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetAllNecessaryStuff()
    {
        GetScreenHeightAndWidth();
        GetMapWidthHeight();
        GetNrRowsColumns();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetScreenHeightAndWidth()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetMapWidthHeight()
    {
        mapWidthWorldPos = ((screenWidth - mapIndentationX) - (0 +  mapIndentationX)) * coefficientScreenToWorldPosition;
        mapHeightWorldPos = ((screenHeight - mapIndentationY) - (0 + mapIndentationY)) * coefficientScreenToWorldPosition;
        coefficientScreenToWorldPosition = (int) (screenWidth / 10f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetNrRowsColumns()
    {
        nrRows = mapWidthWorldPos / rows;
        nrColumns = mapHeightWorldPos / columns;
    }
}
