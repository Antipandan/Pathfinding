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
    private List<Square> path = new List<Square>();
    private List<Square> currentNeighbours;
    private HashSet<Square> searchedSquares;
    private Square[,] searchGrid;
    // member variables
    private Square startingSquare;
    private Square currentSquare;
    private Square endingSquare;
    private uint totalGCost;
    private bool paused = false;

    private void Awake()
    {
        
    }

    private void Update()
    {
        // StartCoroutine(Pathfinder());
    }

    public IEnumerator Pathfinder()
    {
        while (true)
        {
            while (paused == false)
            {
                Debug.Log($"pathfinding?");
                yield return new WaitForSeconds(searchFrequencyDelay / 1000f);
            }

            yield return new WaitUntil(() => false);
        }
        
    }

    private bool function()
    {
        return false;
    }
    

    private void AStarAlgorithm()
    {
        // holy spaghetti, detta är extremt dåligt men jag kan inte lista ut coroutines!!!!!!!!!
        Thread.Sleep(searchFrequencyDelay);
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
