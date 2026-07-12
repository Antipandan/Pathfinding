using System;
using System.Collections.Generic;
using GameCode;
using Unity;
using UnityEngine;
using Utility;
using static GameCode.SquareTypes;

namespace GameCode
{
    public class DrawMap : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The color of Squares with squaretype 'WallSquare'.")]
        [SerializeField] private Color wallColor = Constants.squareColors[WallSquare];
        [Tooltip("The color of Squares with squaretype 'RegularSquare'.")]
        [SerializeField] private Color regularColor = Constants.squareColors[RegularSquare];
        [Space] 
        [Tooltip("The color of Squares with squaretype 'NeighbourSquare'.")]
        [SerializeField] private Color neighbourColor = Constants.squareColors[NeighbourSquare];
        [Tooltip("The color of Squares with squaretype 'FoundPathSquare'.")]
        [SerializeField] private Color foundPathColor = Constants.squareColors[FoundPathSquare];
        [Tooltip("The color of Squares with squaretype 'FinalPathSquare'.")]
        [SerializeField] private Color finalPathColor = Constants.squareColors[FinalPathSquare];
        [Space] 
        [Tooltip("The color of Squares with squaretype 'EndNodeSquare'.")]
        [SerializeField] private Color endNodeColor = Constants.squareColors[EndNodeSquare];
        [Tooltip("The color of Squares with squaretype 'StartNodeSquare'.")]
        [SerializeField] private Color startNodeColor = Constants.squareColors[StartNodeSquare];
        [Space]
        [Header("References (dont touch)")]
        [SerializeField] private CustomEvents customEvents;
        private Dictionary<SquareTypes, Color> squareColors = new Dictionary<SquareTypes, Color>(7);

        private void Awake()
        {
            customEvents.onColorUpdate += ChangeColor;
            Setup();
        }

        #region EssentialFunctions

        
        private void BuildDictionary()
        {
            squareColors = new Dictionary<SquareTypes, Color>
            {
                {WallSquare, wallColor},
                {RegularSquare, regularColor},
                {NeighbourSquare, neighbourColor},
                {FoundPathSquare, foundPathColor},
                {FinalPathSquare, finalPathColor},
                {EndNodeSquare, endNodeColor},
                {StartNodeSquare, startNodeColor}
            };
        }

        private void Setup()
        {
            BuildDictionary();
        }
        
        private void ChangeColor(Square square)
        {
            square.GetComponent<SpriteRenderer>().color = squareColors[square.SquareType];
        }
        #endregion

        [ExecuteAlways]
        private void OnValidate()
        {
            Setup();
            customEvents.PublishOnReset();
        }
    }
}
