using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEditor;

namespace GameCode
{
    public class GenerateMap : MonoBehaviour
    {
        [SerializeField] private int columns = 3;
        [SerializeField] private int rows = 3;
        [SerializeField] private Vector2Int startingPosition = Vector2Int.zero;
        [SerializeField] private Vector2Int endingPosition = Vector2Int.zero;
        [SerializeField] private GameObject squarePrefab;
        [SerializeField] private CustomEvents customEvents;

        private void Awake()
        {
            customEvents.onReset += Reset;
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            ClampDimensions();
        }

        private void ClampDimensions()
        {
            columns = (ushort)Mathf.Clamp(columns, 1, int.MaxValue);
            rows = (ushort)Mathf.Clamp(rows, 1, int.MaxValue);
        }

        private void Reset()
        {
            Setup();
        }

        private void OnDisable()
        {
            customEvents.onReset -= Reset;
        }

        private void OnEnable()
        {
            customEvents.onReset += Reset;
        }
        
        private void OnValidate()
        {
            if (startingPosition != endingPosition) return;
            startingPosition = Vector2Int.zero;
            endingPosition = new Vector2Int(columns - 1, rows - 1);
            
        }
        
    }
}

