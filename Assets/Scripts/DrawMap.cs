using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using static SquareTypes;

public class DrawMap : MonoBehaviour
{
    [SerializeField] private CustomEvents customEvents;
    [SerializeField] private GenerateMap generateMap;
    [Space]
    [SerializeField] private Color wallColor = Constants.squareColors[WallSquare];
    [SerializeField] private Color regularColor = Constants.squareColors[RegularSquare];
    [Space]
    [SerializeField] private Color neighbourColor = Constants.squareColors[NeighbourSquare];
    [SerializeField] private Color foundPathColor = Constants.squareColors[FoundPathSquare];
    [Space]
    [SerializeField] private Color endNodeColor = Constants.squareColors[EndNodeSquare];
    [SerializeField] private Color startNodeColor = Constants.squareColors[StartNodeSquare];
    // det finns säker ett bättre sätt att göra detta på men jag har crunch:at lite för mycket idag...
    private Dictionary<SquareTypes, Color> squareColors = new Dictionary<SquareTypes, Color>();
    private Square[,] drawGrid = {};

    public Dictionary<SquareTypes, Color> SquareColors
    {
        get => squareColors;
    }

    private void Awake()
    {
        SetupStuff();
        BuildDictionary();
        if (generateMap == null) Debug.LogError($"Warning no reference to {nameof(generateMap)}!");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void BuildDictionary()
    {
        squareColors = new Dictionary<SquareTypes, Color>
        {
            {WallSquare, wallColor},
            {RegularSquare, regularColor},
            {NeighbourSquare, neighbourColor},
            {FoundPathSquare, foundPathColor},
            {EndNodeSquare, endNodeColor},
            {StartNodeSquare, startNodeColor}
        };
    }

    private void SetupStuff()
    {
        customEvents.UpdateGrid += UpdateGrid;
        RetrieveGridFirst();
    }

    private void RetrieveGridFirst()
    {
        drawGrid = customEvents.PublishRetrieveGrid();
    }

    public void UpdateGrid(Square[,] grid)
    {
        drawGrid = grid;
    }

    private void OnDrawGizmos()
    {
        // förhindra att felmeddelanden dycker upp i editmode
        if (!UnityEditor.EditorApplication.isPlaying) return;
        foreach (Square square in drawGrid)
        {
            if (square.Type.GetDominantSquareType() == NeighbourSquare) Debug.Log($"neighbour!");
            Vector2Int position = square.SquarePosition;
            Gizmos.color = SelectRightColor(square);
            Gizmos.DrawCube(new Vector3(position.x, position.y, 0f), Vector3.one / 2f);
        }
        RetrieveGridFirst();
    }
    
    private void OnValidate()
    {
        // det måste finnas ett bättre sätt att göra detta på!
        UtilityFunctions.PreventFunctionRunningInEditor(BuildDictionary);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Color SelectRightColor(Square square)
    {
        // holy spaghetti
        SquareTypes typeToGetColor = (SquareTypes)(int)Mathf.Pow(2f, (float)square.Type.GetDominantSquareType());
        Color selectedColor = squareColors[typeToGetColor];
        Debug.Log($"selected color: {selectedColor} at type: {typeToGetColor}");
        return selectedColor;
    }
}
