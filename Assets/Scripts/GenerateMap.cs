using System;
using System.Collections.Generic;
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
            SubscribeToAllEvents();
            Setup();
            CreateMapHolder();
        }
        
        private void Start()
        {
            GenerateSquareMap();
        }
        
        private void Reset()
        {
            Setup();
            PreventFunctionsRunningInEditor(GenerateSquareMap);
            ReColorSquares(); 
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
        
        private void SetupSquares(Square[] existingSquares, Vector2 squareDimensions)
        {
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    Square currentSquare = existingSquares[x + x * y];
                    currentSquare.GetComponent<GameObject>().transform.SetParent(mapHolder, false);
                    SetupSquare(currentSquare, new Vector2Int(x, y), squareDimensions.x, squareDimensions.y);
                }
            } 
        }
        
        private void InitializeSquareValues(Square square, SquareTypes squareType = SquareTypes.RegularSquare)
        {
            Square.CustomEvent = customEvents;
            square.Weight = random.Next(minWeight, maxWeight + 1);
            square.G = square.Weight;
            square.H = 0f;
            square.SquareType = squareType;
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

        #endregion
        
        #region GenerateMapFunctions
        
        private void ReColorSquares()
        {
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    // kommer också att kalla metod som uppdaterar färgen via setter
                    squares[y, x].SquareType = squares[y, x].SquareType;
                }
            }
        }

        private void GenerateSquareMap()
        {
            Square[] existingObjects = FindObjectsByType<Square>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            if (existingObjects.Length == 0)
            {
                ResetBoard();
            }
            AssignStartEndSquare();
        }

        private void ResetBoard(bool instantiateObjects = true)
        {
            Vector2 squareDimensions = GetDimensionsOfSquarePrefab(squarePrefab);
            squares = new Square[columns, rows];
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    Square square = instantiateObjects ? Instantiate(squarePrefab, mapHolder).GetComponent<Square>() : squares[y, x];
                    SetupSquareProperly(square, new Vector2Int(x, y), squareDimensions.x, squareDimensions.y);
                }
            }
        }

        private void ResetBoard(Square[] existingSquares)
        {
            Vector2 squareDimensions = GetDimensionsOfSquarePrefab(squarePrefab);
            squares = new Square[columns, rows];
            if (existingSquares.Length > squares.Length)
            {
                SetupSquares(existingSquares, squareDimensions);
                DeleteExcessSquares(FindExcessSquares(existingSquares));
            }
            else if (existingSquares.Length < squares.Length)
            {
                
            }

            else
            {
                SetupSquares(existingSquares, squareDimensions);
            }
            
        }

        #endregion

        #region ShrinkExistingSquares

        private List<Square> FindExcessSquares(Square[] existingSquares)
        {
            
            List<Square> deltaSquares = new List<Square>();
            foreach (Square square in existingSquares)
            {
                for (int y = 0; y < columns; y++)
                {
                    for (int x = 0; x < rows; x++)
                    {
                        Square existingSquare = squares[y, x];
                        if (square != existingSquare) deltaSquares.Add(square);
                    }
                } 
            }
            return  deltaSquares;
        }
        

        private static void DeleteExcessSquares(List<Square> squaresToDelete)
        {
            for (int i = squaresToDelete.Count - 1; i >= 0; i--)
            {
                Destroy(squaresToDelete[i]);
                squaresToDelete.RemoveAt(i);
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
            // Längst upp i högra hörnet
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

