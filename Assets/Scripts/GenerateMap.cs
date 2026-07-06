using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity;
using UnityEditor;
using Random = System.Random;
using static Utility.UtilityFunctions;

namespace GameCode
{
    public class GenerateMap : MonoBehaviour
    {
        [SerializeField] private int columns = 3;
        [SerializeField] private int rows = 3;
        [SerializeField] [Range(0, 500)] int maxWeigth = 15;
        [SerializeField] private Vector2 padding = Vector2.zero;
        [SerializeField] private string seed = "Number or Text here!";
        [SerializeField] private Vector2Int startingPosition = Vector2Int.zero;
        [SerializeField] private Vector2Int endingPosition = Vector2Int.zero;
        [SerializeField] private GameObject squarePrefab;
        [SerializeField] private CustomEvents customEvents;
        [SerializeField] private Transform mapHolder;
        private Square[,] squares;
        private Random random;

        private void Awake()
        {
            // jag vet inte om detta är en bra idé? Förmodligen inte
            Square.CustomEvent = customEvents;
            SubscribeToAllEvents();
            Setup();
            CreateMapHolder();
        }

        private void SubscribeToAllEvents()
        {
            customEvents.onReset += Reset;
        }

        private void Start()
        {
            GenerateSquareMap();
        }

        private void Setup()
        {
            ClampDimensions();
            random = new Random(ParseSeed(seed));
        }

        private void ClampDimensions()
        {
            columns = (ushort)Mathf.Clamp(columns, 1, int.MaxValue);
            rows = (ushort)Mathf.Clamp(rows, 1, int.MaxValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateMapHolder()
        {
            if (mapHolder == null) Instantiate(new GameObject());
        }

        private void GenerateSquareMap()
        {
            Square[] existingObjects = FindObjectsByType<Square>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            Vector2 squareDimensions = squarePrefab.GetComponent<SpriteRenderer>().bounds.size;
            if (existingObjects.Length == 0)
            {
                squares = new Square[rows, columns];
                for (int y = 0; y < columns; y++)
                {
                    for (int x = 0; x < rows; x++)
                    {
                        Square square = Instantiate(squarePrefab, mapHolder).GetComponent<Square>();
                        SetupSquareProperly(square);
                        square.transform.position = (squareDimensions + padding) * new Vector2(x, y);
                        squares[y, x] = square;
                    }
                }
            }
            else if (existingObjects.Length > rows * columns)
            {
                return;
            }
            
            else if (existingObjects.Length < rows * columns)
            {
                return;
            }
            // detta fungerar men att inkludera den inre koden nedan i den tidigare for loopen fungerar inte?
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    squares[y, x].SquareType = SquareTypes.RegularSquare;
                }
            }
            AssignStartEndSquare();
        }

        private void AssignStartEndSquare()
        {
            squares[startingPosition.y, startingPosition.x].SquareType = SquareTypes.StartNodeSquare;
            squares[endingPosition.y, endingPosition.x].SquareType = SquareTypes.EndNodeSquare;
        }

        private void SetupSquareProperly(Square square)
        {
            square.Weight = random.Next(0, maxWeigth);
        }
        

        private void Reset()
        {
            Setup();
            PreventFunctionsRunningInEditor(GenerateSquareMap);
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
            customEvents.PublishOnReset();
        }
    }
}

