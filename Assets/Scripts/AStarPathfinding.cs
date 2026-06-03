using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AStarPathfinding : MonoBehaviour
{
    [SerializeField] private GenerateMap generateMap;
    [SerializeField] private DistanceFormulaTypes distanceFormula;
    [Tooltip("Delay in milliseconds(ms)")]
    [SerializeField] private ushort SearchFrequencyDelay;
    // collections
    private List<Square> path = new List<Square>();
    private HashSet<Square> searchedSquares;
    private Square[,] searchGrid;
    // member variables
    private Square startingSquare;
    private Square currentSquare;
    private Square endingSquare;
    private uint totalGCost;


    private void Awake()
    {
        return;
    }

    private void Update()
    {
        return;
    }

    private void SetupStuff()
    {
        
    }
    
    private int CalculateDistance()
    {
        int distance = 0;
        switch (distanceFormula)
        {
            case DistanceFormulaTypes.EuclidianDistance:
                distance = UtilityFunctions.CalculateEuclidieanDistance(currentSquare.SquarePosition, endingSquare.SquarePosition);
                break;
            case DistanceFormulaTypes.ManhattanDistance:
                distance = UtilityFunctions.CalculateManhattanDistance(currentSquare.SquarePosition, endingSquare.SquarePosition);
                break;
        }
        return distance;
    }
}
