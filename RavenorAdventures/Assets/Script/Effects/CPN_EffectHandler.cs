using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_EffectHandler : RVN_Component<CPN_Data_EffectHandler>
{
    private List<Effect> currentAppliedEffects;
    [SerializeField] private RVN_ComponentHandler handler;

    [SerializeField] private UnityEvent<EffectScriptable> OnApplyEffect;
    [SerializeField] private UnityEvent<EffectScriptable> OnRemoveEffect;

    public override void SetData(CPN_Data_EffectHandler toSet)
    {
        foreach(EffectScriptable eff in toSet.Effects())
        {

        }
    }

    public void ApplyEffect(EffectScriptable toApply)
    {

    }

    public void RemoveEffect(EffectScriptable toRemove)
    {
        TryExecuteEffect(toRemove.GetEffect, EffectTrigger.OnEnd, handler);


    }

    public void TryExecuteAllEffects(EffectTrigger trigger, RVN_ComponentHandler effectTarget)
    {
        foreach(Effect eff in currentAppliedEffects)
        {
            TryExecuteEffect(eff, trigger, effectTarget);
        }
    }

    public void TryExecuteEffect(Effect toExecute, EffectTrigger trigger, RVN_ComponentHandler effectTarget)
    {
        foreach(Effect.StatEffect eff in toExecute.StatEffects)
        {
            TryExecuteStatEffect(eff, trigger, effectTarget);
        }
    }

    private void TryExecuteStatEffect(Effect.StatEffect toExecute, EffectTrigger trigger, RVN_ComponentHandler effectTarget)
    {
        if (toExecute.trigger == trigger)
        {
            //CODE REVIEW : Voir comment faire pour les GetComponentOfType. Devoir avoir une variable différente à chaque fois c'est chiant
            switch (toExecute.stat)
            {
                case EffectStatEnum.BaseDamage:
                    
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster1))
                    {
                        caster1.AddPower((int)toExecute.valueToChange);
                    }
                    break;
                case EffectStatEnum.Accuracy:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster2))
                    {
                        caster2.AddPower((int)toExecute.valueToChange);
                    }
                    break;
                case EffectStatEnum.RerollDice:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster3))
                    {
                        caster3.AddPower((int)toExecute.valueToChange);
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
}
