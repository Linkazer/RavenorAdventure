using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AppliedEffect
{
    [SerializeField] private EffectScriptable effect;
    [SerializeField] private int currentStack;

    private CPN_SpellCaster caster;

    private RVN_ComponentHandler target;

    private RoundTimer effectTimer;

    private GameObject effectDisplay;

    public Action actOnDurationUpdate;
    public Action actOnRemoveEffect;

    public CPN_SpellCaster Caster => caster;

    public EffectScriptable GetEffect => effect;
    public float Duration => effectTimer.roundLeft;
    public int Stacks => currentStack;

    public void UpdateDuration()
    {
        effectTimer.ProgressTimer(0.5f);
        actOnDurationUpdate?.Invoke();
    }

    public void ResetEffect(RVN_ComponentHandler toResetFrom, float resetDuration)
    {
        if (effectTimer.roundLeft < resetDuration)
        {
            effectTimer.roundLeft = resetDuration;
        }

        if(currentStack < effect.MaxStack)
        {
            currentStack++;
            ApplyEffect(toResetFrom, caster);
        }
    }

    public void ApplyEffect(RVN_ComponentHandler toAddFrom, CPN_SpellCaster nCaster)
    {
        caster = nCaster;

        target = toAddFrom;

        foreach (Effect eff in effect.GetEffects)
        {
            eff.ApplyEffect(toAddFrom, caster);
        }
    }

    private void UpdateRealtimeEffect()
    {
        foreach (Effect eff in effect.GetEffects)
        {
            if(eff.trigger == EffectTrigger.OnBeginTurn || eff.trigger == EffectTrigger.OnEndTurn)
            {
                eff.TryUseEffect(target, caster);
            }
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

        actOnRemoveEffect?.Invoke();

        target = null;
    }

    public AppliedEffect()
    {

    }

    public AppliedEffect(EffectScriptable nEffect, GameObject effectDisplayer, float nDuration, Action endEffectCallback)
    {
        effect = nEffect;
        effectDisplay = effectDisplayer;
        currentStack = 1;

        effectTimer = RVN_RoundManager.Instance.CreateTimer(nDuration, UpdateRealtimeEffect, endEffectCallback);
    }
}
