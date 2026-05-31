using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Screen = UnityEngine.Device.Screen;

public class GenerateMap : MonoBehaviour
{
    [Header("Padding")]
    [SerializeField] private Vector2Int screenPadding = new Vector2Int(0, 0);
    [SerializeField] private ushort rows = 40;
    [SerializeField] private ushort columns = 40;
    [Header("Other Settings")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] [Range(0, uint.MaxValue - 1)] private uint maxWeight = UInt32.MaxValue - 1;
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
        Debug.Log($"screen Width: {Screen.width}, screen Height: {Screen.height}");
        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                Instantiate(squarePrefab, 
                    new Vector3(row * width, Screen.height - columns * column - height, 0),
                    Quaternion.identity ,transform);
            }
        }
    }

    private void AStarPathfinding()
    {
        
    }
    
}
