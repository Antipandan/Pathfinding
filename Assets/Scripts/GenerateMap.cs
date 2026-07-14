using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Utility.UtilityFunctions;
using Random = System.Random;

namespace GameCode
{
    
    public class GenerateMap : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Represents the number of Squares in the X axis")]
        [SerializeField] private int rows = 3;
        [Tooltip("Represents the number of Squares in the Y axis")]
        [SerializeField] private int columns = 3;
        [Tooltip("Determines in what direction neighbours can be found")]
        [SerializeField] private ValidNeighbours validNeighbours = ValidNeighbours.HorizontalVertical;
        [Tooltip("The minimum randomly generated weight of Squares")]
        [SerializeField] [Range(0, 499)] private int minWeight = 0;
        [Tooltip("The ceiling for the maximum randomly generated weight of Squares")]
        [SerializeField] [Range(0, 500)] int maxWeight = 15;
        [Tooltip("Padding for placement of Square instances in the X and Y axis respectively")]
        [SerializeField] private Vector2 padding = Vector2.zero;
        [Tooltip("Seed to be used for the randomness engine. Same seed across multiple runs will generate the same result")]
        [SerializeField] private string seed = "Number or Text here!";
        [Tooltip("Position of the starting point for the Astar algorithm. Values will loopback when out of range. Range goes from 0 to rows - 1")]
        [SerializeField] private Vector2Int startingPosition = Vector2Int.zero;
        [Tooltip("Position of the ending point for the Astar algorithm. Values will loopback when out of range. Range goes from 0 to columns - 1")]
        [SerializeField] private Vector2Int endingPosition = new Vector2Int(2, 2);
        [Header("References (dont touch)")]
        [SerializeField] private GameObject squarePrefab;
        [SerializeField] private CustomEvents customEvents;
        [SerializeField] private Transform mapHolder;
        private Square[,] squares;
        private Random random;

        public int GetNrSquares
        {
            get => rows * columns;
        }

        private void Awake()
        {
            Square.CustomEvent = customEvents;
            Setup();
            CreateMapHolder();
        }
        
        private void Start()
        {
            SubscribeToAllEvents();
            GenerateSquareMap();
            ReColorSquares();
        }
        
        private void Reset()
        {
            Debug.Log($"reset from generate");
            Square.CustomEvent = customEvents;
            Setup();
            GenerateSquareMap();
            ReColorSquares(); 
        }

        private void OnEnable()
        {
            Square.CustomEvent = customEvents;
        }

        [ExecuteAlways]
        private void OnValidate()
        {
            Debug.Log($"validate and all the other stuff according to the W.H.O");
            Square.CustomEvent = customEvents;
            CheckIfPositionIsOutside(ref startingPosition);
            CheckIfPositionIsOutside(ref endingPosition);
            CheckIfPositionIsSame();
            customEvents.PublishOnReset();
        }
        
        #region EssentialFunctions

        private void SubscribeToAllEvents()
        {
            customEvents.onReset += Reset;
            customEvents.onGetNeighbourSquares += GetNeighbours;
            customEvents.onGetStartingSquare += GetStartingSquare;
            customEvents.onGetEndingSquare += GetEndingSquare;
        }
        
        private void Setup()
        {
            ClampDimensions();
            random = new Random(ParseSeed(seed));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private  Square IndexProperly(Square[] s, Vector2Int index)
        {
            return s[index.x + index.y * rows];
        }

        #endregion

        #region SetEssentialSquares

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
        
        private void AssignStartEndSquare()
        {
            squares[startingPosition.y, startingPosition.x].SquareType = SquareTypes.StartNodeSquare;
            squares[endingPosition.y, endingPosition.x].SquareType = SquareTypes.EndNodeSquare;
        }

        #endregion
        
        #region SetupSquareFunctions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateMapHolder()
        {
            if (mapHolder == null) Instantiate(new GameObject());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector2 GetDimensionsOfSquarePrefab(GameObject square)
        {
            if (square is null) return Vector2.zero;
            return square.GetComponent<SpriteRenderer>().bounds.size;
        }
        

        private void InitializeSquareValues(Square square)
        {
            square.Weight = random.Next(minWeight, maxWeight + 1);
            square.G = square.Weight;
            square.H = 0f;
        }
        
        private void InitializeSquareValues(Square square, SquareTypes newSquareType)
        {
            InitializeSquareValues(square);
            square.SquareType = newSquareType;
        }
        
        private void SetupSquare(Square square, Vector2Int index, float dimensionsX = 0f, float dimensionsY = 0f)
        {
            square.transform.position = (new Vector2(dimensionsX, dimensionsY) + padding) * new Vector2(index.x, index.y);
            square.Index = new Vector2Int(index.x, index.y);
            squares[index.y, index.x] = square;
        }

        private void SetupSquareProperly(Square square, Vector2Int index, float dimensionsX = 0f, float dimensionsY = 0f)
        {
            InitializeSquareValues(square);
            SetupSquare(square, index, dimensionsX, dimensionsY);
        }

        private void SetupSquareProperly(Square square, Vector2Int index, SquareTypes newSquareType,
            float dimensionsX = 0f, float dimensionsY = 0f)
        {
            InitializeSquareValues(square, newSquareType);
            SetupSquare(square, index, dimensionsX, dimensionsY);
        }

        #endregion
        
        #region GenerateMapFunctions
        

        private void ReColorSquares()
        {
            DoubleForLoop(new Vector2Int(columns, rows), InternalFunction);
            return;
            void InternalFunction(Vector2Int index)
            {
                squares[index.y, index.x].SquareType = squares[index.y, index.x].SquareType;

            }
        }

        private void GenerateSquareMap()
        {
            Square[] existingObjects = FindObjectsByType<Square>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
            if (existingObjects.Length == 0) ResetBoard();
            else if (existingObjects.Length == GetNrSquares) ResetBoard(existingObjects);
            AssignStartEndSquare();
        }

        private void ResetBoard()
        {
            Vector2 squareDimensions = GetDimensionsOfSquarePrefab(squarePrefab);
            squares = new Square[columns, rows];
            DoubleForLoop(new Vector2Int(columns, rows), LocalFunction, squareDimensions);
            return;

            void LocalFunction(Vector2Int index, Vector2 dimensions)
            {
                Square square = Instantiate(squarePrefab, mapHolder).GetComponent<Square>();
                SetupSquareProperly(square, index, dimensions.x, dimensions.y);
            }
        }

        private void ResetBoard(Square[] existingObjects)
        { 
            Vector2 squareDimensions = GetDimensionsOfSquarePrefab(squarePrefab);
            squares = new Square[columns, rows];
            DoubleForLoop(new Vector2Int(columns, rows), LocalFunction, squareDimensions);
            Flushboard();
            return;
            
            void LocalFunction(Vector2Int index, Vector2 dimensions)
            {
                Square square = IndexProperly(existingObjects, index);
                SetupSquareProperly(square, index, dimensions.x, dimensions.y);
            }
        }

        private void Flushboard()
        {
            foreach (Square square in squares)
            {
                if (square.SquareType != SquareTypes.WallSquare) square.SquareType = SquareTypes.RegularSquare;
            }
        }

        #endregion
        
        #region Neighbour
        
        public List<Square> GetNeighbours(Square square)
        {
            if (square is null) return null;
            switch (validNeighbours)
            {
                case ValidNeighbours.Horizontal:
                    return GetHorizontalNeighbours(square);
                case ValidNeighbours.Vertical:
                    return GetVerticalNeighbours(square);
                case ValidNeighbours.Diagonal:
                    return GetDiagonalNeigbours(square);
                case ValidNeighbours.HorizontalVertical:
                    return GetHorizontalVerticalNeighbours(square);
                case ValidNeighbours.HorizontalDiagonal:
                    return GetHorizontalDiagonalNeighbours(square);
                case ValidNeighbours.DiagonalVertical:
                    return GetDiagonalVerticalNeighbours(square);
                case ValidNeighbours.HorizontalVerticalDiagonal:
                    return GetHorizontalVerticalDiagonalNeigbours(square);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AddSingleNeighbour(Square square, List<Square> neighbours)
        {
            if (square.SquareType != SquareTypes.WallSquare)
            {
                neighbours.Add(square);
            }
        }
        
        private List<Square> GetHorizontalNeighbours(Square square)
        {
            List<Square> neighbours = new List<Square>();
            Vector2Int index = square.Index;
            if (index.x - 1 >= 0) AddSingleNeighbour(squares[index.y, index.x - 1], neighbours);
            if (index.x + 1 < rows) AddSingleNeighbour(squares[index.y, index.x + 1], neighbours);
            return neighbours;
        }

        private List<Square> GetVerticalNeighbours(Square square)
        {
            List<Square> neighbours = new List<Square>();
            Vector2Int index = square.Index;
            if (index.y - 1 >= 0) AddSingleNeighbour(squares[index.y - 1, index.x], neighbours);
            if (index.y + 1 < columns) AddSingleNeighbour(squares[index.y + 1, index.x], neighbours);
            return neighbours;
        }

        private List<Square> GetDiagonalNeigbours(Square square)
        {
            List<Square> neighbours = new List<Square>();
            Vector2Int index = square.Index;
            if (index.y + 1 < columns && index.x + 1 < rows) AddSingleNeighbour(squares[index.y + 1, index.x + 1], neighbours);
            if (index.y + 1 < columns && index.x - 1 >= 0) AddSingleNeighbour(squares[index.y + 1, index.x - 1], neighbours);
            if (index.y - 1 >= 0 && index.x - 1 >= 0) AddSingleNeighbour(squares[index.y - 1, index.x - 1], neighbours);
            if (index.y - 1 >= 0 && index.x + 1 < rows) AddSingleNeighbour(squares[index.y - 1, index.x + 1], neighbours);
            return neighbours;
        }

        private List<Square> GetHorizontalVerticalNeighbours(Square square)
        {
            List<Square> neighbours = new List<Square>();
            neighbours.AddRange(GetHorizontalNeighbours(square));
            neighbours.AddRange(GetVerticalNeighbours(square));
            return neighbours;
        }

        private List<Square> GetHorizontalDiagonalNeighbours(Square square)
        {
            List<Square> neighbours = new List<Square>();
            neighbours.AddRange(GetDiagonalNeigbours(square));
            neighbours.AddRange(GetHorizontalNeighbours(square));
            return neighbours;
        }

        private List<Square> GetDiagonalVerticalNeighbours(Square square)
        {
            List<Square> neighbours = new List<Square>();
            neighbours.AddRange(GetDiagonalNeigbours(square));
            neighbours.AddRange(GetVerticalNeighbours(square));
            return neighbours;
        }

        private List<Square> GetHorizontalVerticalDiagonalNeigbours(Square square)
        {
            List<Square> neighbours = new List<Square>();
            neighbours.AddRange(GetHorizontalNeighbours(square));
            neighbours.AddRange(GetVerticalNeighbours(square));
            neighbours.AddRange(GetDiagonalNeigbours(square));
            return neighbours;
        }
        #endregion
        
        #region ClampInspectorValues

        private void ClampDimensions()
        {
            columns = Mathf.Clamp(columns, 1, int.MaxValue);
            rows = Mathf.Clamp(rows, 1, int.MaxValue);
        }
        
        private void CheckIfPositionIsOutside(ref Vector2Int position)
        {
            Vector2Int newPosition = new Vector2Int(position.x % rows, position.y % columns);
            if (newPosition.x < 0) newPosition.x += rows; 
            if (newPosition.y < 0) newPosition.y += columns;
            position = newPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckIfPositionIsSame()
        {
            if (startingPosition.y != endingPosition.y || startingPosition.x != endingPosition.x) return;
            startingPosition = Vector2Int.zero;
            endingPosition = new Vector2Int(columns - 1, rows - 1);
        }

        #endregion
        
    }
}

