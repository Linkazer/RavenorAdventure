using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EFFTYPE_ActionEffect : EffectType
{
    public SpellScriptable spellToUse;

    public override void ApplyEffect(RVN_ComponentHandler effectTarget)
    {
        Debug.Log(trigger);
        switch (trigger)
        {
            case EffectTrigger.OnApply:

                break;
            case EffectTrigger.OnBeginTurn:
                if(effectTarget.GetComponentOfType<CPN_Character>(out CPN_Character beginTurnCharacter))
                {
                    beginTurnCharacter.ActOnBeginTurn += UseEffect;
                }
                break;
            case EffectTrigger.OnDealDamageTowardSelf:
                Debug.Log("Try add Action");
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster dealDamageSeldCaster))
                {
                    Debug.Log("Add Action");
                    dealDamageSeldCaster.actOnDealDamageSelf += UseEffect;
                }
                break;
            case EffectTrigger.OnDealDamageTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster dealDamageTargetCaster))
                {
                    dealDamageTargetCaster.actOnDealDamageTarget += UseEffect;
                }
                break;
            case EffectTrigger.OnDeath:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler deathHandler))
                {
                    deathHandler.actOnDeath += UseEffect;
                }
                break;
            case EffectTrigger.OnEnd:
                
                break;
            case EffectTrigger.OnEndTurn:
                //Check Character// TO DO : Ajouter les effets manquant
                break;
            case EffectTrigger.OnEnterNode:

                break;
            case EffectTrigger.OnExitNode:

                break;
            case EffectTrigger.OnTakeDamageTowardSelf:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler takeDamageSelf))
                {
                    takeDamageSelf.actOnTakeDamageSelf += UseEffect;
                }
                break;
            case EffectTrigger.OnTakeDamageTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler takeDamageTarget))
                {
                    takeDamageTarget.actOnTakeDamageTarget += UseEffect;
                }
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
                if (effectTarget.GetComponentOfType<CPN_Character>(out CPN_Character beginTurnCharacter))
                {
                    beginTurnCharacter.ActOnBeginTurn += UseEffect;
                }
                break;
            case EffectTrigger.OnDealDamageTowardSelf:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster dealDamageSeldCaster))
                {
                    dealDamageSeldCaster.actOnDealDamageSelf -= UseEffect;
                }
                break;
            case EffectTrigger.OnDealDamageTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster dealDamageTargetCaster))
                {
                    dealDamageTargetCaster.actOnDealDamageTarget += UseEffect;
                }
                break;
            case EffectTrigger.OnDeath:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler deathHandler))
                {
                    deathHandler.actOnDeath += UseEffect;
                }
                break;
            case EffectTrigger.OnEnd:

                break;
            case EffectTrigger.OnEndTurn:
                //Check Character// TO DO : Ajouter les effets manquant
                break;
            case EffectTrigger.OnEnterNode:

                break;
            case EffectTrigger.OnExitNode:

                break;
            case EffectTrigger.OnTakeDamageTowardSelf:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler takeDamageSelf))
                {
                    takeDamageSelf.actOnTakeDamageSelf += UseEffect;
                }
                break;
            case EffectTrigger.OnTakeDamageTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler takeDamageTarget))
                {
                    takeDamageTarget.actOnTakeDamageTarget += UseEffect;
                }
                break;
        }
    }

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        Debug.Log("Try use Action on " + effectTarget.gameObject);

        LaunchedSpellData launchedSpell = new LaunchedSpellData(spellToUse, null, Grid.GetNodeFromWorldPoint(effectTarget.transform.position));

        if (RVN_SpellManager.CanUseSpell(launchedSpell))
        {
            RVN_SpellManager.UseSpell(launchedSpell, null);
        }
    }
}

