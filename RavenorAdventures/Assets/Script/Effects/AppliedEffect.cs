using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AppliedEffect
{
    [SerializeField] private EffectScriptable effect;
    [SerializeField] private int durationLeft;

    public EffectScriptable GetEffect => effect;
    public int Duration => durationLeft;

    public void UpdateDuration()
    {
        durationLeft--;
    }

    public void ResetEffect(int resetDuration)
    {
        if (durationLeft < resetDuration)
        {
            durationLeft = resetDuration;
        }
    }

    public void ApplyEffect(RVN_ComponentHandler toAddFrom)
    {
        foreach (Effect eff in effect.GetEffects)
        {
            eff.ApplyEffect(toAddFrom);
        }
    }

    public void RemoveEffect(RVN_ComponentHandler toRemoveFrom)
    {
        foreach (Effect eff in effect.GetEffects)
        {
            eff.RemoveEffect(toRemoveFrom);
        }
    }

    public AppliedEffect()
    {

    }

    public AppliedEffect(EffectScriptable nEffect, int nDuration)
    {
        effect = nEffect;
        durationLeft = nDuration;
    }
}
