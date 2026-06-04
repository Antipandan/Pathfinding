using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

public class AStarPathfinding : MonoBehaviour, IPathfinder
{
    [SerializeField] private GenerateMap generateMap;
    [SerializeField] private DistanceFormulaTypes distanceFormula;
    [Tooltip("Delay in milliseconds(ms)")]
    [SerializeField] private ushort searchFrequencyDelay;
    // collections
    private List<Square> openList = new List<Square>();
    private List<Square> closedList = new List<Square>();
    // member variables
    private Square startingSquare;
    private Square currentSquare;
    private Square endingSquare;
    private uint totalGCost;
    private bool paused = false;
    

    private void Start()
    {
        SetupStuff();
        StartCoroutine(Pathfinder());
    }
    

    public IEnumerator Pathfinder()
    {
        WaitForSeconds wait = new WaitForSeconds(searchFrequencyDelay / 1000f);
        while (true)
        {
            // AStarAlgorithm();
            yield return wait;
        }
    }
    
    
    private void AStarAlgorithm()
    {
        while (openList.Count > 0)
        {
            
        }
    }

    private void SetupStuff()
    {
        startingSquare = generateMap.GetStartingSquare;
        endingSquare = generateMap.GetGoalSquare;
        openList.Add(startingSquare);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void IncrementGCost(Square square)
    {
        totalGCost += square.Weight;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DecrementGCost(Square square)
    {
        totalGCost -= square.Weight;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetSquareNeighbours(Square square)
    {
        return;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearNeighbours()
    {
        return;
    }

    private void OnDisable()
    {
        StopCoroutine(Pathfinder());
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
