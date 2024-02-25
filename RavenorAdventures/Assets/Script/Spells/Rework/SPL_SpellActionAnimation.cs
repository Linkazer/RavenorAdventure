using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SPL_SpellActionAnimation
{
    [SerializeField] protected SPL_AnimationTarget animationTarget;
    [SerializeField] protected float animationDuration;

    public abstract float AnimationDuration { get; }

    public void PlayAnimation(SPL_SpellAction spellAction, SPL_SpellResolver resolver)
    {
        resolver.OnAnimationLaunch(this);

        OnPlayAnimation(spellAction, resolver);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spellAction">Le SpellAction qui contient l'animation.</param>
    /// <param name="resolver">Le Resolver de l'action.</param>
    protected abstract void OnPlayAnimation(SPL_SpellAction spellAction, SPL_SpellResolver resolver);

    protected virtual void EndAnimation(SPL_SpellResolver resolver)
    {
        resolver.OnAnimationEnd(this);
    }

    protected List<Node> GetAnimationNodes(SPL_SpellAction spellAction, SPL_SpellResolver resolver)
    {
        switch(animationTarget)
        {
            case SPL_AnimationTarget.FullZone:
                return spellAction.Shape.GetZone(resolver.CastedSpellData.Caster.CurrentNode, resolver.CastedSpellData.TargetNode);
            case SPL_AnimationTarget.HandlersInZone:
                List<Node> zoneNode = spellAction.Shape.GetZone(resolver.CastedSpellData.Caster.CurrentNode, resolver.CastedSpellData.TargetNode);

                for(int i = 0; i < zoneNode.Count; i++)
                {
                    if (zoneNode[i].GetNodeHandler<RVN_ComponentHandler>().Count == 0)
                    {
                        zoneNode.RemoveAt(i);
                        i--;
                    }
                }
                return zoneNode;
            case SPL_AnimationTarget.CasterNode:
                return new List<Node>() { resolver.CastedSpellData.Caster.CurrentNode };
            case SPL_AnimationTarget.TargetNode:
                return new List<Node>() { resolver.CastedSpellData.TargetNode };
        }

        return new List<Node>();
    }
}