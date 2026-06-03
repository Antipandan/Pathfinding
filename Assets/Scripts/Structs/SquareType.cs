using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity;
using Unity.Mathematics;
using UnityEngine;

public struct SquareType
{
    private int type;
    public SquareType(params SquareTypes[] squares)
    {
        this.type = 0b0;
        foreach (SquareTypes squareType in squares)
        {
            this.type |= (int) squareType;
        }
    }

    public SquareType(params int[] ints)
    {
        this.type = 0b0;
        foreach (int i in ints)
        {
            this.type &= GetMostSignificantNumber(i);
        }
    }
    
    
    public int GetType
    {
        get => this.type;
    }

    /// <summary>
    /// Checks If the number / bit of a 'SquareTypes' is included in type of said 'SquareType'
    /// </summary>
    /// <param name="desiredType">'SquareTypes' to search for in 'type'</param>
    /// <returns></returns>
    public bool HasSquareType(SquareTypes desiredType)
    {
        int bitIndex = (int)desiredType;
        return ((type & bitIndex) >> (bitIndex - 1) & 0b1) == 0b1;
    }

    /// <summary>
    /// Calculates the Most significant bit and interprets that as a number.
    /// </summary>
    /// <param name="number">Number to get the most significant bit of</param>
    /// <returns>Returns 2 to the power of the index of the most significant
    /// bit e.g an input of 68 will return 64 because 2**6 = 64, where 6
    /// represents the index of the most significant bit</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetMostSignificantNumber(int number)
    {
        if (number <= 0) return 0;
        // Debug.Log($"number: {Math.Log(number, 2)}, incoming number: {number}");
        return (int)(Math.Log(number, 2));
    }
    
    /// <summary>
    /// Calculates the Most significant bit and interprets that as a number.
    /// </summary>
    /// <param name="number">Number to the exponent from</param>>
    /// <returns>Returns 2 to the power of the index of the most significant
    /// bit e.g an input of 68 will return 64 because 2**6 = 64, where 6
    /// represents the index of the most significant bit. Returns the exponent as a 'SquareTypes'</returns>

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SquareTypes GetDominantSquareType(int number)
    {
        return (SquareTypes)GetMostSignificantNumber(number);
    }
    
    /// <summary>
    /// Calculates the Most significant bit and interprets that as a number.
    /// </summary>
    /// <returns>Returns 2 to the power of the index of the most significant
    /// bit e.g an input of 68 will return 64 because 2**6 = 64, where 6
    /// represents the index of the most significant bit. Returns the exponent as a 'SquareTypes'</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SquareTypes GetDominantSquareType()
    {
        return GetDominantSquareType(type);
    }

    /// <summary>
    /// Calculates the Most significant bit and interprets that as a number.
    /// </summary>
    /// <returns>Returns 2 to the power of the index of the most significant
    /// bit e.g an input of 68 will return 64 because 2**6 = 64, where 6
    /// represents the index of the most significant bit</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetMostSignificantNumber()
    {
        return GetMostSignificantNumber(GetType);
    }
    
    public override string ToString()
    {
        return $"Value: {GetType} Base: {base.ToString()}";
    }
}
