using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using Utility;
using static GameCode.Constants;

namespace GameCode
{
    [ExecuteAlways]
    public class Square : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SquareTypes squareType = SquareTypes.RegularSquare;
        [SerializeField] private float weight = 0f;
        [SerializeField] private float g;
        [SerializeField] private float h;
        [Header("References (dont touch)")]
        [SerializeField] private TextMeshPro weightText = null;
        [SerializeField] private TextMeshPro fText = null;
        private static CustomEvents customEvent;
        private Vector2Int index;

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

        [ExecuteAlways]
        public SquareTypes SquareType
        {
            get => squareType;
            set
            {
                squareType = value;
                customEvent?.PublishOnColorUpdate(this);
            } 
        }

        public static CustomEvents CustomEvent
        {
            get => customEvent;
            set => customEvent = value;
        }

        #region UpdateText

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

        #endregion

        private void Start()
        {
            UpdateText(fText, F);
        }

        private void OnValidate()
        {
            Weight = Mathf.Clamp(weight, 0f, float.MaxValue);
            SquareType = SquareType;
        }

        public override string ToString()
        {
            return $"G: {G}, H: {H}, F: {F}, Weight: {Weight}, SquareType: {squareType}";
        }
    }

}