using System;
using Unity.VisualScripting;
using UnityEngine;

public class CustomEvents : MonoBehaviour
{
    public event Action onReset;
    
    public void PublishOnReset()
    {
        onReset?.Invoke();
    }
}
