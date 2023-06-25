using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_AddEffect : SequenceAction
{
    [SerializeField] private EffectScriptable effectToAdd;
    [SerializeField] private RVN_ComponentHandler targetHandler;

    protected override void OnStartAction()
    {
        UE_AddEffect(targetHandler);
        EndAction();
    }

    protected override void OnEndAction()
    {

    }

    protected override void OnSkipAction()
    {

    }

    public void UE_AddEffect(RVN_ComponentHandler handler)
    {
        if(handler.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler caster))
        {
            caster.ApplyEffect(effectToAdd);
        }
    }
}