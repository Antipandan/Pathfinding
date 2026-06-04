using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity;
using UnityEngine;
using static SquareTypes;

public class DrawMap : MonoBehaviour
{
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

    public Dictionary<SquareTypes, Color> SquareColors
    {
        get => squareColors;
    }

    private void Awake()
    {
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
    

    private void OnDrawGizmos()
    {
        // förhindra att felmeddelanden dycker upp i editmode
        if (!UnityEditor.EditorApplication.isPlaying) return;
        foreach (Square square in generateMap.GetSquares)
        {
            Vector2Int position = square.SquarePosition;
            Gizmos.color = SelectRightColor(square);
            Gizmos.DrawCube(new Vector3(position.x, position.y, 0f), Vector3.one / 2f);
        }
    }

    private void RequestColorChange()
    {
        
    }
    private void OnValidate()
    {
        // det måste finnas ett bättre sätt att göra detta på!
        BuildDictionary();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Color SelectRightColor(Square square)
    {
        // holy spaghetti
        return squareColors[(SquareTypes)(int)Mathf.Pow(2f, (float)square.Type.GetDominantSquareType())];
    }
}
