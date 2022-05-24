using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_EffectHandler : RVN_Component<CPN_Data_EffectHandler>
{
    [SerializeField] private List<AppliedEffect> currentAppliedEffects;

    [SerializeField] private UnityEvent<EffectScriptable> OnApplyEffect;
    [SerializeField] private UnityEvent<EffectScriptable> OnRemoveEffect;

    public override void SetData(CPN_Data_EffectHandler toSet)
    {
        foreach(EffectScriptable eff in toSet.Effects())
        {
            ApplyEffect(eff.GetEffect);
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

    public void ApplyEffect(Effect toApply)
    {
        if(HasEffect(toApply) != null)
        {
            HasEffect(toApply).ResetEffect(3);
        }
        else
        {
            currentAppliedEffects.Add(new AppliedEffect(toApply, 3));
            toApply.ApplyEffect(Handler);
        }
    }

    public void RemoveEffect(Effect toRemove)
    {
        if (HasEffect(toRemove) != null)
        {
            RemoveEffect(HasEffect(toRemove));
        }
    }

    private void RemoveEffect(AppliedEffect toRemove)
    {
        currentAppliedEffects.Remove(toRemove);
    }

    private AppliedEffect HasEffect(Effect toCheck)
    {
        foreach(AppliedEffect eff in currentAppliedEffects)
        {
            if(eff.GetEffect.Name == toCheck.Name)
            {
                return eff;
            }
        }
        return null;
    }
}
