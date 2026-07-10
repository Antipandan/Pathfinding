using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using Utility;

namespace GameCode
{
    public class AStarPathfinding : MonoBehaviour
    {
        [SerializeField] private DistanceFormulaTypes distanceFormula = DistanceFormulaTypes.ManhattanDistance;
        [Tooltip("delay in milliseconds (ms)")]
        [SerializeField] private float aStarSearchDelay = 100f;
        [Tooltip("delay in milliseconds (ms)")]
        [SerializeField] private float tracingSearchDelay = 100f;
        [SerializeField] private CustomEvents customEvent;
        private List<Square> openList;
        private List<Square> closedList;
        private Square startingSquare;
        private Square endingSquare;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            openList = new List<Square>();
            closedList = new List<Square>();
            if (customEvent == null)
            {
                Debug.LogWarning($"Warning! Reference to {nameof(customEvent)} is null!", this);
            }

        }
        
        private void Start()
        {
            // måste sätta de här för att events ska kunna prenumerera i tid?
            endingSquare = customEvent.PublishOnGetEndingSquare();
            startingSquare = customEvent.PublishOnGetStartingSquare();
            openList.Add(startingSquare);
            StartCoroutine(AStarPathfindingAlgorithm());
        }

        private IEnumerator AStarPathfindingAlgorithm()
        {
            bool foundPath = false;
            List<Square> neighbours = new List<Square>(); 
            while (openList.Count > 0 && !foundPath)
            {
                Square cheapestSquare = FindCheapestSquare();
                if (cheapestSquare is null)
                {
                    yield break;
                } 
                openList.Remove(cheapestSquare);

                neighbours = customEvent.PublishOnGetNeighbourSquares(cheapestSquare);
                neighbours = FilterOutNeighbours(neighbours);
                foreach (Square square in neighbours)
                {
                    Square neighbour = square;
                    // steg 1
                    if (neighbour == endingSquare)
                    {
                        Debug.Log($"found ending square");
                        closedList.Add(neighbour);
                        foundPath = true;
                    }

                    // steg 2
                    neighbour.G += cheapestSquare.G;
                    neighbour.H = CalculateDistance(neighbour, endingSquare);
                    
                    // steg 3 och 4
                    if (!DetermineIfSkip(neighbour))
                    {
                        openList.Add(neighbour);
                        TryUpdateSquare(neighbour, SquareTypes.NeighbourSquare);
                    }
                }
                closedList.Add(cheapestSquare);
                yield return new WaitForSeconds(aStarSearchDelay / 1000f);
            }
            if (foundPath) StartCoroutine(TraceBackPath());
        }

        private List<Square> FilterOutNeighbours(List<Square> neighbours)
        {
            List<Square> filteredNeighbours = new List<Square>();
            foreach (Square neighbour in neighbours)
            {
                if (!closedList.Contains(neighbour))filteredNeighbours.Add(neighbour);
            }
            return filteredNeighbours;
        }
        
        
        private bool DetermineIfSkip(Square successor)
        {
            bool skip = false;
            foreach (Square openSquare in openList)
            {
                if (successor.Index == openSquare.Index &&
                    openSquare.F < successor.F) skip = true;
            }

            foreach (Square closedSquare in closedList)
            {
                if (successor.Index == closedSquare.Index &&
                    closedSquare.F < successor.F) skip = true;
            }
            return skip;
        }
        
        
        private Square FindCheapestSquare()
        {
            Square cheapestSquare = null;
            bool sameFValue = CheckIfAllSameFValues(openList);
            foreach (Square square in openList)
            {
                if (!sameFValue)
                {
                    if (cheapestSquare is null || square.F < cheapestSquare.F) cheapestSquare = square;
                }
                else
                {
                    if (cheapestSquare is null || square.G > cheapestSquare.G) cheapestSquare = square;
                }
            }
            TryUpdateSquare(cheapestSquare, SquareTypes.FoundPathSquare);
            
            return cheapestSquare;
        }

        private static bool CheckIfAllSameFValues(List<Square> squares)
        {
            Square previousSquare = null;
            foreach (Square square in squares)
            {
                if (previousSquare is null) previousSquare = square;
                else if (!Mathf.Approximately(previousSquare.F, square.F)) return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void TryUpdateSquare(Square square, SquareTypes squareType)
        {
            if (square.SquareType < squareType) square.SquareType = squareType;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator TraceBackPath()
        {
            Debug.Log($"closedList containts startingSquare: {closedList.Contains(startingSquare)}");
            HashSet<Square> visitedSquares = new HashSet<Square>();
            Square currentSquare = endingSquare;
            while (currentSquare is not null)
            {
                UpdateSingleTraceSquare(currentSquare, visitedSquares);
                List<Square> neighbours = customEvent.PublishOnGetNeighbourSquares(currentSquare);
                foreach (Square neighbour in neighbours)
                {
                    Debug.Log($"all neighbour:  {neighbour.Index}");
                }
                List<Square> borderingNeighbours = new List<Square>();
                foreach (Square neighbour in neighbours)
                {
                    if (closedList.Contains(neighbour) && !visitedSquares.Contains(neighbour))
                    {
                        borderingNeighbours.Add(neighbour);
                    }
                }
                
                currentSquare = FindCheapestGSquare(borderingNeighbours);
                if (currentSquare == startingSquare || currentSquare is null)
                {
                    Debug.Log($"traceback completed!");
                    yield break;
                }
                yield return new WaitForSeconds(tracingSearchDelay / 1000f);
            }
                        
        }

        private static void UpdateSingleTraceSquare(Square square, HashSet<Square> visitedSquares)
        {
            visitedSquares.Add(square);
            if (square.SquareType < SquareTypes.FinalPathSquare) square.SquareType = SquareTypes.FinalPathSquare;
        }

        private static Square FindCheapestGSquare(List<Square> squares)
        {
            Square cheapestSquare = null;
            foreach (Square square in squares)
            {
                if (cheapestSquare is null || cheapestSquare.G > square.G) cheapestSquare = square;
            }
            return cheapestSquare;
        }

        private void OnDisable()
        {
            StopCoroutine(AStarPathfindingAlgorithm());
        }

        private void OnEnable()
        {
            Setup();
            StartCoroutine(AStarPathfindingAlgorithm());
        }
        
        private int CalculateDistance(Square square)
        {
            int distance = 0;
            switch (distanceFormula)
            {
                case DistanceFormulaTypes.EuclidianDistance:
                    distance = UtilityFunctions.CalculateEuclidieanDistance(square.Index, endingSquare.Index);
                    break;
                case DistanceFormulaTypes.ManhattanDistance:
                    distance = UtilityFunctions.CalculateManhattanDistance(square.Index, endingSquare.Index);
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
                    distance = UtilityFunctions.CalculateEuclidieanDistance(startSquare.Index, endSquare.Index);
                    break;
                case DistanceFormulaTypes.ManhattanDistance:
                    distance = UtilityFunctions.CalculateManhattanDistance(startSquare.Index, endSquare.Index);
                    break;
            }
            return distance;
        }
        
        
        private void OnApplicationQuit()
        {
            customEvent.onReset -= Reset;
            StopCoroutine(AStarPathfindingAlgorithm());
        }

        private void Reset()
        {
            return;
        }
    }
}

