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
    
    private float size = 1/10f;
    private int coefficientScreenToWorldPosition;
    private int screenHeight;
    private int screenWidth;
    
    private int mapWidthWorldPos;
    private int mapHeightWorldPos;

    private Square[,] squares;

    public Square[,] GetSquares => squares;

    private void Awake()
    {
        squares = new Square[rows, columns];
        // Debug.Log($"mapWidthWorldPos: {mapWidthWorldPos},  mapHeightWorldPos: {mapHeightWorldPos}");
        Instantiate(new GameObject("test Position"), new Vector3(mapWidthWorldPos, mapHeightWorldPos, 0), Quaternion.identity);
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
        GetNrRowsColumns();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetScreenHeightAndWidth()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        coefficientScreenToWorldPosition = (int) (screenHeight / 10f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetMapWidthHeight()
    {
        Debug.Log($"screenWidth: {screenWidth},  screenHeight: {screenHeight},\ncoefficientScreenToWorldPosition: {coefficientScreenToWorldPosition}, ");
        mapWidthWorldPos = (int) (((screenWidth - mapIndentationX) - (0 +  mapIndentationX)) * ((1 / (float) coefficientScreenToWorldPosition)));
        Debug.Log($"mapWidth: {mapWidthWorldPos}");
        mapHeightWorldPos = (int)(((screenHeight - mapIndentationY) - (0 + mapIndentationY)) * (1 / (float) coefficientScreenToWorldPosition));
        Debug.Log($"mapHeight: {mapHeightWorldPos}");

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetNrRowsColumns()
    {
        return;
    }
}
