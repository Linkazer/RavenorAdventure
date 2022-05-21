using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected override void OnUseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback) //TO DO : Mettre à jour lors de la création du système de dégâts/pv
    {
        List<CPN_HealthHandler> hitableObjects = targetNode.GetNodeComponent<CPN_HealthHandler>();

        RVN_SS_DamageSpellScriptable usedScriptable = GetScriptable(spellToUse);

        foreach(CPN_HealthHandler hitedObject in hitableObjects)
        {
            Debug.Log(spellToUse.caster.gameObject.name + " deal " + usedScriptable.DamageDealt + " damages to " + hitedObject.gameObject.name + ".");

            hitedObject.TakeDamage(usedScriptable.DamageDealt, 0);//TO DO : Voir comment on gère les Heal et la réduction d'Armure
        }

        if (spellToUse.scriptable.SpellAnimation != null)
        {
            AnimationInstantiater.PlayAnimationAtPosition(spellToUse.scriptable.SpellAnimation, targetNode.worldPosition, callback);
        }
        else if(callback != null)
        {
            TimerManager.CreateGameTimer(0.5f, callback);
        }
    }
}
