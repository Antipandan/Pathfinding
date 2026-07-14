using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utility;

// ReSharper disable once CheckNamespace
namespace GameCode
{
    public class AStarPathfinding : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Determines if the Traceback algorithm should color ending and starting squares")]
        [SerializeField] private bool colorEntirePath = false;
        [Tooltip("Which distance formula to use when calculating the distance and new H value for Squares")]
        [SerializeField] private DistanceFormulaTypes distanceFormula = DistanceFormulaTypes.ManhattanDistance;
        [Tooltip("Puts extra emphasis on the H value of a square, potentially leading to shorter paths")]
        [SerializeField] [Range(0, ushort.MaxValue)] private ushort HeuristicMultiplier = 1;
        [Tooltip("The delay for having found a treversable square in the Astar algorithm. Delay in milliseconds (ms)")]
        [SerializeField] private float aStarSearchDelay = 100f;
        [Tooltip("The delay for having found a treversable square in the Traceback algorithm. Delay in milliseconds (ms)")]
        [SerializeField] private float tracingSearchDelay = 100f;
        [Header("References (dont touch)")]
        [SerializeField] private CustomEvents customEvent;
        private List<Square> openList;
        private List<Square> closedList;
        private Square startingSquare;
        private Square endingSquare;

        private void Awake()
        {
            Setup();
        }
        
        private void Start()
        {
            endingSquare = customEvent.PublishOnGetEndingSquare();
            startingSquare = customEvent.PublishOnGetStartingSquare();
            openList.Add(startingSquare);
            SubscribeToEvents();
            StartCoroutine(AStarPathfindingAlgorithm());
            
        }

        #region EssentialFunctions

        private void Reset()
        {
            openList.Clear();
            closedList.Clear();
            openList.Add(startingSquare);
            StopCoroutine(AStarPathfindingAlgorithm());
            StartCoroutine(AStarPathfindingAlgorithm());
        }

        private void SubscribeToEvents()
        {
            customEvent.onReset += Reset;
        }

        private int CalculateDistance(Square startSquare, Square endSquare)
        {
            switch (distanceFormula)
            {
                case DistanceFormulaTypes.EuclidianDistance:
                    return UtilityFunctions.CalculateEuclidieanDistance(startSquare.Index, endSquare.Index) * HeuristicMultiplier;
                case DistanceFormulaTypes.ManhattanDistance:
                    return UtilityFunctions.CalculateManhattanDistance(startSquare.Index, endSquare.Index) * HeuristicMultiplier;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        #endregion

        #region AStarPathfindingAlgorithm

        private IEnumerator AStarPathfindingAlgorithm()
        {
            bool foundPath = false;
            while (openList.Count > 0 && !foundPath)
            {
                Square cheapestSquare = FindCheapestSquare();
                if (cheapestSquare is null)
                {
                    yield break;
                } 
                openList.Remove(cheapestSquare);

                List<Square> neighbours = customEvent.PublishOnGetNeighbourSquares(cheapestSquare);
                neighbours = FilterOutNeighbours(neighbours);
                foreach (Square square in neighbours)
                {
                    // steg 1
                    if (square == endingSquare)
                    {
                        closedList.Add(square);
                        foundPath = true;
                    }
                    // steg 2
                    square.G += cheapestSquare.G;
                    square.H = CalculateDistance(square, endingSquare);
                    // steg 3 och 4
                    if (!DetermineIfSkip(square))
                    {
                        openList.Add(square);
                        TryUpdateSquare(square, SquareTypes.NeighbourSquare);
                    }
                }
                closedList.Add(cheapestSquare);
                yield return new WaitForSeconds(aStarSearchDelay / 1000f);
            }
            if (foundPath) StartCoroutine(TraceBackPath());
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void TryUpdateSquare(Square square, SquareTypes squareType)
        {
            if (square.SquareType < squareType) square.SquareType = squareType;
        }

        #endregion

        #region TraceBackAlgorithm

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator TraceBackPath()
        {
            HashSet<Square> visitedSquares = new HashSet<Square>();
            Square currentSquare = endingSquare;
            UpdateSingleTraceSquare(currentSquare, visitedSquares);
            while (currentSquare is not null &&  currentSquare != startingSquare)
            {
                UpdateSingleTraceSquare(currentSquare, visitedSquares);
                List<Square> neighbours = customEvent.PublishOnGetNeighbourSquares(currentSquare);
                List<Square> borderingNeighbours = new List<Square>();
                foreach (Square neighbour in neighbours)
                {
                    if (closedList.Contains(neighbour) && !visitedSquares.Contains(neighbour))
                    {
                        borderingNeighbours.Add(neighbour);
                    }
                }
                
                currentSquare = FindCheapestGSquare(borderingNeighbours);
                yield return new WaitForSeconds(tracingSearchDelay / 1000f);
            }
            UpdateSingleTraceSquare(currentSquare, visitedSquares);
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
        
        private void UpdateSingleTraceSquare(Square square, HashSet<Square> visitedSquares)
        {
            visitedSquares.Add(square);
            if (colorEntirePath || square.SquareType < SquareTypes.FinalPathSquare)
            {
                square.SquareType = SquareTypes.FinalPathSquare;
            }
        }

        #endregion

        private void OnDisable()
        {
            StopCoroutine(AStarPathfindingAlgorithm());
        }

        private void OnEnable()
        {
            Setup();
            StartCoroutine(AStarPathfindingAlgorithm());
        }

        private void OnValidate()
        {
            customEvent.PublishOnReset();
        }

        private void OnApplicationQuit()
        {
            StopCoroutine(AStarPathfindingAlgorithm());
        }
    }
}

