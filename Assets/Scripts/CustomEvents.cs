using System;
using GameCode;
using Unity.VisualScripting;
using UnityEngine;

public class CustomEvents : MonoBehaviour
{
    public event Action onReset;
    
    public event Action<Square, SquareTypes> onSquareUpdate;

    public event Action<Square> onColorUpdate;
    
    public void PublishOnReset()
    {
        onReset?.Invoke();
    }

    public void PublishOnSquareUpdate(Square square, SquareTypes newType)
    {
        onSquareUpdate?.Invoke(square, newType);
        onColorUpdate?.Invoke(square);
    }

    public void PublishOnColorUpdate(Square square)
    {
        onColorUpdate?.Invoke(square);
    }
}
