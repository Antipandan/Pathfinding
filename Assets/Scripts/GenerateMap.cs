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
        [SerializeField] [Range(0, 500)] int maxWeight = 15;
        [SerializeField] [Range(0, 499)] private int minWeight = 0;
        [SerializeField] private Vector2 padding = Vector2.zero;
        [SerializeField] private string seed = "Number or Text here!";
        [SerializeField] private Vector2Int startingPosition = Vector2Int.zero;
        [SerializeField] private Vector2Int endingPosition = new Vector2Int(2, 2);
        [SerializeField] private GameObject squarePrefab;
        [SerializeField] private CustomEvents customEvents;
        [SerializeField] private Transform mapHolder;
        private Square[,] squares;
        private Random random;

        private void Awake()
        {
            // jag vet inte om detta är en bra idé? Förmodligen inte
            SubscribeToAllEvents();
            Setup();
            CreateMapHolder();
        }

        private void SubscribeToAllEvents()
        {
            customEvents.onReset += Reset;
            customEvents.onGetNeighbourSquares += GetNeighbours;
            customEvents.onGetStartingSquare += GetStartingSquare;
            customEvents.onGetEndingSquare += GetEndingSquare;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Square GetStartingSquare()
        {
            return squares[startingPosition.y, startingPosition.x];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Square GetEndingSquare()
        {
            return squares[endingPosition.y, endingPosition.x];
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
                squares = new Square[columns, rows];
                for (int y = 0; y < columns; y++)
                {
                    for (int x = 0; x < rows; x++)
                    {
                        Square square = Instantiate(squarePrefab, mapHolder).GetComponent<Square>();
                        SetupSquareProperly(square);
                        square.transform.position = (squareDimensions + padding) * new Vector2(x, y);
                        square.Index = new Vector2Int(x, y);
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
            
            // Tänk som att vi målar ett baslager av färgen specificerad av regularSquare i drawMap
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    squares[y, x].SquareType = SquareTypes.RegularSquare;
                }
            }
            AssignStartEndSquare();
        }

        private List<Square> GetNeighbours(Square square)
        {
            Debug.Log($"getting neighbours from: {square.Index}");
            if (square == null) return null;
            List<Square> neighbours = new List<Square>();
            Vector2Int index = square.Index;
            if (index.x - 1 >= 0) AddSingleNeighbour(squares[index.y, index.x - 1], neighbours);
            if (index.x + 1 < rows) AddSingleNeighbour(squares[index.y, index.x + 1], neighbours);  
            if (index.y - 1 >= 0) AddSingleNeighbour(squares[index.y - 1, index.x], neighbours);
            if (index.y + 1 < columns) AddSingleNeighbour(squares[index.y + 1, index.x], neighbours);
            return neighbours;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AddSingleNeighbour(Square square, List<Square> neighbours)
        {
            if (square.SquareType != SquareTypes.WallSquare)
            {
                neighbours.Add(square);
            }
        }

        private void AssignStartEndSquare()
        {
            squares[startingPosition.y, startingPosition.x].SquareType = SquareTypes.StartNodeSquare;
            squares[endingPosition.y, endingPosition.x].SquareType = SquareTypes.EndNodeSquare;
        }

        private void SetupSquareProperly(Square square)
        {
            Square.CustomEvent = customEvents;
            square.Weight = random.Next(minWeight, maxWeight);
            square.G = square.Weight;
            square.H = 0f;
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
        }
    }
}

