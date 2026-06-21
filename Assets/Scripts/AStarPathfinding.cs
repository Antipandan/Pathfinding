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

    private readonly HashSet<Square> closedList = new HashSet<Square>();
    // member variables
    private Square startingSquare;
    private Square currentSquare;
    private Square endingSquare;
    

    private void Start()
    {
        CustomEvents._instance.OnReset += Reset;
        SetupStuff();
        StartCoroutine(AStarAlgorithm());
    }

    private Square FindCheapestSquare()
    {
        Square cheapestSquare = null;
        foreach (Square square in openList)
        {
            if (cheapestSquare == null || CalculateFCost(square) < CalculateFCost(cheapestSquare))
            {
                cheapestSquare = square;
            }
        }
        UpdateSquare(ref cheapestSquare, SquareTypes.FoundPathSquare);
        return cheapestSquare;
    }
    
    // https://www.geeksforgeeks.org/dsa/a-search-algorithm/
    private IEnumerator AStarAlgorithm()
    {
        List<Square> neighbours = new List<Square>(); 
        while (openList.Count > 0)
        {
            Square cheapestSquare = FindCheapestSquare();
            openList.Remove(cheapestSquare);

            neighbours = generateMap.GetNeighbours(cheapestSquare);
            foreach (Square square in neighbours)
            {
                Square neighbour = square;
                // steg 1
                if (neighbour == endingSquare)
                {
                    Debug.Log($"goal found!");
                    closedList.Add(cheapestSquare);
                    foreach (Square s in closedList)
                    {
                        Debug.Log($"position: {s.SquarePosition}"); 
                    }
                    yield break;
                }
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
    }
    

    private void UpdateSquare(ref Square square, SquareTypes newType)
    {
        square.AddMoreTypes(newType);
        generateMap.ChangeValueAtIndex(new Vector2Int(square.SquarePosition.x, square.SquarePosition.y), square);
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
        CustomEvents._instance.OnReset -= Reset;
    }

    private void OnEnable()
    {
        CustomEvents._instance.OnReset += Reset;
    }

    private void Reset()
    {
        openList.Clear();
        closedList.Clear();
        SetupStuff();
    }

    private void OnValidate()
    {
        CustomEvents._instance.PublishOnReset();
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

    private void OnApplicationQuit()
    {
        CustomEvents._instance.OnReset -= Reset;
    }
}
