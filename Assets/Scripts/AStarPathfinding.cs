using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace GameCode
{
    public class AStarPathfinding : MonoBehaviour
    {
        [SerializeField] private GenerateMap map;
        [Tooltip("delay in milliseconds (ms)")]
        [SerializeField] private float searchDelay = 100f;
    }
}

