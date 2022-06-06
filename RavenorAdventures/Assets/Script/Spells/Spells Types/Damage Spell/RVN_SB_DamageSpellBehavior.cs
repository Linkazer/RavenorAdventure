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

        foreach(CPN_HealthHandler hitedObject in hitableObjects)
        {
            switch(usedScriptable.Type)
            {
                case DamageType.Normal:
                    List<Dice> damageDices = DiceManager.GetDices(usedScriptable.DiceUsed, 6, usedScriptable.Accuracy);

                    float damage = CalculateDamage(damageDices, usedScriptable.PossibleReroll, hitedObject);
                    if(damage > 0)
                    {
                        damage += usedScriptable.BaseDamage;

                        hitedObject.TakeDamage(spellToUse.caster, damageDices, damage, usedScriptable.ArmorPierced);

                        if(spellToUse.caster != null)
                        {
                            spellToUse.caster.DealDamage(hitedObject);
                        }
                    }
                    else
                    {
                        if(usedScriptable.ArmorPierced > 0)
                        {
                            hitedObject.TakeDamage(spellToUse.caster, 0, usedScriptable.ArmorPierced);
                        }
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
                diceDamage[i].succeed = true;
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
