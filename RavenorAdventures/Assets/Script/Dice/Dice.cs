using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    private const float coefOnUse = 0.25f;

    public static Dictionary<MonoBehaviour, List<int>> dicesHistory = new Dictionary<MonoBehaviour, List<int>>();

    private int faceNumber;
    private int faceBonus;

    private float result;

    public bool succeed = false;
    public int rerolled = 0;// 1 : Reroll Attaque, 2 : Reroll Defense, 3 : Reroll des deux

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
        result = UnityEngine.Random.Range(1, faceNumber+1) + faceBonus;

        return result;
    }

    public float Roll(MonoBehaviour asker)
    {
        result = 0;

        List<int> history = new List<int>();
        if (asker != null)
        {
            if (dicesHistory.ContainsKey(asker))
            {
                history = dicesHistory[asker];
            }
            else
            {
                dicesHistory[asker] = new List<int>();
            }
        }

        List<float> weights = new List<float>() { 1f, 1f, 1f, 1f, 1f, 1f };

        foreach(int hist in history)
        {
            if(hist > 0)
            {
                weights[hist - 1] *= coefOnUse;
            }
        }

        float maxWeight = 0;

        foreach(float f in weights)
        {
            maxWeight += f;
        }

        float rng = UnityEngine.Random.Range(0, maxWeight);

        float currentCount = 0;
        int index = 0;

        foreach (float f in weights)
        {
            currentCount += f;
            index++;
            if (rng < currentCount)
            {
                result = index;
                break;
            }
        }

        try
        {
            if (asker != null)
            {
                dicesHistory[asker].Add((int)result);
            }
        }
        catch(Exception e)
        {
            Debug.Log((int)result);
        }

        if (asker != null)
        {
            if (dicesHistory[asker].Count > 10)
            {
                dicesHistory[asker].RemoveAt(0);
            }
        }

        return result;
    }
}
