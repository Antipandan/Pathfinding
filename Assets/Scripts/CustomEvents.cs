using System;
using Unity.VisualScripting;
using UnityEngine;

public class CustomEvents : MonoBehaviour
{ 
    private event Action onReset;
    public event Action OnReset
    {
        add => onReset += value;
        remove => onReset -= value;
    }
    

    public void PublishOnReset()
    {
        onReset?.Invoke();
    }
    
}
