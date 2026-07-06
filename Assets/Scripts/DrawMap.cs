using System;
using System.Collections.Generic;
using GameCode;
using Unity;
using UnityEngine;
using Utility;
using static GameCode.SquareTypes;

namespace Gamecode
{
    public class DrawMap : MonoBehaviour
    {
        [SerializeField] private Color wallColor = Constants.squareColors[WallSquare];
        [SerializeField] private Color regularColor = Constants.squareColors[RegularSquare];
        [Space] 
        [SerializeField] private Color neighbourColor = Constants.squareColors[NeighbourSquare];
        [SerializeField] private Color foundPathColor = Constants.squareColors[FoundPathSquare];
        [Space] 
        [SerializeField] private Color endNodeColor = Constants.squareColors[EndNodeSquare];
        [SerializeField] private Color startNodeColor = Constants.squareColors[StartNodeSquare];
        [Space]
        [SerializeField] private CustomEvents customEvents;
        private Dictionary<SquareTypes, Color> squareColors = new Dictionary<SquareTypes, Color>(6);

        private void Awake()
        {
            customEvents.onColorUpdate += ChangeColor;
        }

        private void Start()
        {
            squareColors = new Dictionary<SquareTypes, Color>(6)
            {
                {WallSquare, wallColor},{RegularSquare, regularColor},{NeighbourSquare, neighbourColor},
                {FoundPathSquare, foundPathColor},{EndNodeSquare, endNodeColor}, {StartNodeSquare, startNodeColor}
            };
        }
        
        private void ChangeColor(Square square)
        {
            square.GetComponent<SpriteRenderer>().color = squareColors[square.SquareType];
        }
    }
}
