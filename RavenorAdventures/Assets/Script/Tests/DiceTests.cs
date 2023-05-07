using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTests : MonoBehaviour
{
    public enum DiceCalculType
    {
        DamageChance,
        HitsOnTarget,
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

    public bool usedDungeonSaga;
    public bool usedDnD;

    [Header("Behavior")]
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
        }
    }

    [Header("New system")]
    public int diceNb;
    public int diceResultNeeded;
    public int diceNumberNeeded;

    public int randomWanted = 6;

    [ContextMenu("Try New System")]
    public void NewSystemTest()
    {
        Results = new SortedDictionary<int, int>();

        int succeedDice = 0;

        int r = 0;

        for (int i = 0; i < iterations; i++)
        {
            succeedDice = 0;
            r = 0;

            for(int j = 0; j < diceNb; j++)
            {
                if(Random.Range(1, randomWanted + 1) >= diceResultNeeded)
                {
                    succeedDice++;
                }
            }

            if(succeedDice >= diceNumberNeeded)
            {
                r = 1;
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

    private void DamageChanceTest()
    {
        Results = new SortedDictionary<int, int>();


        for (int i = 0; i < iterations; i++)
        {
            int r = 0;

            for (int j = 0; j < attackers.Length; j++)
            {
                r += CalculateDamage(attackers[j]);
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

    private void HitsOnTargetTest()
    {
        Results = new SortedDictionary<int, int>();

        for (int i = 0; i < iterations; i++)
        {
            int currentArmor = armor;
            int currentHealth = health;

            int touches = 0;

            while (currentHealth > 0)
            {
                for (int j = 0; j < attackers.Length; j++)
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

        if (usedDungeonSaga)
        {
            totalDamage += TryDSDices();
        }
        else if(usedDnD)
        {
            totalDamage += TryDNDDices();
        }
        else
        {
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
                d.Roll();


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
        }

        if(totalDamage > 0 || baseDamageDone)
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
    public int dsOffReroll;
    public int dsDefReroll;

    public int TryDSDices()
    {
        List<int> attackResult = new List<int>();
        List<int> defenseResult = new List<int>();

        int leftOffReroll = dsOffReroll;
        int leftDefReroll = dsDefReroll;

        for (int i = 0; i < attackDice; i++)
        {
            attackResult.Add(Random.Range(0, 6) + 1);

            if(leftOffReroll > 0 && attackResult[i] <= defenseArmor)
            {
                leftOffReroll--;
                attackResult[i] = Random.Range(0, 6) + 1;
            }
        }

        for (int i = 0; i < defenseDice; i++)
        {
            defenseResult.Add(Random.Range(0, 6) + 1);

            if (leftDefReroll > 0 &&  defenseResult[i] <= defenseArmor)
            {
                leftDefReroll--;
                defenseResult[i] = Random.Range(0, 6) + 1;
            }
        }

        attackResult.Sort();
        defenseResult.Sort();

        attackResult.Reverse();
        defenseResult.Reverse();

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

    [Header("D&D dices")]
    public bool advantage;
    public bool disadvantage;
    public int dndAC;
    public int dndAttackBonus;

    private int TryDNDDices()
    {
        Dice baseDice = new Dice(6, dndAttackBonus);
        baseDice.Roll();

        Dice rerollDice = new Dice(6, dndAttackBonus);
        rerollDice.Roll();

        Dice diceToUse = baseDice;

        if(advantage && diceToUse.Result < rerollDice.Result)
        {
            diceToUse = rerollDice;
        }
        else if(disadvantage && diceToUse.Result > rerollDice.Result)
        {
            diceToUse = rerollDice;
        }

        return (diceToUse.Result > dndAC) ? 1 : 0; 
    }

}
