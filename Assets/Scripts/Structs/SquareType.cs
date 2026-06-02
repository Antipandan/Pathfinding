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
        type = 0b0;
        foreach (SquareTypes squareType in squares)
        {
            type &= (int) squareType;
        }
    }

    public SquareType(params int[] ints)
    {
        type = 0b0;
        foreach (int i in ints)
        {
            type &= GetMostSignificantNumber(i);
        }
    }
    
    public int Type
    {
        get => type;
    }

    /// <summary>
    /// Calculates the Most significant bit and interprets that as a number.
    /// </summary>
    /// <param name="number">Number to get the most significant bit of</param>
    /// <returns>Returns 2 to the power of the index of the most significant
    /// bit e.g an input of 68 will return 64 because 2**6 = 64, where 6
    /// represents the index of the most significant bit</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once MemberCanBePrivate.Global
    public static int GetMostSignificantNumber(int number)
    {
        if (number <= 0) return 0;
        return (int)(Math.Log(number, 0b10));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SquareTypes GetDominantSquareType(int number)
    {
        return (SquareTypes)GetMostSignificantNumber(number);
    }
    
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
        return GetMostSignificantNumber(Type);
    }
    
    public override string ToString()
    {
        return $"Value: {Type} Base: {base.ToString()}";
    }
}
