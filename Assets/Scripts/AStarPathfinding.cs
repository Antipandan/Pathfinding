using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Mono.Collections.Generic;
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
    private Square[,] searchGrid;
    private List<Square> openList = new List<Square>();
    private List<Square> closedList = new List<Square>();
    // member variables
    private Square startingSquare;
    private Square currentSquare;
    private Square endingSquare;
    

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
            Square cheapestSquare = null;
            foreach (Square square in openList)
            {
                if (cheapestSquare == null) cheapestSquare = square;
                else if (CalculateFCost(square) < CalculateFCost(cheapestSquare)) cheapestSquare = square;
            }

            List<Square> neighbours = TryGetNeighbours(cheapestSquare);
            foreach (Square neighbour in neighbours)
            {
                if (neighbour == endingSquare) return;
                
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Square> TryGetNeighbours(Square square)
    {
        Vector2Int ParentPosition = square.SquarePosition;
        List<Square> neighbours = new List<Square>
        {
            searchGrid[ParentPosition.x, ParentPosition.y - 1], 
            searchGrid[ParentPosition.x, ParentPosition.y + 1],
            searchGrid[ParentPosition.x - 1, ParentPosition.y],
            searchGrid[ParentPosition.x + 1, ParentPosition.y],
        };
        TryDeleteNullEntries(neighbours);
        return neighbours;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void TryDeleteNullEntries(List<Square> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null) list.RemoveAt(i);
        }
    }

    private uint CalculateFCost(Square square)
    {
        return square.G + (uint) CalculateDistance(square);
    }

    private void SetupStuff()
    {
        startingSquare = generateMap.GetStartingSquare;
        endingSquare = generateMap.GetGoalSquare;
        openList.Add(startingSquare);
    }
    

    private void OnDisable()
    {
        StopCoroutine(Pathfinder());
    }

    private int CalculateDistance(Square square)
    {
        int distance = 0;
        switch (distanceFormula)
        {
            case DistanceFormulaTypes.EuclidianDistance:
                distance = UtilityFunctions.CalculateEuclidieanDistance(square.SquarePosition, endingSquare.SquarePosition);
                break;
            case DistanceFormulaTypes.ManhattanDistance:
                distance = UtilityFunctions.CalculateManhattanDistance(square.SquarePosition, endingSquare.SquarePosition);
                break;
        }
        return distance;
    }
}
