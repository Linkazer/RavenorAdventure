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
        foreach (Effect eff in toApply.GetEffects)
        {
            if (HasEffect(eff) != null)
            {
                HasEffect(eff).ResetEffect(3);
            }
            else
            {
                currentAppliedEffects.Add(new AppliedEffect(eff, 3));
                eff.ApplyEffect(Handler);
            }
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
        toRemove.RemoveEffect(handler);

        currentAppliedEffects.Remove(toRemove);
    }

    private AppliedEffect HasEffect(Effect toCheck)
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
