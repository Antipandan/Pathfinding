using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using static Constants;
using static SquareTypes;

public class DrawMap : MonoBehaviour
{
    [SerializeField] private GenerateMap generateMap;
    [Space]
    [SerializeField] private Color regularColor = GetSquareColors[RegularColor];
    [SerializeField] private Color wallColor = GetSquareColors[WallSquare];
    [Space]
    [SerializeField] private Color neighbourColor = GetSquareColors[NeighbourColor];
    [SerializeField] private Color foundPathColor = GetSquareColors[FoundPathColor];
    [Space]
    [SerializeField] private Color endNodeColor = GetSquareColors[EndNodeColor];
    [SerializeField] private Color startNodeColor = GetSquareColors[StartNodeColor];
    // det finns säker ett bättre sätt att göra detta på men jag har crunch:at lite för mycket idag...
    private Dictionary<SquareTypes, Color> squareColors = new Dictionary<SquareTypes, Color>(6);

    public Dictionary<SquareTypes, Color> SquareColors
    {
        get => squareColors;
    }

    private void Awake()
    {
        squareColors = new Dictionary<SquareTypes, Color>();
        {
        }
        if (generateMap == null) Debug.LogError($"Warning no reference to {nameof(generateMap)}!");
    }

    private void OnDrawGizmos()
    {
        foreach (Square square in generateMap.GetSquares)
        {
            
            Vector2Int position = square.SquarePosition;
            Gizmos.DrawCube(new Vector3(position.x, position.y, 0f), Vector3.one / 4f);
        }
    }
}
