using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_RemoveSpecificEffect : Effect
{
    [SerializeField] private List<EffectScriptable> effectToRemove = new List<EffectScriptable>();

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        if (effectTarget.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
        {
            foreach (EffectScriptable eff in effectToRemove)
            {
                effectHandler.RemoveEffect(eff);
            }
        }
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {
        
    }
}
