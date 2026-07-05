using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SquareUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshPro fText;
    [SerializeField] private TextMeshPro weightText;
    private Square square;
    
    public void UpdateFText(ushort newFValue)
    {
        fText.text = $"{newFValue}";
    }

    public void UpdateWeightText(ushort newWeight)
    {
        weightText.text = $"{newWeight}";
    }
}
