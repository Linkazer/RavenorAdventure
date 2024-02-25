using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPL_ANI_Fx : SPL_SpellActionAnimation
{
    [SerializeField] private InstantiatedAnimationHandler fxToPlay = null;

    public override float AnimationDuration => fxToPlay != null ? fxToPlay.PlayTime : animationDuration;

    protected override void OnPlayAnimation(SPL_SpellAction spellAction, SPL_SpellResolver resolver)
    {
        Action animationCallback = () => EndAnimation(resolver);

        foreach (Node node in GetAnimationNodes(spellAction, resolver))
        {
            AnimationInstantiater.PlayAnimationAtPosition(fxToPlay, node.worldPosition, animationCallback);
            animationCallback = null;
        }

        if(animationCallback != null)
        {
            animationCallback.Invoke();
        }
    }
}
