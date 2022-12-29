using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AppliedEffect
{
    [SerializeField] private EffectScriptable effect;
    [SerializeField] private float durationLeft;

    public EffectScriptable GetEffect => effect;
    public float Duration => durationLeft;

    public void UpdateDuration()
    {
        if (durationLeft >= 0)
        {
            durationLeft-=.5f;
        }
    }

    public void ResetEffect(float resetDuration)
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

    public AppliedEffect(EffectScriptable nEffect, float nDuration)
    {
        effect = nEffect;
        durationLeft = nDuration;
    }
}
