using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Effect
{
    public EffectTrigger trigger;

    public void ApplyEffect(RVN_ComponentHandler effectTarget, CPN_SpellCaster caster)
    {
        switch (trigger)
        {
            case EffectTrigger.OnApply:
                UseEffect(effectTarget);
                break;
            case EffectTrigger.OnApplyWithoutCancel:
                UseEffect(effectTarget);
                break;
            case EffectTrigger.OnBeginTurn:
                if (effectTarget is CPN_Character)
                {
                    (effectTarget as CPN_Character).ActOnBeginTeamTurn += UseEffect;
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
                if (effectTarget is CPN_Character) //(effectTarget.GetComponentOfType<CPN_Character>(out CPN_Character endTurnCharacter))
                {
                    (effectTarget as CPN_Character).ActOnEndTeamTurn += UseEffect;
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
            case EffectTrigger.OnGetAttackedTowardSelf:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler getAttackedSelf))
                {
                    getAttackedSelf.actOnAttackReceivedTowardSelf += UseEffect;
                }
                break;
            case EffectTrigger.OnGetAttackedTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler getAttackedTarget))
                {
                    getAttackedTarget.actOnAttackReceivedTowardTarget += UseEffect;
                }
                break;
            case EffectTrigger.OnAttackTowardSelf:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster attackSelf))
                {
                    attackSelf.actOnUseSkillSelf += UseEffect;
                }
                break;
            case EffectTrigger.OnAttackTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster attackTarget))
                {
                    attackTarget.actOnUseSkillTarget += UseEffect;
                }
                break;
            case EffectTrigger.OnRemoveAllArmor:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler removeFullArmorHealth))
                {
                    removeFullArmorHealth.actOnRemoveAllArmor += UseEffect;
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
                if (effectTarget is CPN_Character) //(effectTarget.GetComponentOfType<CPN_Character>(out CPN_Character beginTurnCharacter))
                {
                    (effectTarget as CPN_Character).ActOnBeginTeamTurn -= UseEffect;
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
                if (effectTarget is CPN_Character) //(effectTarget.GetComponentOfType<CPN_Character>(out CPN_Character endTurnCharacter))
                {
                    (effectTarget as CPN_Character).ActOnEndTeamTurn -= UseEffect;
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
            case EffectTrigger.OnGetAttackedTowardSelf:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler getAttackedSelf))
                {
                    getAttackedSelf.actOnAttackReceivedTowardSelf -= UseEffect;
                }
                break;
            case EffectTrigger.OnGetAttackedTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler getAttackedTarget))
                {
                    getAttackedTarget.actOnAttackReceivedTowardTarget -= UseEffect;
                }
                break;
            case EffectTrigger.OnAttackTowardSelf:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster attackSelf))
                {
                    attackSelf.actOnUseSkillSelf -= UseEffect;
                }
                break;
            case EffectTrigger.OnAttackTowardTarget:
                if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster attackTarget))
                {
                    attackTarget.actOnUseSkillTarget -= UseEffect;
                }
                break;
            case EffectTrigger.OnRemoveAllArmor:
                if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler removeFullArmorHealth))
                {
                    removeFullArmorHealth.actOnRemoveAllArmor -= UseEffect;
                }
                break;
        }
    }

    public void TryUseEffect(RVN_ComponentHandler effectTarget, CPN_SpellCaster caster)
    {
        UseEffect(effectTarget);
    }

    protected abstract void UseEffect(RVN_ComponentHandler effectTarget);

    protected abstract void UndoEffect(RVN_ComponentHandler effectTarget);
}
