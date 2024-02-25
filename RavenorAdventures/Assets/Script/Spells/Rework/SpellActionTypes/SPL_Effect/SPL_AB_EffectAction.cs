using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPL_AB_EffectAction : SPL_SpellActionBehavior<SPL_AS_EffectAction>
{
    protected override void ResolveAction(SPL_AS_EffectAction actionToResolve, SPL_SpellResolver spellResolver)
    {
        foreach (Node node in actionToResolve.Shape.GetZone(spellResolver.CastedSpellData.Caster.CurrentNode, spellResolver.CastedSpellData.TargetNode))
        {
            List<CPN_EffectHandler> hitableObjects = node.GetNodeComponent<CPN_EffectHandler>();

            foreach (CPN_EffectHandler hitedObject in hitableObjects)
            {
                foreach (EffectScriptable effect in actionToResolve.EffectsOnTarget)
                {
                    hitedObject.ApplyEffect(effect, spellResolver.CastedSpellData.Caster);
                }
            }
        }

        foreach (SPL_SpellActionAnimation anim in actionToResolve.DamageAnimations)
        {
            anim.PlayAnimation(actionToResolve, spellResolver);
        }

        EndResolve(actionToResolve.GetNextAction(spellResolver.CastedSpellData), spellResolver);
    }
}
