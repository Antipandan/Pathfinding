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
        [SerializeField] private float searchDelay = 100f;
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
            Debug.Log($"ending square is now: {endingSquare.Index}");
            Debug.Log($"ending square weight: {endingSquare.Weight}");
            startingSquare = customEvent.PublishOnGetStartingSquare();
            openList.Add(startingSquare);
            StartCoroutine(AStarPathfindingAlgorithm());
        }

        private IEnumerator AStarPathfindingAlgorithm()
        {
            List<Square> neighbours = new List<Square>(); 
            while (openList.Count > 0)
            {
                Square cheapestSquare = FindCheapestSquare();
                openList.Remove(cheapestSquare);

                neighbours = customEvent.PublishOnGetNeighbourSquares(cheapestSquare);
                Debug.Log($"neighbours length: {neighbours.Count}");
                foreach (Square square in neighbours)
                {
                    Debug.Log($"finding position: {square.Index}");
                    Square neighbour = square;
                    // steg 1
                    if (neighbour == endingSquare)
                    {
                        closedList.Add(neighbour);
                        yield break;
                    }
                    // steg 2
                    neighbour.G = cheapestSquare!.G + cheapestSquare.Weight;
                    neighbour.H = CalculateDistance(neighbour, customEvent.PublishOnGetEndingSquare());
                    Debug.Log($"new H = {neighbour.H}");
                    
                    // steg 3 och 4
                    if (!DetermineIfSkip(neighbour))
                    {
                        openList.Add(neighbour);
                        TryUpdateSquare(neighbour, SquareTypes.NeighbourSquare);
                    }
                }
                closedList.Add(cheapestSquare);
                yield return new WaitForSeconds(searchDelay / 1000f);
            }

            Debug.Log($"exiting out of loop!");
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
            foreach (Square square in openList)
            {
                if (cheapestSquare == null || square.F < cheapestSquare.F)
                {
                    cheapestSquare = square;
                }
            }
            TryUpdateSquare(cheapestSquare, SquareTypes.FoundPathSquare);
            return cheapestSquare;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TryUpdateSquare(Square square, SquareTypes squareType)
        {
            if (square.SquareType < squareType) square.SquareType = squareType;
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

