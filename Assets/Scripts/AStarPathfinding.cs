using System;
using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(AStarPathfindingAlgorithm());
        }

        private IEnumerator AStarPathfindingAlgorithm()
        {
            int testIterations = 25;
            for (int i = 0; i <= testIterations; i++)
            {
                yield return new WaitForSeconds(searchDelay);
            }
        }

        private void OnDisable()
        {
            StopCoroutine(AStarPathfindingAlgorithm());
        }

        private void OnEnable()
        {
            StartCoroutine(AStarPathfindingAlgorithm());
            Setup();
            customEvent.PublishOnReset();
        }

        private void Reset()
        {
            customEvent.PublishOnReset();
        }
    }
}

