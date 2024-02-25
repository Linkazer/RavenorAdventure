using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPL_DamageActionData
{
    public CPN_SpellCaster caster;
    public CPN_HealthHandler target;
    public List<Dice> dices;
    public bool didHit;
    public List<KeyValuePair<SPL_DamageType, int>> previousDamageResults;

    public SPL_DamageActionData(CPN_SpellCaster nCaster, CPN_HealthHandler nTarget, List<Dice> nDices, bool nDidHit, List<KeyValuePair<SPL_DamageType, int>> nPreviousDamageResults)
    {
        caster = nCaster;
        target = nTarget;
        dices = nDices;
        didHit = nDidHit;
        previousDamageResults = nPreviousDamageResults;
    }
}

[Serializable]
public abstract class SPL_DamageOrigin
{
    public abstract int GetDamageAmount(SPL_DamageActionData damageData);
}

public class SPL_DO_Dices : SPL_DamageOrigin
{
    public override int GetDamageAmount(SPL_DamageActionData damageData)
    {
        int toReturn = 0;

        foreach(Dice dice in damageData.dices)
        {
            if(dice.succeed)
            {
                toReturn++;
            }
        }

        return toReturn;
    }
}

public class SPL_DO_Direct : SPL_DamageOrigin
{
    [SerializeField] private int damageAmount;

    public override int GetDamageAmount(SPL_DamageActionData damageData)
    {
        return damageAmount;
    }
}