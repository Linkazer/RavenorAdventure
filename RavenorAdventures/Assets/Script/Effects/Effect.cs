using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Effect : ScriptableObject
{
    [SerializeField] private string effectName;
    [SerializeField] private Sprite icon;

    public EffectTrigger trigger;

    public string Name => effectName;
    public Sprite Icon => icon;

    public void ApplyEffect(RVN_ComponentHandler effectTarget)
    {
        switch (trigger)
        {
            case EffectTrigger.OnApply:
                UseEffect(effectTarget);
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
                //TO DO : Mettre dans le EffectHandler ??
                break;
            case EffectTrigger.OnEndTurn:
                if (effectTarget.GetComponentOfType<CPN_Character>(out CPN_Character endTurnCharacter))
                {
                    endTurnCharacter.ActOnEndTeamTurn += UseEffect;
                }
                break;
            case EffectTrigger.OnEnterNode:
                //TO DO : Faire les effets sur le terrain
                break;
            case EffectTrigger.OnExitNode:
                //TO DO : Faire les effets sur le terrain
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

    public void RemoveEffect(RVN_ComponentHandler effectTarget)
    {
        switch (trigger)
        {
            case EffectTrigger.OnApply:
                UndoEffect(effectTarget);
                break;
            case EffectTrigger.OnBeginTurn:
                if (effectTarget.GetComponentOfType<CPN_Character>(out CPN_Character beginTurnCharacter))
                {
                    beginTurnCharacter.ActOnBeginTurn -= UseEffect;
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
                    dealDamageTargetCaster.actOnDealDamageTarget -= UseEffect;
                }
                break;
            case EffectTrigger.OnDeath:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler deathHandler))
                {
                    deathHandler.actOnDeath -= UseEffect;
                }
                break;
            case EffectTrigger.OnEnd:
                //TO DO : Mettre dans le EffectHandler ??
                break;
            case EffectTrigger.OnEndTurn:
                if (effectTarget.GetComponentOfType<CPN_Character>(out CPN_Character endTurnCharacter))
                {
                    endTurnCharacter.ActOnEndTeamTurn -= UseEffect;
                }
                break;
            case EffectTrigger.OnEnterNode:
                //TO DO : Faire les effets sur le terrain
                break;
            case EffectTrigger.OnExitNode:
                //TO DO : Faire les effets sur le terrain
                break;
            case EffectTrigger.OnTakeDamageTowardSelf:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler takeDamageSelf))
                {
                    takeDamageSelf.actOnTakeDamageSelf -= UseEffect;
                }
                break;
            case EffectTrigger.OnTakeDamageTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler takeDamageTarget))
                {
                    takeDamageTarget.actOnTakeDamageTarget -= UseEffect;
                }
                break;
        }
    }

    protected abstract void UseEffect(RVN_ComponentHandler effectTarget);

    protected abstract void UndoEffect(RVN_ComponentHandler effectTarget);
}
