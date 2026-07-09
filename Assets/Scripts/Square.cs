using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Unity;
using UnityEngine.Serialization;
using Utility;
using Random = System.Random;
using static GameCode.Constants;

namespace GameCode
{
    public class Square : MonoBehaviour
    {
        [SerializeField] private TextMeshPro weightText = null;
        [SerializeField] private TextMeshPro fText = null;
        [SerializeField] private float weight = 0f;
        private static CustomEvents customEvent;
        private SquareTypes squareType = SquareTypes.RegularSquare;
        [SerializeField] private Vector2Int index;
        [SerializeField] private float g;
        [SerializeField] private float h;
        [SerializeField] private Square parentSquare = null;

        public Vector2Int Index
        {
            get => index;
            set => index = value;
        }

        public float G
        {
            get => g;
            set
            {
                g = value;
                UpdateText(fText, F);
            }
        }

        public float H
        {
            get => h;
            set
            {
                h = value;
                UpdateText(fText, F);
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
                UpdateText(weightText, weight);
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

        public static CustomEvents CustomEvent
        {
            get => customEvent;
            set => customEvent = value;
        }

        public Square ParentSquare
        {
            get => parentSquare;
            set => parentSquare = value;
        }

        private void Start()
        {
            UpdateText(fText, F);
        }

        private void OnValidate()
        {
            Weight = Mathf.Clamp(weight, 0f, float.MaxValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckTextLengthUnderMax(int value)
        {
            return maxNumberLenght > UtilityFunctions.GetLengthOfNumber(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckTextLengthUnderMax(float value)
        {
            return maxNumberLenght > UtilityFunctions.GetLengthOfNumber(value);
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