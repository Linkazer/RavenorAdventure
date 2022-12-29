using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager
{
    public static List<Dice> RollDices(int faceNumber, int diceNumber)
    {
        List<Dice> toReturn = new List<Dice>();

        for(int i = 0; i < diceNumber; i++)
        {
            toReturn.Add(new Dice(faceNumber));
            toReturn[i].Roll();
        }

        return toReturn;
    }

    public static List<Dice> GetDices(int diceNumber, int faceNumber, int faceBonus = 0)
    {
        List<Dice> toReturn = new List<Dice>();

        for (int i = 0; i < diceNumber; i++)
        {
            toReturn.Add(new Dice(faceNumber, faceBonus));
            toReturn[i].Roll();
        }

        return toReturn;
    }
}
