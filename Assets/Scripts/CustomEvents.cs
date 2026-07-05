using System;
using Unity.VisualScripting;
using UnityEngine;

public class CustomEvents : MonoBehaviour
{
    public event Action onReset;

    public event Func<int> onGetNumberLength;
    
    public void PublishOnReset()
    {
        onReset?.Invoke();
    }
    
    public int PublishOnGetNumberLength()
    {
        return onGetNumberLength!.Invoke();
    }
}
