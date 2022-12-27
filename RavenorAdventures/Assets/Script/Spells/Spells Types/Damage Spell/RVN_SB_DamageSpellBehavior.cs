using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Normal,
    Heal,
    IgnoreArmor,
    RegenArmor
}

public class RVN_SB_DamageSpellBehavior : RVN_SpellBehavior<RVN_SS_DamageSpellScriptable>
{
    protected override void OnEndSpell(LaunchedSpellData spellToEnd)
    {
        
    }

    protected override bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        return true;
    }

    /// <summary>
    /// Apply the effect and damage of the spell and launch the spell animation.
    /// </summary>
    /// <param name="spellToUse">The spell to use.</param>
    /// <param name="targetNode">The targeted Node.</param>
    /// <param name="callback">The callback to call at the end of the spell.</param>
    protected override bool OnUseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback) //TO DO : Mettre à jour lors de la création du système de dégâts/pv
    {
        bool toReturn = true;

        List<CPN_HealthHandler> hitableObjects = targetNode.GetNodeComponent<CPN_HealthHandler>();

        RVN_SS_DamageSpellScriptable usedScriptable = GetScriptable(spellToUse);

        foreach (CPN_HealthHandler hitedObject in hitableObjects)
        {
            if (CanUseOnTarget(spellToUse.scriptable.HitableTargets, spellToUse.caster, hitedObject))
            {
                switch (usedScriptable.Type)
                {
                    case DamageType.Heal:
                        hitedObject.TakeHeal(usedScriptable.BaseDamage + usedScriptable.DiceUsed);
                        break;
                    case DamageType.RegenArmor:
                        hitedObject.AddArmor(usedScriptable.BaseDamage + usedScriptable.DiceUsed);
                        break;
                    default:
                        List<Dice> damageDices = DiceManager.GetDices(usedScriptable.DiceUsed, 6, usedScriptable.Accuracy);

                        float damage = CalculateDamage(damageDices, usedScriptable, hitedObject, out bool doesHit);

                        if (!doesHit)
                        {
                            toReturn = false;
                        }

                        hitedObject.TakeDamage(spellToUse.caster, damageDices, damage);

                        if (damage > 0)
                        {
                            if (spellToUse.caster != null)
                            {
                                if (usedScriptable.Lifesteal > 0 && spellToUse.caster.Handler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler casterHealth))
                                {
                                    casterHealth.TakeHeal(damage * usedScriptable.Lifesteal);
                                }

                                spellToUse.caster.DealDamage(hitedObject);
                            }
                        }
                        else
                        {
                            if (usedScriptable.ArmorPierced > 0)
                            {
                                hitedObject.TakeDamage(spellToUse.caster, 0);
                            }
                        }

                        hitedObject.actOnAttackReceivedTowardTarget?.Invoke(spellToUse.caster.Handler);
                        hitedObject.actOnAttackReceivedTowardSelf?.Invoke(hitedObject.Handler);

                        break;
                }
            }
        }

        if(toReturn)
        {
            foreach (CPN_HealthHandler hitedObject in hitableObjects)
            {
                if (CanUseOnTarget(spellToUse.scriptable.HitableTargets, spellToUse.caster, hitedObject))
                {
                    if (hitedObject.Handler.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
                    {
                        foreach (EffectScriptable eff in usedScriptable.HitEffectsOnTarget)
                        {
                            ApplyEffects(effectHandler, eff);
                        }
                    }
                }
            }

            if(spellToUse.caster != null)
            {
                if (spellToUse.caster.Handler.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
                {
                    foreach (EffectScriptable eff in usedScriptable.HitEffectsOnCaster)
                    {
                        ApplyEffects(effectHandler, eff);
                    }
                }
            }
        }

        return toReturn;
    }

    private float CalculateDamage(List<Dice> diceDamage, RVN_SS_DamageSpellScriptable spellUsed, CPN_HealthHandler target, out bool didHit)
    {
        didHit = false;

        float totalDamage = 0;
        int currentOffensiveRerolls = 0;
        int currentDefensiveRerolls = 0;

        for (int i = 0; i < diceDamage.Count; i++)
        {
            totalDamage += CheckDiceHit(diceDamage[i], target.Defense, currentOffensiveRerolls < spellUsed.OffensiveRerolls, currentDefensiveRerolls < target.DefensiveRerolls, out bool usedOffensiveReroll, out bool usedDefensiveReroll);

            if(usedDefensiveReroll)
            {
                currentDefensiveRerolls++;
                diceDamage[i].rerolled += 1;
            }

            if(usedOffensiveReroll)
            {
                currentOffensiveRerolls++;
                diceDamage[i].rerolled += 2;
            }

           
        }

        if(totalDamage > 0)
        {
            didHit = true;
        }

        if (diceDamage.Count <= 0 || totalDamage > 0)
        {
            target.RemoveArmor(spellUsed.ArmorPierced);
        }

        if (totalDamage > 0)
        {
            totalDamage += spellUsed.BaseDamage;

            if (spellUsed.Type != DamageType.IgnoreArmor)
            {
                if (totalDamage > target.CurrentArmor)
                {
                    totalDamage -= target.CurrentArmor;
                }
                else
                {
                    totalDamage = 0;
                }
            }

            target.RemoveArmor(1);
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
                dice.succeed = true;
                toReturn = 1;
            }
        }
        else if (hasOffensiveReroll)
        {
            //dice.rerolled += 2;

            usedOffensiveReroll = true;
            dice.Roll();
            return CheckDiceHit(dice, defense, false, hasDefensiveReroll, out bool off, out usedDefensiveReroll);
        }

        return toReturn;
    }
}
