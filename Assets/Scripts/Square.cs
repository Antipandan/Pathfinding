using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity;
using Utility;
using Random = System.Random;

public class Square : MonoBehaviour
{
    [SerializeField] private uint weight = 0;
    [SerializeField] private SquareTypes squareType = SquareTypes.RegularSquare;

    public void RandomizeWeight(Random random, int maxVal)
    {
        RandomizeWeight(random, 0, maxVal);
    }

    public void RandomizeWeight(Random random, int minVal = 0, int maxVal = Int32.MaxValue)
    {
        if (maxVal < 0) throw new ArgumentOutOfRangeException(nameof(maxVal));
        weight = (uint) random.Next(minVal, maxVal);
    }
}
