using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Unity;
using Utility;
using Random = System.Random;
using static GameCode.Constants;

namespace GameCode
{
    public class Square : MonoBehaviour
    {
        [SerializeField] private TextMeshPro WeightText = null;
        [SerializeField] private TextMeshPro FText = null;
        [SerializeField] private float weight = 0f;
        private static CustomEvents customEvent;
        private SquareTypes squareType;
        private Vector2Int index;
        private float g;
        private float h;

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
            private set
            {
                h = value;
            }
        }

        public float F
        {
            get => g + h;
        }

        public float Weight
        {
            get => weight;
            set
            {
                weight = value;
                UpdateText(WeightText, weight);
            }
        }

        public SquareTypes SquareType
        {
            get => squareType;
            set
            {
                squareType = value;
                customEvent.PublishOnColorUpdate(this);
            } 
        }

        private void OnValidate()
        {
            Weight = Mathf.Clamp(weight, 0f, float.MaxValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckTextLengthUnderMax(int value)
        {
            return maxNumberLenght > UtilityFunctions.GetLengthOfInt(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckTextLengthUnderMax(float value)
        {
            return maxNumberLenght > UtilityFunctions.GetLengthOfInt((int)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UpdateText(TextMeshPro textMeshPro, int value)
        {
            textMeshPro.text = CheckTextLengthUnderMax(value) ? $"{value}" : $"{(int)(Mathf.Pow(10, maxNumberLenght) - 1)}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UpdateText(TextMeshPro textMeshPro, float value)
        {
            textMeshPro.text = CheckTextLengthUnderMax(value) ? $"{(int)value}" : $"{(int)(Mathf.Pow(10, maxNumberLenght) - 1)}";
        }
    }

}