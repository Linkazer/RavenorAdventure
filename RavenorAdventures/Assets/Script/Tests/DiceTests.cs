using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DiceTests : MonoBehaviour
{
    public enum DiceCalculType
    {
        DamageChance,
        HitsOnTarget,
        DiceResult,
    }

    [System.Serializable]
    public class AttackTest
    {
        public int AttackDice;
        public int OffensiveRerolls;
        public int Accuracy;
        public int Power;
    }

    [SerializeField] private string testName;
    [SerializeField] private DiceCalculType calculType;

    [Header("(Old) Behavior")]
    [SerializeField] private bool doesNeedFirstHit;
    [SerializeField] private bool baseDamageDone;

    [Header("Attack")]
    [SerializeField] private AttackTest[] attackers;

    [Header("Defense")]
    [SerializeField] private int Defense;
    [SerializeField] private int DefensiveRerolls;

    [Header("Target")]
    [SerializeField] private int health;
    [SerializeField] private int armor;
    [SerializeField] private int damageReduction;

    [Header("Tests")]
    [SerializeField] private int iterations;

    private SortedDictionary<int, int> Results;

    [ContextMenu("Try Dices")]
    public void Test()
    {
        switch(calculType)
        {
            case DiceCalculType.DamageChance:
                DamageChanceTest();
                break;
            case DiceCalculType.HitsOnTarget:
                HitsOnTargetTest();
                break;
            case DiceCalculType.DiceResult:
                DiceResultChances();
                break;
        }
    }

    private void DamageChanceTest()
    {
        Results = new SortedDictionary<int, int>();


        for (int i = 0; i < iterations; i++)
        {
            ResetDiceHistory();
            int r = 0;

            for (int j = 0; j < attackers.Length; j++)
            {
                if (!doesNeedFirstHit || j == 0 || r > 0)
                {
                    r += CalculateDamage(attackers[j]);
                }
            }

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

        foreach (KeyValuePair<int, int> rslt in Results)
        {
            toDisplay += $"\n {rslt.Key} : {(((float)rslt.Value / (float)iterations) * 100f).ToString("F2")}%";
        }

        Debug.Log(toDisplay);
    }

    private void DiceResultChances()
    {
        Results = new SortedDictionary<int, int>();

        for (int i = 0; i < iterations; i++)
        {
            ResetDiceHistory();

            Dice d = new Dice(6, 0);
            d.Roll(this);

            int diceResult = Mathf.RoundToInt(d.Result);

            if (Results.ContainsKey(diceResult))
            {
                Results[diceResult]++;
            }
            else
            {
                Results.Add(diceResult, 1);
            }
        }

        string toDisplay = ($"{testName} : ");

        float total = 0;
        float coef = 0;

        foreach (KeyValuePair<int, int> rslt in Results)
        {
            toDisplay += $"\n {rslt.Key} : {(((float)rslt.Value / (float)iterations) * 100f).ToString("F2")}%";

            total += rslt.Key * rslt.Value;
            coef += rslt.Value;
        }

        toDisplay += $"\n Average result : {total / coef}";

        Debug.Log(toDisplay);
    }

    private void HitsOnTargetTest()
    {
        Results = new SortedDictionary<int, int>();

        for (int i = 0; i < iterations; i++)
        {
            int currentArmor = armor;
            int currentHealth = health;

            int touches = 0;
            int lastDamage = 0;

            while (currentHealth > 0)
            {
                for (int j = 0; j < attackers.Length; j++)
                {
                    if (!doesNeedFirstHit || j == 0 || lastDamage > 0)
                    {
                        int damage = CalculateDamage(attackers[j]);

                        if (damage > 0)
                        {

                            if (damage >= damageReduction)
                            {
                                damage -= damageReduction;
                            }

                            if (damage >= currentArmor)
                            {
                                currentHealth -= damage - currentArmor;
                            }

                            if (currentArmor > 0)
                            {
                                currentArmor--;
                            }
                        }

                        lastDamage = damage;
                    }
                }

                touches++;
            }

            if (Results.ContainsKey(touches))
            {
                Results[touches]++;
            }
            else
            {
                Results.Add(touches, 1);
            }
        }

        string toDisplay = ($"{testName} : ");

        float moyenne = 0;

        foreach (KeyValuePair<int, int> rslt in Results)
        {
            if ((((float)rslt.Value / (float)iterations) * 100f) > 0.5f)
            {
                toDisplay += $"\n {rslt.Key} : {(((float)rslt.Value / (float)iterations) * 100f).ToString("F2")}%";
            }

            moyenne += rslt.Key * (float)rslt.Value;
        }

        Debug.Log("Moyenne : " + (moyenne / (float)iterations) + "\n" +  toDisplay);
    }

    private int CalculateDamage(AttackTest attacker)
    {
        int totalDamage = 0;

        int currentOffensiveRerolls = 0;
        int currentDefensiveRerolls = 0;

        for (int i = 0; i < attacker.AttackDice; i++)
        {
            int bonus = 0;

            if (i < Mathf.Abs(attacker.Accuracy))
            {
                if (attacker.Accuracy > 0)
                {
                    bonus = 1;
                }
                else
                {
                    bonus = -1;
                }
            }

            Dice d = new Dice(6, attacker.Accuracy);
            d.Roll(this);

            totalDamage += CheckDiceHit(d, Defense, currentOffensiveRerolls < attacker.OffensiveRerolls, currentDefensiveRerolls < DefensiveRerolls, out bool usedOff, out bool usedDef);

            if (usedDef)
            {
                currentDefensiveRerolls++;
            }

            if (usedOff)
            {
                currentOffensiveRerolls++;
            }
        }

        if (totalDamage > 0 || baseDamageDone)
        {
            totalDamage += attacker.Power;
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
                dice.Roll(this);

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
            dice.Roll(this);

            return CheckDiceHit(dice, defense, false, hasDefensiveReroll, out bool off, out usedDefensiveReroll);
        }

        return toReturn;
    }

    [ContextMenu("Reset History")]
    public void ResetDiceHistory()
    {
        Dice.dicesHistory.Clear();
    }
}
