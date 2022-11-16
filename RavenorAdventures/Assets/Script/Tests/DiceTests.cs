using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTests : MonoBehaviour
{
    [SerializeField] private string testName;

    [Header("Attack")]
    [SerializeField] private int AttackDice;
    [SerializeField] private int OffensiveRerolls;
    [SerializeField] private int Accuracy;
    [SerializeField] private int Power;

    [Header("Defense")]
    [SerializeField] private int Defense;
    [SerializeField] private int DefensiveRerolls;

    [Header("Tests")]
    [SerializeField] private int iterations;

    private Dictionary<int, int> Results;

    [ContextMenu("Try Dices")]
    public void Test()
    {
        Results = new Dictionary<int, int>();

        for(int i = 0; i < AttackDice + 1; i++)
        {
            Results.Add(i, 0);
        }

        for (int i = 0; i < iterations; i++)
        {
            int r = CalculateDamage();

            if (Results.ContainsKey(r))
            {
                Results[r]++;
            }
            else
            {
                Results.Add(r, 1);
            }
        }

        string toDisplay = ($"{testName} : ");

        foreach(KeyValuePair<int,int> rslt in Results)
        {
            toDisplay += $"\n {rslt.Key} : {Mathf.RoundToInt(((float)rslt.Value / (float)iterations) * 100f)}%";
        }

        Debug.Log(toDisplay);
    }

    private int CalculateDamage()
    {
        int totalDamage = 0;
        int currentOffensiveRerolls = 0;
        int currentDefensiveRerolls = 0;

        for (int i = 0; i < AttackDice; i++)
        {
            int bonus = 0;

            if (i < Mathf.Abs(Accuracy))
            {
                if (Accuracy > 0)
                {
                    bonus = 1;
                }
                else
                {
                    bonus = -1;
                }
            }

            Dice d = new Dice(6, Accuracy);
            d.Roll();

            totalDamage += CheckDiceHit(d, Defense, currentOffensiveRerolls < OffensiveRerolls, currentDefensiveRerolls < DefensiveRerolls, out bool usedOff, out bool usedDef);

            if(usedDef)
            {
                currentDefensiveRerolls++;
            }

            if (usedOff)
            {
                currentOffensiveRerolls++;
            }
        }

        if(totalDamage > 0)
        {
            totalDamage += Power;
        }

        return totalDamage;
    }

    private int CheckDiceHit(Dice dice, int defense, bool hasOffensiveReroll, bool hasDefensiveReroll, out bool usedOffensiveReroll, out bool usedDefensiveReroll)
    {
        int toReturn = 0;

        usedDefensiveReroll = false;
        usedOffensiveReroll = false;

        if (dice.Result > defense)
        {
            if (hasDefensiveReroll)
            {
                usedDefensiveReroll = true;
                dice.Roll();

                return CheckDiceHit(dice, defense, hasOffensiveReroll, false, out usedOffensiveReroll, out bool def);
            }
            else
            {
                toReturn = 1;
            }
        }
        else if (hasOffensiveReroll)
        {
            usedOffensiveReroll = true;
            dice.Roll();

            return CheckDiceHit(dice, defense, false, hasDefensiveReroll, out bool off, out usedDefensiveReroll);
        }

        return toReturn;
    }

    [Header("DungeonSaga dices")]
    public int attackDice;
    public int defenseDice;
    public int defenseArmor;

    public int TryDSDices()
    {
        List<int> attackResult = new List<int>();
        List<int> defenseResult = new List<int>();

        for(int i = 0; i < attackDice; i++)
        {
            attackResult.Add(Random.Range(0, 6) + 1);
        }

        for (int i = 0; i < defenseDice; i++)
        {
            defenseResult.Add(Random.Range(0, 6) + 1);
        }

        attackResult.Sort();
        defenseResult.Sort();

        int hitNb = 0;

        for(int i = 0; i < attackResult.Count; i++)
        {
            if(attackResult[i] > defenseArmor && (defenseResult.Count <= i || attackResult[i] > defenseResult[i]))
            {
                hitNb++;
            }
        }

        return hitNb;
    }
}
