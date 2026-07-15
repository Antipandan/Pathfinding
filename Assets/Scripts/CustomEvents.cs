using System;
using System.Collections.Generic;
using GameCode;
using UnityEngine;

public class CustomEvents : MonoBehaviour
{
    public event Action onReset;

    public event Action onPathfindingReset;

    public event Action<Square> onColorUpdate;

    public event Func<Square, List<Square>> onGetNeighbourSquares;

    public event Func<Square> onGetStartingSquare;
    
    public event Func<Square> onGetEndingSquare;
    
    public void PublishOnReset()
    {
        Debug.Log($"publishing on reset");
        onPathfindingReset?.Invoke();
        onReset?.Invoke();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    [ExecuteAlways]
    public void PublishOnColorUpdate(Square square)
    {
        onColorUpdate?.Invoke(square);
    }

    public List<Square> PublishOnGetNeighbourSquares(Square square)
    {
        return onGetNeighbourSquares?.Invoke(square);
    }

    public Square PublishOnGetStartingSquare()
    {
        return onGetStartingSquare?.Invoke();
    }

    public Square PublishOnGetEndingSquare()
    {
        return onGetEndingSquare?.Invoke();
    }
    
}
