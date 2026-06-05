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
        AssignNewType(squares);
    }

    public SquareType(params int[] ints)
    {
        this.type = 0b0;
        AssignNewType(ints);
    }
    
    public int GetType
    {
        get => this.type;
    }

    /// <summary>
    /// Takes the int value of the newTypes and applies them to 'this.type'
    /// </summary>
    /// <param name="newTypes">Collection of types to be added to 'this.type'</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssignNewType(params SquareTypes[] newTypes)
    {
        foreach (SquareTypes newType in newTypes)
        {
            AssignSingleNewType(newType);
        }
    }

    /// <summary>
    /// Takes the int value, finds the MSB and applies them to 'this.type'.
    /// </summary>
    /// <param name="ints">Integers representing different squares</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssignNewType(params int[] ints)
    {
        foreach (int i in ints)
        {
            AssignSingleNewType(i);
        }
    }

    /// <summary>
    /// Try and add more types. Designed to be used externally when modifying value of struct.
    /// Will not add certain types to others
    /// </summary>
    /// <param name="squareType">Collection of types to be added to 'this.type'</param>
    public void TryAddMoreTypes(params SquareTypes[] squareType)
    {
        foreach (SquareTypes newType in squareType)
        {
            if (!Constants.NonMixableSquareTypes.Contains(newType))
            {
                Debug.Log($"assigning new type!");
                AssignNewType(newType);
            }
        }
    }

    /// <summary>
    /// Perform an OR operation on 'this.type' and int value of 'newType' to integrate its value into 'this.type'
    /// <example><c>(0b1000 |= 0b0010) = 0b1010</c></example>
    /// </summary>
    /// <param name="newType">SquareTypes to integrate into 'this.type'</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssignSingleNewType(SquareTypes newType)
    {
        type |= (int)newType;
    }

    /// <summary>
    /// Perform an OR operation on 'this.type' and the integer value to integrate its value into 'this.type'
    /// <example><c>(0b1000 |= 0b0010) = 0b1010</c></example>
    /// </summary>
    /// <param name="i">Integer to integrate into 'this.type'</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssignSingleNewType(int i)
    {
        type |= GetMostSignificantNumber(i);
    }

    /// <summary>
    /// Checks If the number / bit of a 'SquareTypes' is included in type of said 'SquareType'
    /// <example><c>0b1010, 0b1001 -> 0b1000 = 'EndNodeSquare'</c></example>>
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
    /// bit e.g. an input of 68 will return 64 because 2**6 = 64, where 6
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
