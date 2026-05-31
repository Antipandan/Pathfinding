using System;
using DefaultNamespace;
using UnityEngine;
using Utility;
    
public class GenerateMap : MonoBehaviour
{
    [Header("Padding")]
    [SerializeField] private Vector2Int screenPadding = new Vector2Int(0, 0);
    [SerializeField] private uint rows = 40;
    [SerializeField] private uint columns = 40;
    [Header("Other Settings")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private DistanceFormulaTypes distanceFormula = DistanceFormulaTypes.ManhattanDistance;
    [SerializeField] [Range(0, 100f)] private float pauseInterval = 0f;


    private void Awake()
    {
        object nullObject = null;
        if (UtilityFunctions.CheckIfObjectsAreNull(out nullObject, squarePrefab))
        {
            
        }
    }
    
    private void GenerateGrid()
    {
        
    }

    private void AStarPathfinding()
    {
        
    }
    
}
