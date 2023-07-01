using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AppliedEffect
{
    [SerializeField] private EffectScriptable effect;
    [SerializeField] private float durationLeft;
    [SerializeField] private int currentStack;

    private GameObject effectDisplay;

    public EffectScriptable GetEffect => effect;
    public float Duration => durationLeft;

    public void UpdateDuration()
    {
        if (durationLeft >= 0)
        {
            durationLeft-=.5f;
        }
    }

    public void ResetEffect(RVN_ComponentHandler toResetFrom, float resetDuration)
    {
        if (durationLeft < resetDuration)
        {
            durationLeft = resetDuration;
        }

        if(currentStack < effect.MaxStack)
        {
            currentStack++;
            ApplyEffect(toResetFrom);
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
        for (int i = 0; i < currentStack; i++)
        {
            foreach (Effect eff in effect.GetEffects)
            {
                eff.RemoveEffect(toRemoveFrom);
            }
        }

        if (effectDisplay != null)
        {
            GameObject.Destroy(effectDisplay); //TODO : Voir si on a besoin d'un pool ou pas
        }
    }

    public AppliedEffect()
    {

    }

    public AppliedEffect(EffectScriptable nEffect, GameObject effectDisplayer, float nDuration)
    {
        effect = nEffect;
        durationLeft = nDuration;
        effectDisplay = effectDisplayer;
        currentStack = 1;
    }
}
