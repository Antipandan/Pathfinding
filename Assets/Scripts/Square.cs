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
    [SerializeField] private TextMeshPro fText;
    [SerializeField] private TextMeshPro weightText;
    
    private uint g;
    private uint h;
    private uint weight;
    private SquareType typesSquaresSquare;
    private Vector2Int squarePosition = Vector2Int.zero;

    public Square(Vector2Int squarePosition, int maxWeight, params SquareTypes[] types)
    {
        ModifySquare(squarePosition, maxWeight, types);
    }

    public Square()
    {
        ModifySquare(Vector2Int.zero, 0, SquareTypes.RegularSquare);
    }
    
    public uint Weight
    {
        // liten work around men jag bryr mig inte längre direkt!
        get
        {
            if (typesSquaresSquare.HasSquareType(SquareTypes.StartNodeSquare) || 
                    typesSquaresSquare.HasSquareType(SquareTypes.EndNodeSquare)) return 0;
            return weight;
        }
    }
    
    // denna ger en kopia inte en referens!!!!
    public SquareType TypesSquare
    {
        get => typesSquaresSquare;
        set => typesSquaresSquare = value;
    }

    public Vector2Int SquarePosition
    {
        get => squarePosition;
    }

    public uint G
    {
        get => g;
        set => g = value;
    }

    public uint H
    {
        get => h;
        set => h = value;
    }

    public uint F
    {
        get => g + h;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ModifySquare(Vector2Int squarePosition, int maxWeight, params SquareTypes[] types)
    {
        this.squarePosition = squarePosition;
        this.typesSquaresSquare = new SquareType(types);
        this.weight = (uint) maxWeight;
        this.g = weight;
        this.h = 0;
    }

    public void AddMoreTypes(params SquareTypes[] types)
    {
        typesSquaresSquare.TryAddMoreTypes(types);
    }
    

    public void ChangeWeightText(uint newValue)
    {
        weightText.text = newValue.ToString();
    }
}
