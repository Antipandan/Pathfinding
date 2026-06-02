using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Screen = UnityEngine.Device.Screen;

[RequireComponent(typeof(Tilemap), typeof(TilemapRenderer), typeof(Grid))]
public class GenerateMap : MonoBehaviour, IGenerateMap
{
    [SerializeField] private ushort rows;
    [SerializeField] private ushort columns;
    
    private TilemapRenderer tilemapRenderer;
    private Grid tilemapGrid;
    private Tilemap tilemap;
    
    private int screenHeight;
    private int screenWidth;
    private Square[,] squares;

    public Square[,] GetSquares => squares;

    private void Awake()
    {
        GetAllNecessaryStuff();
        GenerateGrid();
        FillRectangle(Vector3Int.zero);
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
    
    
    
    private void FillRectangle(Vector3Int origin)
    {
        FillTile(new Vector3Int(0, 0, 0), Color.red);
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector3Int pos = new Vector3Int(origin.x + x, origin.y + y, origin.z);

            }
        }
        tilemap.RefreshAllTiles();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void FillTile(Vector3Int position, Color color)
    {
        tilemap.SetColor(position, color);
    }
    
}
