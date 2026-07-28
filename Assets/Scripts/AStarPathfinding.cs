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
        [Tooltip("should the Algorithm automatically restart when it's finished?")]
        [SerializeField] private bool restartOnEnd = false;
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
        private HashSet<Square> openList;
        private HashSet<Square> closedList;
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
            startingSquare = customEvent.PublishOnGetStartingSquare();
            endingSquare = customEvent.PublishOnGetEndingSquare();
            openList.Add(startingSquare);
            SetupStartingSquare();
            StartCoroutine(AStarPathfindingAlgorithm());
        }

        private void SetupStartingSquare()
        {
            startingSquare.G = 0;
            startingSquare.H = CalculateDistance(startingSquare, endingSquare);
        }

        private void SubscribeToEvents()
        {
            customEvent.onReset += Reset;
            customEvent.onPathfindingReset += Stop;
        }

        private void Stop()
        {
            StopAllCoroutines();
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
            openList = new HashSet<Square>();
            closedList = new HashSet<Square>();
            if (customEvent == null)
            {
                Debug.LogWarning($"Warning! Reference to {nameof(customEvent)} is null!", this);
            }
        }

        #endregion

        #region AStarPathfindingAlgorithm

        private IEnumerator AStarPathfindingAlgorithm()
        {
            startingSquare.G = 0;
            startingSquare.H = CalculateDistance(startingSquare, endingSquare);
            bool foundPath = false;
            while (openList.Count > 0 && !foundPath)
            {
                Square cheapestSquare = FindCheapestSquare();
                if (cheapestSquare == endingSquare)
                {
                    foundPath = true;
                    break;
                }
                if (cheapestSquare is null) yield break;
                openList.Remove(cheapestSquare);
                closedList.Add(cheapestSquare);
                List<Square> neighbours = customEvent.PublishOnGetNeighbourSquares(cheapestSquare);
                // neighbours = FilterOutNeighbours(neighbours);
                foreach (Square currentNeighbour in neighbours)
                {
                    
/*                    if (square == endingSquare)
                    {
                        closedList.Add(square);
                        foundPath = true;
                    }
                    */
                    if (closedList.Contains(currentNeighbour)) continue;
                    float tentativeG = currentNeighbour.Weight + cheapestSquare.G;
                    if (tentativeG < currentNeighbour.G)
                    {
                        Debug.Log($"lower");
                        currentNeighbour.G = tentativeG;
                        currentNeighbour.H = CalculateDistance(currentNeighbour, endingSquare);
                        currentNeighbour.ParentSquare = cheapestSquare;
                    }
                    openList.Add(currentNeighbour);
                    /*if (!DetermineIfSkip(square))
                    {
                        openList.Add(square);
                        TryUpdateSquare(square, SquareTypes.NeighbourSquare);
                    }*/
                }
                yield return new WaitForSeconds(aStarSearchDelay / 1000f);
            }
            if (foundPath)
            {
                Debug.Log($"path found!");
                StartCoroutine(TraceBackPath());
            }
            else
            {
                if (!restartOnEnd) yield break;
                Debug.Log($"no squares found!");
                customEvent.PublishOnReset();
            }
        }
        
        private Square FindCheapestSquare()
        {
            Square cheapestSquare = null;
            List<Square> sameFValues = new List<Square>();
            foreach (Square square in openList)
            {
                if (cheapestSquare is null)
                {
                    cheapestSquare = square;
                    sameFValues.Add(cheapestSquare);
                }
                else if (Mathf.Approximately(square.F, cheapestSquare.F)) sameFValues.Add(square);
                else if (square.F < cheapestSquare.F)
                {
                    sameFValues.Clear();
                    cheapestSquare = square;
                    sameFValues.Add(cheapestSquare);
                }
            }
            if (sameFValues.Count > 1)
            {
                cheapestSquare = internalHelperFunction(sameFValues);
            }
            TryUpdateSquare(cheapestSquare, SquareTypes.FoundPathSquare);
            return cheapestSquare;
            
            static Square internalHelperFunction(List<Square> sameFValues)
            {
                List<Square> sameGValues = InternalFindCheapestGSquares(sameFValues);
                if (sameGValues.Count <= 1) return sameGValues[0];
                List<Square> sameHValues = InternalFindCheapestH(sameGValues);
                if (sameHValues.Count <= 1) return sameHValues[0];
                List<Square> sameWeightValues = InternalFindCheapestWeightValue(sameHValues);
                return sameWeightValues[0];
            }
            
            // refactor o'clock??
            static List<Square> InternalFindCheapestGSquares(List<Square> sameFValues)
            {
                List<Square> sameGValues = new List<Square>();
                foreach (Square square in sameFValues)
                {
                    if (sameGValues.Count == 0) sameGValues.Add(square);
                    else if (Mathf.Approximately(square.G, sameGValues[0].G)) sameGValues.Add(square);
                    else if (square.G < sameGValues[0].G)
                    {
                        sameGValues.Clear();
                        sameGValues.Add(square);
                    }
                }
                return sameGValues;
            }

            static List<Square> InternalFindCheapestH(List<Square> sameGValues)
            {
                List<Square> sameHValues = new List<Square>();
                foreach (Square square in sameGValues)
                {
                    if (sameGValues.Count == 0) sameHValues.Add(square);
                    else if (Mathf.Approximately(square.H, sameGValues[0].H)) sameHValues.Add(square);
                    else if (square.H < sameGValues[0].H)
                    {
                        sameHValues.Clear();
                        sameHValues.Add(square);
                    }
                }
                return sameHValues;
            }

            static List<Square> InternalFindCheapestWeightValue(List<Square> sameHValues)
            {
                List<Square> sameWeightValues = new List<Square>();
                foreach (Square square in sameHValues)
                {
                    if (sameWeightValues.Count == 0) sameWeightValues.Add(square);
                    else if (Mathf.Approximately(square.Weight, sameHValues[0].H)) sameWeightValues.Add(square);
                    else if (square.Weight < sameHValues[0].H)
                    {
                        sameWeightValues.Clear();
                        sameWeightValues.Add(square);
                    }
                }
                return sameWeightValues;
            }
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
            return skip;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void TryUpdateSquare(Square square, SquareTypes squareType)
        {
            if (square?.SquareType < squareType) square.SquareType = squareType;
        }

        #endregion

        #region TraceBackAlgorithm

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator TraceBackPath()
        {
            HashSet<Square> visitedSquares = new HashSet<Square>();
            Square currentSquare = endingSquare;
            // UpdateSingleTraceSquare(currentSquare, visitedSquares);
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
            Debug.Log($"Total cost for path: {CalculatePathCost(visitedSquares)}");
            if (restartOnEnd) customEvent.PublishOnReset();
        }

        private long CalculatePathCost(HashSet<Square> squares)
        {
            long total = 0;
            foreach (Square square in squares)
            {
                total += (long)square.Weight;
            }
            return total;
        }
        
        private static Square FindCheapestGSquare(List<Square> squares)
        {
            Square cheapestSquare = null;
            foreach (Square square in squares)
            {
                if (cheapestSquare is null) cheapestSquare = square;
                else if (Mathf.Approximately(square.G, cheapestSquare.G)) cheapestSquare = FindMostExpensiveHSquare(squares);
                else if (cheapestSquare.G > square.G) cheapestSquare = square;
            }
            return cheapestSquare;
        }

        private static Square FindMostExpensiveHSquare(List<Square> squares)
        {
            Square cheapestSquare = null;
            foreach (Square square in squares)
            {
                if (cheapestSquare is null || square.H > cheapestSquare.H) cheapestSquare = square;
                else if (Mathf.Approximately(cheapestSquare.H, square.H)) cheapestSquare = FindCheapestFSquare(squares);
            }
            return cheapestSquare;
        }

        private static Square FindCheapestFSquare(List<Square> squares)
        {
            Square cheapestSquare = null;
            foreach (Square square in squares)
            {
                if (cheapestSquare is null || cheapestSquare.F > square.F) cheapestSquare = square ;
            }
            return cheapestSquare;
        }
        
        private void UpdateSingleTraceSquare(Square square, HashSet<Square> visitedSquares)
        {
            if (square is null) return;
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

