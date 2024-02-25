using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_EffectHandler : RVN_Component<CPN_Data_EffectHandler>
{
    [SerializeField] private Transform effectDisplayHolder;

    [SerializeField] private List<EffectScriptable> startingEffects = new List<EffectScriptable>();
    
    private List<AppliedEffect> currentAppliedEffects = new List<AppliedEffect>();

    [SerializeField] private UnityEvent<AppliedEffect> OnApplyEffect;
    [SerializeField] private UnityEvent<AppliedEffect> OnRemoveEffect;

    public List<AppliedEffect> Effects => currentAppliedEffects;

    protected override CPN_Data_EffectHandler GetDataFromHandler()
    {
        if (handler is CPN_Character)
        {
            return (handler as CPN_Character).Scriptable;
        }

        return null;
    }

    protected override void SetData(CPN_Data_EffectHandler toSet)
    {
        startingEffects = new List<EffectScriptable>(toSet.Effects());
    }

    public override void Activate()
    {
        foreach (EffectScriptable eff in startingEffects)
        {
            ApplyEffect(eff, null);
        }
    }

    public override void Disactivate()
    {

    }

    public override void OnStartHandlerGroupRound()
    {
        base.OnStartHandlerGroupRound();

        UpdateEffectDuration();
    }

    public override void OnEndHandlerGroupRound()
    {
        base.OnEndHandlerGroupRound();

        UpdateEffectDuration();
    }

    public void UpdateEffectDuration()
    {
        AppliedEffect effectToCheck = null;

        for (int i = 0; i < currentAppliedEffects.Count; i++)
        {
            effectToCheck = currentAppliedEffects[i];
            effectToCheck.UpdateDuration();
            if(effectToCheck.Duration <= 0)
            {
                i--;
            }
        }
    }

    public void ApplyEffect(EffectScriptable toApply, CPN_SpellCaster caster)
    {
        AppliedEffect potentialAppliedEffect = TryGetAppliedEffect(toApply);

        if (potentialAppliedEffect != null)
        {
            potentialAppliedEffect.ResetEffect(Handler, toApply.Duration);
        }
        else
        {
            GameObject effectDisplay = null;

            if (toApply.EffectDisplay != null)
            {
                effectDisplay = Instantiate(toApply.EffectDisplay, effectDisplayHolder);
            }

            potentialAppliedEffect = new AppliedEffect(toApply, effectDisplay, toApply.Duration, ()=>RemoveEffect(potentialAppliedEffect));

            potentialAppliedEffect.ApplyEffect(Handler, caster);

            currentAppliedEffects.Add(potentialAppliedEffect);

            if (toApply.Duration == 0)
            {
                RemoveEffect(toApply);
            }
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

    public bool HasEffect(EffectScriptable toCheck)
    {
        foreach (AppliedEffect eff in currentAppliedEffects)
        {
            if (eff.GetEffect.name == toCheck.name)
            {
                return true;
            }
        }
        return false;
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
