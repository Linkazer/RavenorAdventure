using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Normal,
    Heal,
    IgnoreArmor
}

public class RVN_SB_DamageSpellBehavior : RVN_SpellBehavior<RVN_SS_DamageSpellScriptable>
{
    protected override void OnEndSpell(LaunchedSpellData spellToEnd)
    {
        Debug.Log("End spell : " + GetScriptable(spellToEnd));
    }

    protected override bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        Debug.Log("Is spell usable : " + GetScriptable(spellToCheck) + " at position : " + targetNode.worldPosition);
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

        foreach(CPN_HealthHandler hitedObject in hitableObjects)
        {
            switch(usedScriptable.Type)
            {
                case DamageType.Normal:
                    float damage = CalculateDamage(DiceManager.GetDices(usedScriptable.DiceUsed, 6, usedScriptable.Accuracy), usedScriptable.PossibleReroll, hitedObject);
                    if(damage > 0)
                    {
                        damage += usedScriptable.BaseDamage;

                        hitedObject.TakeDamage(damage, usedScriptable.ArmorPierced);

                        Debug.Log(hitedObject.gameObject);
                        hitedObject.actOnTakeDamageSelf?.Invoke(hitedObject.Handler); // CODE REVIEW : Voir pour mettre ça autre part
                        hitedObject.actOnTakeDamageTarget?.Invoke(spellToUse.caster.Handler);

                        if(spellToUse.caster != null)
                        {
                            spellToUse.caster.DealDamage(hitedObject);
                        }


                        if (hitedObject.Handler.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
                        {
                            foreach (EffectScriptable eff in usedScriptable.Effects())
                            {
                                ApplyEffects(effectHandler, eff.GetEffect);
                            }
                        }

                    }
                    else
                    {
                        toReturn = false;
                    }
                    break;
                case DamageType.Heal:
                    hitedObject.TakeHeal(usedScriptable.DiceUsed); //TO DO : Voir si on ajoute la régénération d'Armure ici
                    break;
                case DamageType.IgnoreArmor:
                    //TO DO : Voir pour les dégâts Brut
                    break;
            }
        }

        if (usedScriptable.SpellAnimation != null)
        {
            AnimationInstantiater.PlayAnimationAtPosition(usedScriptable.SpellAnimation, targetNode.worldPosition, callback);
        }
        else if(callback != null)
        {
            TimerManager.CreateGameTimer(0.5f, callback);
        }

        return toReturn;
    }

    private float CalculateDamage(List<Dice> diceDamage, int possibleRerolls, CPN_HealthHandler target)
    {
        float totalDamage = 0;
        int currentRelance = 0;

        for(int i = 0; i < diceDamage.Count; i++)
        {
            if(diceDamage[i].Result > target.Defense)
            {
                totalDamage++;
            }
            else if(currentRelance < possibleRerolls)
            {
                currentRelance++;
                diceDamage[i].Roll();
                i--;
            }
        }

        return totalDamage;
    }
}
