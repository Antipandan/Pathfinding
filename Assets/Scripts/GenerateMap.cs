using System;
using DefaultNamespace;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    [SerializeField] private uint rows = 40;
    [SerializeField] private uint columns = 40;
    [SerializeField] private DistanceFormulaTypes distanceFormula = DistanceFormulaTypes.ManhattanDistance;
    [SerializeField] [Range(0, 100f)] private float pauseInterval = 0f;

    private void GenerateGrid()
    {
        
    }

    private void AStarPathfinding()
    {
        
    }
    
}
