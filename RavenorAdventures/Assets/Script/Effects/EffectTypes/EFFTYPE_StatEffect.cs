using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EFFTYPE_StatEffect : EffectType
{
    public EffectStatEnum stat;
    public float valueToChange;

    public override void ApplyEffect(RVN_ComponentHandler effectTarget)
    {
        switch (trigger)
        {
            case EffectTrigger.OnApply:

                break;
            case EffectTrigger.OnBeginTurn:
                //Check Character
                break;
            case EffectTrigger.OnDealDamageTowardSelf:
                //Check Spellcaster
                break;
            case EffectTrigger.OnDealDamageTowardTarget:
                //Check Spellcaster
                break;
            case EffectTrigger.OnDeath:
                //Check HealthHandler
                break;
            case EffectTrigger.OnEnd:

                break;
            case EffectTrigger.OnEndTurn:
                //Check Character
                break;
            case EffectTrigger.OnEnterNode:

                break;
            case EffectTrigger.OnExitNode:

                break;
            case EffectTrigger.OnTakeDamageTowardSelf:
                //Check HealthHandler
                break;
            case EffectTrigger.OnTakeDamageTowardTarget:
                //Check HealthHandler
                break;
        }
        
    }

    public override void RemoveEffect(RVN_ComponentHandler effectTarget)
    {
        switch (trigger)
        {
            case EffectTrigger.OnApply:

                break;
            case EffectTrigger.OnBeginTurn:
                //Check Character
                break;
            case EffectTrigger.OnDealDamageTowardSelf:
                //Check Spellcaster
                break;
            case EffectTrigger.OnDealDamageTowardTarget:
                //Check Spellcaster
                break;
            case EffectTrigger.OnDeath:
                //Check HealthHandler
                break;
            case EffectTrigger.OnEnd:

                break;
            case EffectTrigger.OnEndTurn:
                //Check Character
                break;
            case EffectTrigger.OnEnterNode:

                break;
            case EffectTrigger.OnExitNode:

                break;
            case EffectTrigger.OnTakeDamageTowardSelf:
                //Check HealthHandler
                break;
            case EffectTrigger.OnTakeDamageTowardTarget:
                //Check HealthHandler
                break;
        }
    }

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        switch (stat)
        {
            case EffectStatEnum.BaseDamage:

                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster1))
                {
                    caster1.AddPower((int)valueToChange);
                }
                break;
            case EffectStatEnum.Accuracy:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster2))
                {
                    caster2.AddPower((int)valueToChange);
                }
                break;
            case EffectStatEnum.RerollDice:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster3))
                {
                    caster3.AddPower((int)valueToChange);
                }
                break;
            case EffectStatEnum.ActionByTurn:
                break;
            case EffectStatEnum.Armor:
                break;
            case EffectStatEnum.Defense:
                break;
            case EffectStatEnum.Movement:
                break;
        }
    }
}
