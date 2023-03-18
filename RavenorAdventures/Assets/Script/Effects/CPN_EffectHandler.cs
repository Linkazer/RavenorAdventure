using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_EffectHandler : RVN_Component<CPN_Data_EffectHandler>
{
    [SerializeField] private Transform effectDisplayHolder;

    [SerializeField] private List<AppliedEffect> currentAppliedEffects;

    [SerializeField] private UnityEvent<AppliedEffect> OnApplyEffect;
    [SerializeField] private UnityEvent<AppliedEffect> OnRemoveEffect;

    public List<AppliedEffect> Effects => currentAppliedEffects;

    public override void OnEnterBattle()
    {
        
    }

    public override void OnExitBattle()
    {
        
    }

    public override void SetData(CPN_Data_EffectHandler toSet)
    {
        foreach(EffectScriptable eff in toSet.Effects())
        {
            ApplyEffect(eff);
        }
    }

    public void UpdateEffectDuration()
    {
        List<AppliedEffect> toRemove = new List<AppliedEffect>();

        foreach(AppliedEffect eff in currentAppliedEffects)
        {
            eff.UpdateDuration();
            if(eff.Duration == 0)
            {
                toRemove.Add(eff);
            }
        }

        foreach(AppliedEffect eff in toRemove)
        {
            RemoveEffect(eff);
        }
    }

    public void ApplyEffect(EffectScriptable toApply)
    {
        AppliedEffect potentialAppliedEffect = TryGetAppliedEffect(toApply);

        if (potentialAppliedEffect != null)
        {
            potentialAppliedEffect.ResetEffect(toApply.Duration);
        }
        else
        {
            GameObject effectDisplay = null;

            if (toApply.EffectDisplay != null)
            {
                effectDisplay = Instantiate(toApply.EffectDisplay, effectDisplayHolder);
            }

            potentialAppliedEffect = new AppliedEffect(toApply, effectDisplay, toApply.Duration);

            potentialAppliedEffect.ApplyEffect(Handler);

            currentAppliedEffects.Add(potentialAppliedEffect);
        }

        if (potentialAppliedEffect != null)
        {
            OnApplyEffect?.Invoke(potentialAppliedEffect);
        }
    }

    public void RemoveEffect(EffectScriptable toRemove)
    {
        if (TryGetAppliedEffect(toRemove) != null)
        {
            RemoveEffect(TryGetAppliedEffect(toRemove));
        }
    }

    private void RemoveEffect(AppliedEffect toRemove)
    {
        toRemove.RemoveEffect(handler);

        OnRemoveEffect?.Invoke(toRemove);

        currentAppliedEffects.Remove(toRemove);
    }

    private AppliedEffect TryGetAppliedEffect(EffectScriptable toCheck)
    {
        foreach(AppliedEffect eff in currentAppliedEffects)
        {
            if(eff.GetEffect.name == toCheck.name)
            {
                return eff;
            }
        }
        return null;
    }
}
