using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    private int faceNumber;
    private int faceBonus;

    private float result;

    public float Result => result;

    public Dice()
    {

    }

    public Dice(int faces, int bonus = 0)
    {
        faceNumber = faces;
        faceBonus = bonus;
    }

    public float Roll()
    {
        result = Random.Range(1, faceNumber+1) + faceBonus;

        return result;
    }
}
