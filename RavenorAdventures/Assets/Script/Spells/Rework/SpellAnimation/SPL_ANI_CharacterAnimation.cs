using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPL_ANI_CharacterAnimation : SPL_SpellActionAnimation
{
    [SerializeField] private CharacterAnimationType casterAnimation = CharacterAnimationType.None;

    private TimerManager.Timer timer = null;

    public override float AnimationDuration => animationDuration;

    protected override void OnPlayAnimation(SPL_SpellAction spellAction, SPL_SpellResolver resolver)
    {
        foreach (Node node in GetAnimationNodes(spellAction, resolver))
        {
            foreach(RVN_ComponentHandler handler in node.GetNodeHandler<RVN_ComponentHandler>())
            {
                handler.animationController.PlayAnimation(casterAnimation.ToString(), resolver.CastedSpellData);
            }
        }

        timer = TimerManager.CreateGameTimer(animationDuration, () => EndAnimation(resolver));
    }

    protected override void EndAnimation(SPL_SpellResolver resolver)
    {
        base.EndAnimation(resolver);

        timer = null;
    }
}
