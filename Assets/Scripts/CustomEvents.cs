using System;
using Unity.VisualScripting;
using UnityEngine;

public class CustomEvents : MonoBehaviour
{
    public static CustomEvents _instance;
    private event Action onReset;
    
    public event Action OnReset
    {
        add => onReset += value;
        remove => onReset -= value;
    }
    private void Start()
    {
        if (_instance != null) _instance = this;
        else Destroy(this);
    }

    public void PublishOnReset()
    {
        onReset?.Invoke();
    }
    
}
