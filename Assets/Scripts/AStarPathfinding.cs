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
    private readonly List<Square> openList = new List<Square>();
    private readonly List<Square> closedList = new List<Square>();
    // member variables
    private Square startingSquare;
    private Square currentSquare;
    private Square endingSquare;
    

    private void Start()
    {
        SetupStuff();
        StartCoroutine(AStarAlgorithm());
    }
    
    // https://www.geeksforgeeks.org/dsa/a-search-algorithm/
    private IEnumerator AStarAlgorithm()
    {
        List<Square> neighbours = new List<Square>(); 
        while (openList.Count > 0)
        {
            // Debug.Log($"iterating!");
            Square cheapestSquare = null;
            foreach (Square square in openList)
            {
                if (cheapestSquare == null || CalculateFCost(square) < CalculateFCost(cheapestSquare))
                {
                    cheapestSquare = square;
                    UpdateSquare(ref cheapestSquare, SquareTypes.FoundPathSquare);
                }
            }
            openList.Remove(cheapestSquare);

            neighbours = generateMap.GetNeighbours(cheapestSquare);
            // Debug.Log($"neighbour count: {neighbours.Count}");
            foreach (Square square in neighbours)
            {
                Square neighbour = square;
                // steg 1
                if (neighbour == endingSquare) break;
                // steg 2
                else
                {
                    neighbour.G = cheapestSquare!.G + (uint) CalculateDistance(neighbour, cheapestSquare);
                    neighbour.H = (uint) CalculateDistance(neighbour);
                }
                // steg 3 och 4
                if (!DetermineIfSkip(neighbour))
                {
                    openList.Add(neighbour);
                    UpdateSquare(ref neighbour, SquareTypes.NeighbourSquare);
                }
            }
            closedList.Add(cheapestSquare);
            yield return new WaitForSeconds(searchFrequencyDelay / 1000f);
        }

        foreach (Square square in generateMap.GetSquares)
        {
            if (square.TypesSquare.GetDominantSquareType() ==  SquareTypes.NeighbourSquare) Debug.Log($"neighbour!");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateSquareType(ref Square square, SquareTypes newType)
    {
        square.TypesSquare.TryAddMoreTypes(newType);
        Debug.Log($"square new type: {square.TypesSquare.Type}");
    }

    private void UpdateSquare(ref Square square, SquareTypes newType)
    {
        UpdateSquareType(ref square, newType);
        UpdateSingleSquare(ref square);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateSingleSquare(ref Square square)
    {
        Debug.Log($"before modifying: '{square.TypesSquare.Type}'");
        generateMap.ChangeValueAtIndex(new Vector2Int(square.SquarePosition.x, square.SquarePosition.y), square);
        Debug.Log($"after modifying: '{generateMap.GetSquares[square.SquarePosition.x, square.SquarePosition.y].TypesSquare.Type}'");
    }

    private bool DetermineIfSkip(Square successor)
    {
        bool skip = false;
        foreach (Square openSquare in openList)
        {
            if (successor.SquarePosition == openSquare.SquarePosition &&
                CalculateFCost(openSquare) < CalculateFCost(successor)) skip = true;
        }

        foreach (Square closedSquare in closedList)
        {
            if (successor.SquarePosition == closedSquare.SquarePosition &&
                CalculateFCost(closedSquare) < CalculateFCost(successor)) skip = true;
        }
        return skip;
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
