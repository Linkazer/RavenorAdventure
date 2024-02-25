using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPL_CCondition_CharacterHasEffect : SPL_SpellActionChooserCondition
{
    [SerializeField] private SPL_CCondition_ConditionTarget conditionTarget;
    [SerializeField] private List<EffectScriptable> effectsWanted = new List<EffectScriptable>();

    public override bool IsConditionValid(SPL_CastedSpell castedSpellData)
    {
        List<EffectScriptable> effectsToCheck = new List<EffectScriptable>();

        List<CPN_EffectHandler> targetHandlers = new List<CPN_EffectHandler>();
        
        switch(conditionTarget)
        {
            case SPL_CCondition_ConditionTarget.Caster:
                if(castedSpellData.Caster.Handler.GetComponentOfType(out CPN_EffectHandler casterEffectHandler))
                {
                    targetHandlers = new List<CPN_EffectHandler>() { casterEffectHandler };
                }
                break;
            case SPL_CCondition_ConditionTarget.Target:
                targetHandlers = castedSpellData.TargetNode.GetNodeComponent<CPN_EffectHandler>();
                break;
        }
       

        foreach(CPN_EffectHandler handler in targetHandlers)
        {
            effectsToCheck = new List<EffectScriptable>(effectsWanted);

            for(int i = 0; i < effectsToCheck.Count; i++)
            {
                if (handler.HasEffect(effectsToCheck[i]))
                {
                    effectsToCheck.RemoveAt(i);
                    i--;
                }
            }

            if(effectsToCheck.Count == 0)
            {
                return true;
            }
        }

        return false;
    }
}
