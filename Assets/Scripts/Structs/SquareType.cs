using System;
using UnityEngine;
using Unity;
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
            // ska fungera. Har ej testat!
            int twoLog = (int)(Math.Log(i, 0b1));
            type &= 0b1*twoLog;
        }
    }

    public override string ToString()
    {
        return $"Value: {type} Base: {base.ToString()}";
    }
}
