using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eff_SecondaryEffect : Effect
{
    [SerializeField] private EffectScriptable secondaryEffect;

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {
        if(effectTarget.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler targetEffectHandler))
        {
            targetEffectHandler.ApplyEffect(secondaryEffect, null);
        }
    }

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        if (effectTarget.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler targetEffectHandler))
        {
            targetEffectHandler.RemoveEffect(secondaryEffect);
        }
    }
}
