using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;


public class DrawMap : MonoBehaviour
{
    [SerializeField] private GenerateMap generateMap;
    [Space]
    [SerializeField] private Color regularColor = Color.white;
    [SerializeField] private Color wallColor = Color.black;
    [Space]
    [SerializeField] private Color neighbourColor = Color.yellow;
    [SerializeField] private Color foundPathColor = Color.red;
    [Space]
    [SerializeField] private Color endNodeColor = Color.magenta;
    [SerializeField] private Color startNodeColor = Color.green;

    private void Awake()
    {
        if (generateMap == null) Debug.LogError($"Warning no reference to {nameof(generateMap)}!");
    }

    private void OnDrawGizmos()
    {
        foreach (var square in generateMap.GetSquares)
        {
            Vector2Int pos = square.SquarePosition;
            Gizmos.DrawCube(new Vector3(pos.x, pos.y, 0f), Vector3.one);
        }
    }
}
