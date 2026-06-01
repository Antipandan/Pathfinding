using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Screen = UnityEngine.Device.Screen;

[RequireComponent(typeof(Tilemap), typeof(TilemapRenderer), typeof(Grid))]
public class GenerateMap : MonoBehaviour
{
    [SerializeField] private ushort rows;
    [SerializeField] private ushort columns;
    [SerializeField] private ushort searchFrequencyDelay = 0;
    
    private TilemapRenderer tilemapRenderer;
    private Grid tilemapGrid;
    private Tilemap tilemap;
    
    private int screenHeight;
    private int screenWidth;

    private void Awake()
    {
        GetAllNecessaryStuff();
        GenerateGrid();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResizeGrid()
    {
        tilemapGrid.cellSize = new Vector3(screenWidth / (float) rows, screenHeight / (float) columns, 0f);
    }
    
    private void GenerateGrid()
    {
        return;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetAllNecessaryStuff()
    {
        tilemapGrid = GetComponentInChildren<Grid>();
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        GetScreenHeightAndWidth();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetScreenHeightAndWidth()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
    
}
