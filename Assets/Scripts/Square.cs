using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Unity;
using Utility;
using Random = System.Random;

public class Square : MonoBehaviour
{
    [SerializeField] private TextMeshPro WeightText = null;
    [SerializeField] private TextMeshPro FText = null;
    private SquareTypes squareType;
    private Vector2Int index;
    private float weight;
    private float g;
    private float h;
    private static CustomEvents customEvent;

    public Vector2Int Index
    {
        get => index;
        set => index = value;
    }
    
    public float G
    {
        get => g;
        set => g = value;
    }

    public float H
    {
        get => h;
        private set => h = value;
    }

    public float F
    {
        get => g + h;
        private set
        {
            g = value;
            int maxLength = customEvent.PublishOnGetNumberLength();
        }
    }

    private static void UpdateText(TextMeshPro textMeshPro, string text)
    {
        textMeshPro.text = text;
    }
}
