using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

public class AStarPathfinding : MonoBehaviour
{
    [SerializeField] private GenerateMap generateMap;
    [SerializeField] private DistanceFormulaTypes distanceFormula;
    [Tooltip("Delay in milliseconds(ms)")]
    [SerializeField] private ushort searchFrequencyDelay;
    // collections
    private List<Square> path = new List<Square>();
    private List<Square> currentNeighbours;
    private HashSet<Square> searchedSquares;
    private Square[,] searchGrid;
    // member variables
    private Square startingSquare;
    private Square currentSquare;
    private Square endingSquare;
    private uint totalGCost;


    private void Awake()
    {
        // SetupStuff();
    }

    private void Update()
    {
        return;
    }

    private void SetupStuff()
    {
        searchGrid = generateMap.GetSquares;
        startingSquare = generateMap.GetStartingSquare;
        endingSquare = generateMap.GetGoalSquare;
        currentSquare = startingSquare;
        searchedSquares.Add(currentSquare);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void IncrementGCost(Square square)
    {
        totalGCost += square.Weight;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetSquareNeighbours(Square square)
    {
        currentNeighbours = generateMap.GetNeighbours(square);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearNeighbours()
    {
        currentNeighbours.Clear();
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
