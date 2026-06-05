using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Mono.Collections.Generic;
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
    private Square[,] searchGrid = {};
    private List<Square> openList = new List<Square>();
    private List<Square> closedList = new List<Square>();
    // member variables
    private Square startingSquare;
    private Square currentSquare;
    private Square endingSquare;
    

    private void Start()
    {
        SetupStuff();
        StartCoroutine(AStarAlgorithm());
    }
    
    private IEnumerator AStarAlgorithm()
    {
        List<Square> neighbours = new List<Square>(); 
        while (openList.Count > 0)
        {
            Square cheapestSquare = null;
            foreach (Square square in openList)
            {
                if (cheapestSquare == null || CalculateFCost(square) < CalculateFCost(cheapestSquare))
                {
                    cheapestSquare = square;
                }
            }
            openList.Remove(cheapestSquare);
            
            neighbours = TryGetNeighbours(cheapestSquare);
            foreach (Square neighbour in neighbours)
            {
                if (neighbour == endingSquare) break;
                else
                {
                    neighbour.G = cheapestSquare!.G + (uint) CalculateDistance(neighbour, cheapestSquare);
                    neighbour.H = (uint) CalculateDistance(neighbour);
                }
                if (!DetermineIfSkip(neighbour)) openList.Add(neighbour);
            }
            closedList.Add(cheapestSquare);
            yield return new WaitForSeconds(searchFrequencyDelay / 1000f);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Square> TryGetNeighbours(Square square)
    {
        List<Square> currentNeighbours = new List<Square>(4);
        Vector2Int ParentPosition = square.SquarePosition;
        TryAddSingleEntry(currentNeighbours,new Vector2Int(ParentPosition.x, ParentPosition.y - 1));
        TryAddSingleEntry(currentNeighbours,new Vector2Int(ParentPosition.x, ParentPosition.y + 1));
        TryAddSingleEntry(currentNeighbours,new Vector2Int(ParentPosition.x - 1, ParentPosition.y));
        TryAddSingleEntry(currentNeighbours,new Vector2Int(ParentPosition.x + 1, ParentPosition.y));
        DeleteNullEntries(ref currentNeighbours);
        return currentNeighbours;
    }

    private void TryAddSingleEntry(List<Square> list, Vector2Int index)
    {
        Square newSquare;
        try
        {
            newSquare = searchGrid[index.x, index.y];
        }
        catch (IndexOutOfRangeException _)
        {
            newSquare = null;
        }
        list.Add(newSquare);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DeleteNullEntries(ref List<Square> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Square currentSquare = list[i];
            if (currentSquare == null) list.RemoveAt(i);
            else
            {
                // varför fungerar inte det här? först blir den 6 sedan 2????+j lnkjwdanklaWDKLN
                currentSquare.Type.TryAddMoreTypes(SquareTypes.NeighbourSquare);
                generateMap.GetSquares[currentSquare.SquarePosition.x, currentSquare.SquarePosition.y] = currentSquare;
            }
        }
    }

    private bool DetermineIfSkip(Square square)
    {
        bool skip = false;
        foreach (Square openSquare in openList)
        {
            if (square.SquarePosition == openSquare.SquarePosition &&
                CalculateFCost(openSquare) < CalculateFCost(square)) skip = true;
        }

        foreach (Square closedSquare in closedList)
        {
            if (square.SquarePosition == closedSquare.SquarePosition &&
                CalculateFCost(closedSquare) < CalculateFCost(square)) skip = true;
        }
        return skip;
    }
    

    private uint CalculateFCost(Square square)
    {
        return square.G + (uint) CalculateDistance(square);
    }

    private void SetupStuff()
    {
        searchGrid = generateMap.GetSquares;
        startingSquare = generateMap.GetStartingSquare;
        endingSquare = generateMap.GetGoalSquare;
        openList.Add(startingSquare);
    }
    

    private void OnDisable()
    {
        StopCoroutine(AStarAlgorithm());
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

    private int CalculateDistance(Square startSquare, Square endSquare)
    {
        int distance = 0;
        switch (distanceFormula)
        {
            case DistanceFormulaTypes.EuclidianDistance:
                distance = UtilityFunctions.CalculateEuclidieanDistance(startSquare.SquarePosition, endSquare.SquarePosition);
                break;
            case DistanceFormulaTypes.ManhattanDistance:
                distance = UtilityFunctions.CalculateManhattanDistance(startSquare.SquarePosition, endSquare.SquarePosition);
                break;
        }
        return distance;
    }
}
