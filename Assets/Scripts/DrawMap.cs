using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public class DrawMap : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color startNodeColor = Color.green;
    [SerializeField] private Color endNodeColor = Color.magenta;
    [SerializeField] private Color foundPathColor = Color.red;
    [SerializeField] private Color neightbourColor = Color.yellow;
    [SerializeField] private Color wallColor = Color.black;
    private GenerateMap generateMap;

    private void OnDrawGizmos()
    {
        foreach (var squareRow in generateMap.GetSquares)
        {
            
        }
    }
}
