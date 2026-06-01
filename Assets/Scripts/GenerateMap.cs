using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utility;
using Screen = UnityEngine.Device.Screen;

public class GenerateMap : MonoBehaviour
{
    [Header("Padding")]
    [SerializeField] private Vector2Int screenPadding = new Vector2Int(0, 0);
    [SerializeField] private Vector2Int squarePadding = new Vector2Int(0, 0);
    [SerializeField] private ushort rows = 10;
    [SerializeField] private ushort columns = 10;
    [Header("Other Settings")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] [Range(0, 500)] private uint maxWeight = 50;
    [SerializeField] private DistanceFormulaTypes distanceFormula = DistanceFormulaTypes.ManhattanDistance;
    [SerializeField] [Range(0, 100f)] private float pauseInterval = 0f;

    private int width = 0;
    private int height = 0;
    private HashSet<GameObject> visitedObjects = new HashSet<GameObject>();
    private List<GameObject> objects = new List<GameObject>();


    private void Awake()
    {
        objects.Capacity = (int) (rows * columns);
        object nullObject = null;
        if (!UtilityFunctions.CheckIfObjectsAreNull(out nullObject, squarePrefab))
        {
            GetDimensions(squarePrefab.GetComponent<RectTransform>());
            GenerateGrid();
        }
    }

    private void GetDimensions(RectTransform transform)
    {
        width = (int) transform.rect.width;
        height = (int) transform.rect.height;
    }
    
    private void GenerateGrid()
    {
        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                float xPosition = GiveSquareExtraXIndent(row + columns * column)
                    ? row * width + 5 * row : row * width; 
                
                float yPosition = GiveSquareExtraYindent(row + columns * column) ?
                    Screen.height - height - (height) * column - 5 * column: Screen.height - height - (height) * column;
                
                Instantiate(squarePrefab, 
                    new Vector3(xPosition, yPosition, 0),
                    Quaternion.identity ,transform);
            }
        }
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool GiveSquareExtraXIndent(int squareIndex)
    {
        if (squareIndex == 0) return false;
        return squareIndex % columns != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool GiveSquareExtraYindent(int squareIndex)
    {
        if (squareIndex < rows + 0 * columns) return false; // anledning bakom att vi skriver 0 * column är att vi inte vill indentera rutor som befinner sig 0:e kolumnen
        return true;
    }

    private void AStarPathfinding()
    {
        
    }
    
}
