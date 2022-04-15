using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPN_AnimationHandler : RVN_Component<Animator>
{
    [SerializeField] protected Animator animator;

    public override void SetData(Animator toSet)
    {
        animator = toSet;
    }

    protected void AnimSetBool(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    protected void AnimSetTrigger(string parameter)
    {
        animator.SetTrigger(parameter);
    }
}
